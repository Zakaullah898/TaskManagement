using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITaskManagementRepo<AppUser> _AppUserRepo;
        private readonly IHelperMethods _helperMethods;
        private readonly IConfiguration _configuration;
        IMapper _mapper;

        public AuthService(
            IHelperMethods helperMethods,
            ITaskManagementRepo<AppUser> AppUserRepo, 
            IMapper mapper,
            IConfiguration configuration) 
        { 
            _mapper = mapper;
            _AppUserRepo = AppUserRepo;
            _helperMethods = helperMethods;
            _configuration = configuration;
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await _AppUserRepo.GetAsync(u=> u.Id == id);
        }
        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _AppUserRepo.GetAllAsync();
        }

        public async Task<bool> VarifyEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            var user = await _AppUserRepo.GetAsync(u => u.Email == email,true);
            if (user != null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        // method for reseting password
        public async Task<bool> ResetPassword(string password, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Email and Password are required and should not be null or empty.");
            }

            // TODO: Add your password reset logic here (e.g., update in DB)
            var user = await _AppUserRepo.GetAsync(u => u.Email == email, true);
            var result = _helperMethods.HashPassword(password);
            user!.PasswordHash = result.Hash;
            user.Salt = result.Salt;
            var isUpdatedPassword = await _AppUserRepo.UpdateAsync(user);
            if (isUpdatedPassword) 
            {
                return true;
            }
            else 
            {
                return false;
            }
            
        }


        public async Task SendOtpEmail(string email, string otp)
        {
            var host = _configuration["SmtpSettings:Host"];
            var port = Convert.ToInt32(_configuration["SmtpSettings:Port"]);
            var userName = _configuration["SmtpSettings:UserName"];
            var password = _configuration["SmtpSettings:Password"];
            var message = new MailMessage();
            message.To.Add(email);
            message.Subject = "Password Reset OTP";
            message.Body = $"Your OTP is: {otp}. It will expire in 5 minutes.";
            message.From = new MailAddress(userName!);

            using var smtp = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await _AppUserRepo.GetAsync(u => u.Email == email, true);
            return user;
        }
    }
}
