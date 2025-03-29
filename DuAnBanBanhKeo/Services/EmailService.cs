using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace DuAnBanBanhKeo.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _emailSender = "nguyenhoanganh28052005@gmail.com";
        private readonly string _emailPassword = "dvto pzzh nxrw jtep";

        public async Task SendEmailAsync(string toEmail, string subject, string otpCode)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Arise - Hỗ Trợ", _emailSender));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            string emailBody = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        padding: 20px;
                        text-align: center;
                    }}
                    .container {{
                        max-width: 500px;
                        background: white;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0px 4px 10px rgba(0,0,0,0.1);
                        margin: auto;
                    }}
                    h2 {{
                        color: #333;
                    }}
                    .otp {{
                        font-size: 24px;
                        font-weight: bold;
                        color: #ffffff;
                        background: #007bff;
                        padding: 10px 20px;
                        display: inline-block;
                        border-radius: 5px;
                        margin: 15px 0;
                    }}
                    p {{
                        color: #666;
                        font-size: 14px;
                    }}
                    .footer {{
                        margin-top: 20px;
                        font-size: 12px;
                        color: #888;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>🔐 Mã OTP của bạn</h2>
                    <p>Vui lòng sử dụng mã OTP dưới đây để đặt lại mật khẩu của bạn:</p>
                    <div class='otp'>{otpCode}</div>
                    <p>Mã OTP có hiệu lực trong <b>5 phút</b>. Không chia sẻ mã này với bất kỳ ai.</p>
                    <p>📧 Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                    <div class='footer'>© 2025 Arise Support Team</div>
                </div>
            </body>
            </html>";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailBody
            };
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, false);
                await smtp.AuthenticateAsync(_emailSender, _emailPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
