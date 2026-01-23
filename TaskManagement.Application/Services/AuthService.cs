using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITaskManagementRepo<AppUser> _userRepo;
        IMapper _mapper;

        public AuthService(ITaskManagementRepo<AppUser> userRepo, IMapper mapper) 
        { 
            _mapper = mapper;
            _userRepo = userRepo;
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await _userRepo.GetAsync(u=> u.Id == id);
        }
    }
}
