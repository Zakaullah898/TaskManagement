using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Automappr
{
    public class AutoMapperConfig :Profile
    {
        public AutoMapperConfig()
        {
            // Add your mappings here
            // CreateMap<Source, Destination>();
            CreateMap<RegisterUserDTO, AppUser>().ReverseMap();
            CreateMap<TaskTableDTO, TaskTable>().ReverseMap();
        }
    }
}
