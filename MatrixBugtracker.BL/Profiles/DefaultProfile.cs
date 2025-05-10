using AutoMapper;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Profiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<RegisterRequestDTO, User>().ReverseMap();
        }
    }
}
