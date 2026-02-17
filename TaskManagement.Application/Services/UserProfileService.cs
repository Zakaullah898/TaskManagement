using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.CustomException;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IConfiguration? _configuration;
        private readonly ITaskManagementRepo<UserProfile> _UserProfileRepo;
        private readonly ITaskManagementRepo<AppUser> _AppUserRepo;

        public UserProfileService(
            IConfiguration? configuration,
            ITaskManagementRepo<UserProfile> UserProfileRepo,
            ITaskManagementRepo<AppUser> appUserRepo)
        {
            _configuration = configuration;
            _UserProfileRepo = UserProfileRepo;
            _AppUserRepo = appUserRepo;
        }

        public async Task<string?> SaveFileAsync(IFormFile? file, string folder = "uploads")
        {
            if (file == null || file.Length == 0)
                return null;

            // Ensure directory exists
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Create unique, safe file name
            var originalName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(originalName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file asynchronously
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Get base URL from appsettings.json if available
            var baseUrl = _configuration!["AppSettings:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                // Fallback to relative URL if not configured
                return $"/{folder}/{uniqueFileName}";
            }

            // Return absolute URL
            return $"{baseUrl}/{folder}/{uniqueFileName}";
        }


        // Creating user profile
        public async Task<bool> CreateUserProfileAsync(UserProfile Entity)
        {
            Console.WriteLine("Creating user profile...");
            if (Entity == null)
            {
                throw new ArgumentNullException(nameof(Entity), "UserProfileDTO cannot be null");
            }
            var user = await _AppUserRepo.GetAsync(u => u.Email == Entity.Email);
            Console.WriteLine($"User Id {user.Id}");
            if (user == null)
            {
                throw new KeyNotFoundException($"User not found with email {Entity.Email}.");
            }
            Entity.UserId = user.Id;
            var ExistingProfile = await _UserProfileRepo.GetAsync(up => up.UserId == Entity.UserId, true);
            if (ExistingProfile != null)
            {
                throw new ConflictException("User profile already exists for this user. Please use update method for updation");
            }
            //// Save uploaded files via file storage service
            //Console.WriteLine("Saving uploaded files...");
            //var profileImageUrl = await SaveFileAsync(Entity.ProfileImage);
            //Console.WriteLine("Files saved successfully.");
            // Map DTO to entity

            //Entity.ProfileImagePath = profileImageUrl;

            //userProfile.CreatedAt = DateTime.UtcNow;
            //userProfile.UpdatedAt = DateTime.UtcNow;
            var isCreated = await _UserProfileRepo.CreateAsync(Entity);
            if (isCreated)
            {
                user.HasProfile = true;
                await _AppUserRepo.UpdateAsync(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UserProfile> GetUserProfileById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id should not be null or empty");
            }
            var userProfile = await _UserProfileRepo.GetAsync(u => u.UserId == userId);
            if (userProfile == null) 
            {
                throw new KeyNotFoundException($"Not found User profile with this user id {userId}");
            }
            return userProfile;
        }

        public async Task<bool> UpdatingUserProfile(UserProfile Entity)
        {
            if (Entity == null)
            {
                throw new ArgumentNullException("Entity shouldn't be null");
            }
            var existingProfile = await _UserProfileRepo.GetAsync(p => p.UserId == Entity.UserId);
            if(existingProfile != null) 
            {
                existingProfile.FirstName = Entity.FirstName;
                existingProfile.LastName = Entity.LastName;
                existingProfile.Email = Entity.Email;
                existingProfile.UserName = Entity.UserName;
                existingProfile.PhoneNumber = Entity.PhoneNumber;
                existingProfile.JobTitle = Entity.JobTitle;
                existingProfile.DateJoined = Entity.DateJoined;
                existingProfile.ProfileImagePath = Entity.ProfileImagePath;
                var isUpdated = await _UserProfileRepo.UpdateAsync(existingProfile);
                if (isUpdated) 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new KeyNotFoundException("User Profie not found please create new profile..");
            }
        }
    }
}
