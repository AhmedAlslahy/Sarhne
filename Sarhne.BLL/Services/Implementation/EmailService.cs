using FluentValidation;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Email;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Enums;
using System.Security.Cryptography;
using static Sarhne.BLL.Abstraction.Errors;
using static Sarhne.BLL.Helper.HelperMethod;


namespace Sarhne.BLL.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IValidator<ConfirmEmailDto> _confirmVlidator;
        private readonly IValidator<ForgetPasswordDto> _forgetVlidator;
        private readonly IValidator<ResetPasswordDto> _resetVlidator;


        public EmailService(UserManager<User> userManager,IWebHostEnvironment env,IConfiguration _config,
            IValidator<ResetPasswordDto> _resetVlidator, IValidator<ForgetPasswordDto> _forgetVlidator
            , IValidator<ConfirmEmailDto> _confirmVlidator)
        {
            _userManager = userManager;
            this._config = _config;
            _env = env;
            this._resetVlidator = _resetVlidator;
            this._forgetVlidator = _forgetVlidator;
            this._confirmVlidator = _confirmVlidator;
        }

        public async Task<Response> EmailBody(string to, string subject, string body, CancellationToken cancellation = default)
        {
            using (var Client = new SmtpClient())
            {
                await Client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls, cancellation);
                await Client.AuthenticateAsync(_config["EmailInformations:Email"], _config["EmailInformations:Password"], cancellation);

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body,
                };

                var message = new MimeMessage
                {
                    Body = bodyBuilder.ToMessageBody()
                };
                message.From.Add(MailboxAddress.Parse(_config["EmailInformations:Email"]));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;
                await Client.SendAsync(message, cancellation);
                await Client.DisconnectAsync(true, cancellation);
            }
            return Response.Success();
        }

        public async Task<Response> SendConfirmEmailOTP(string email, CancellationToken cancellation = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }

            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            user.OTP = otp;
            user.OTPExpire= DateTime.UtcNow.AddMinutes(5);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Response.Fail(new Error("Update Failed", "Cannot update OTP", ErrorType.BadRequest));
            }

            var path = Path.Combine(_env.WebRootPath, "Templates", "EmailConfirme.html");
            var htmlBody = await File.ReadAllTextAsync(path,cancellation);
            htmlBody = htmlBody.Replace("{{Name}}", user.FullName);
            htmlBody = htmlBody.Replace("{{OTP}}", otp);
            await EmailBody(user.Email, "Confirm Email", htmlBody,cancellation);

            return Response.Success();
        }

        public async Task<Response> SendForgetPasswordOTP(string email, CancellationToken cancellation = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }

            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            user.OTP = otp;
            user.OTPExpire = DateTime.UtcNow.AddMinutes(5);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Response.Fail(new Error("Update Failed", "Cannot update OTP", ErrorType.BadRequest));
            }

            var path = Path.Combine(_env.WebRootPath, "Templates", "ForgetPassword.html");
            var htmlBody = await File.ReadAllTextAsync(path, cancellation);
            htmlBody = htmlBody.Replace("{{Name}}", user.FullName);
            htmlBody = htmlBody.Replace("{{OTP}}", otp);
            await EmailBody(user.Email, "Forget Password", htmlBody, cancellation);

            return Response.Success();
        }

        public async Task<Response> ConfirmEmail(ConfirmEmailDto dto,string userId)
        {
            var validationResult = await _confirmVlidator.ValidateAsync(dto);
            var error = ValidationHelper.Validate(validationResult);
            if (error != null)
            {
                return Response.Fail(error);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }
            if(dto.OTP!=user.OTP || DateTime.UtcNow > user.OTPExpire)
            {
                return Response.Fail(new Error("The OTP Is Wrong Or Expire", "Cannot Confirm Email",ErrorType.BadRequest));
            }

            if (user.EmailConfirmed)
            {
                return Response.Fail(new Error( "Email already confirmed", "Cannot Confirm Email",ErrorType.BadRequest));
            }
            user.OTP = null;
            user.OTPExpire = null;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return Response.Success();
        }

        public async Task<Response> ForgetPassword(ForgetPasswordDto dto , string email)
        {
            var validationResult = await _forgetVlidator.ValidateAsync(dto);

            var error = ValidationHelper.Validate(validationResult);

            if (error != null)
            {
                return Response.Fail(error);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }
            if(dto.OTP!=user.OTP || DateTime.UtcNow > user.OTPExpire)
            {
                return Response.Fail(new Error("The OTP Is Wrong Or Expire", "Cannot add new password",ErrorType.BadRequest));
            }
            user.OTP = null;
            user.OTPExpire = null;
            var passwordHash = _userManager.PasswordHasher.HashPassword(user, dto.NewPassword);
            user.PasswordHash = passwordHash;
            await _userManager.UpdateAsync(user);

            return Response.Success();
        }

        public async Task<Response> ResetPassword(ResetPasswordDto dto ,string userId)
        {
            var validationResult = await _resetVlidator.ValidateAsync(dto);

            var error = ValidationHelper.Validate(validationResult);

            if (error != null)
            {
                return Response.Fail(error);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }

              var verify = _userManager.PasswordHasher
             .VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if (verify == PasswordVerificationResult.Failed)
            {
                return Response.Fail(new Error("Wrong Password","Current password is incorrect",ErrorType.BadRequest));
            }

            var NewpasswordHash = _userManager.PasswordHasher.HashPassword(user, dto.NewPassword);
            user.PasswordHash = NewpasswordHash;
            await _userManager.UpdateAsync(user);

            return Response.Success();
        }
    }
}