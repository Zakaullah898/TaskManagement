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
        private readonly ITaskManagementRepo<AppUser> _AppUserRepo;
        IMapper _mapper;

        public AuthService(ITaskManagementRepo<AppUser> AppUserRepo, IMapper mapper) 
        { 
            _mapper = mapper;
            _AppUserRepo = AppUserRepo;
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await _AppUserRepo.GetAsync(u=> u.Id == id);
        }
        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _AppUserRepo.GetAllAsync();
        }


    }
}
