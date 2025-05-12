using AutoMapper;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MatrixBugtracker.BL.Profiles
{
    public class DefaultProfile : Profile
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private IMapper Mapper => _contextAccessor.HttpContext.RequestServices.GetService<IMapper>();
        public DefaultProfile(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            CreateMap<UploadedFile, FileDTO>().AfterMap(ToFileDTO);
            CreateMap<User, UserDTO>().AfterMap(ToUserDTO);

            CreateMap<RegisterRequestDTO, User>().ReverseMap();
            CreateMap<UserEditDTO, User>().ReverseMap();
        }

        private void ToFileDTO(UploadedFile file, FileDTO dto)
        {
            var uri = _contextAccessor.HttpContext.Request;
            string link = $"{uri.Scheme}://{uri.Host}/file/{file.Path}";
            dto.Url = link;
        }

        private void ToUserDTO(User user, UserDTO dto)
        {

            if (user.PhotoFile != null)
            {
                dto.Photo = Mapper.Map<FileDTO>(user.PhotoFile);
            }
        }
    }
}
