using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITaskManagementRepo<AppUser> _AppUserRepo;
        private readonly IHelperMethods _helperMethods;
        IMapper _mapper;

        public AuthService(
            IHelperMethods helperMethods,
            ITaskManagementRepo<AppUser> AppUserRepo, 
            IMapper mapper) 
        { 
            _mapper = mapper;
            _AppUserRepo = AppUserRepo;
            _helperMethods = helperMethods;
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
            var user = await _AppUserRepo.GetAsync(u => u.Email == email);
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



    }
}
