using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IUserProfileService
    {

        Task<string?> SaveFileAsync(IFormFile? file, string folder = "uploads");
        Task<bool> CreateUserProfileAsync(UserProfile Entity);
        Task<UserProfile> GetUserProfileById(string userId);
        Task<bool> UpdatingUserProfile(UserProfile Entity);
    }
}
