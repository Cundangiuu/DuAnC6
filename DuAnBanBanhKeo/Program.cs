<<<<<<< Updated upstream
﻿//using DuAnBanBanhKeo.Responsive;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Modal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
=======
﻿using DuAnBanBanhKeo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DuAnBanBanhKeo.Controllers;
>>>>>>> Stashed changes

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors();
// Configure database context (replace with your actual connection string)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
<<<<<<< Updated upstream
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
// Register your services
// Cấu hình xác thực và phân quyền
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Không cần kiểm tra Issuer
            ValidateAudience = false, // Không cần kiểm tra Audience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:securitykey"] ?? throw new InvalidOperationException("Missing security key")))
        };
    });
// Thêm Authorization với các policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Quản lý kho", policy => policy.RequireRole("quản lý kho"));
    options.AddPolicy("Nhân viên", policy => policy.RequireRole("nhân viên"));
});
builder.Services.AddAutoMapper(typeof(Program));
=======
>>>>>>> Stashed changes

// Swagger setup for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
<<<<<<< Updated upstream
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
=======
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


// **Thêm CORS services**
// **Thêm CORS services**
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5501", "http://localhost:5501", "https://localhost:7203") // Origin của frontend và API
                   .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH") // Thêm PATCH
                   .AllowAnyHeader()
                   .AllowCredentials(); // Quan trọng nếu bạn muốn gửi cookies hoặc authorization headers
        });
});

// **Cấu hình JWT Authentication**
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"], // Lấy từ appsettings.json
            ValidAudience = builder.Configuration["JwtSettings:Audience"], // Lấy từ appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"])) // Lấy từ appsettings.json
        };
    });

// Thêm Authorization
builder.Services.AddAuthorization();


>>>>>>> Stashed changes
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.UseDirectoryBrowser(); // Chỉ dùng trong development để debug (nếu cần)
}
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
   .SetIsOriginAllowed(origin => true)
    .AllowCredentials());
app.UseHttpsRedirection();

<<<<<<< Updated upstream
=======
// **Sử dụng CORS middleware (ĐẢM BẢO NÓ NẰM TRƯỚC app.UseStaticFiles() và app.UseAuthorization())**
app.UseCors("AllowMyOrigin");

app.UseStaticFiles(); // ***ĐÃ THÊM DÒNG NÀY***

// **Sử dụng Authentication và Authorization middleware (thứ tự quan trọng!)**
>>>>>>> Stashed changes
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();