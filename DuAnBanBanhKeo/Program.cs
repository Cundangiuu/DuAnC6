using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Modal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyFrontend", builder =>
    {
        builder.WithOrigins(
            "http://127.0.0.1:5501", // Máy chủ ảo của bạn
            "https://nhom-arise.netlify.app" // Trang web Netlify của bạn
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

// Configure database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configure Authentication and Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // Trong phát triển, có thể tắt
        ValidateAudience = false, // Trong phát triển, có thể tắt
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"]
                ?? throw new InvalidOperationException("Missing JWT SecurityKey")))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Quản lý kho", policy => policy.RequireRole("quản lý kho"));
    options.AddPolicy("Nhân viên", policy => policy.RequireRole("nhân viên"));
});

builder.Services.AddAutoMapper(typeof(Program));

// Swagger setup for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "DuAnBanBanhKeo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Hiển thị chi tiết lỗi trong môi trường phát triển
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DuAnBanBanhKeo API V1");
    });
}

app.UseHttpsRedirection();

// Sử dụng CORS trước Authentication và Authorization
app.UseCors("AllowMyFrontend"); // Sử dụng policy đã cấu hình

app.UseStaticFiles(); // Phục vụ file tĩnh (hình ảnh trong wwwroot)

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();