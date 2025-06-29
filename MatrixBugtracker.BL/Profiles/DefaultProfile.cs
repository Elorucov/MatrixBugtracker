using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.DTOs.Tags;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MatrixBugtracker.BL.Profiles
{
    public class DefaultProfile : Profile
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private IMapper Mapper => _contextAccessor.HttpContext.RequestServices.GetService<IMapper>();
        private IUserIdProvider UserIdProvider => _contextAccessor.HttpContext.RequestServices.GetService<IUserIdProvider>();

        public EnumValueDTO GetTranslatedEnum { get; private set; }

        public DefaultProfile(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            CreateMap<UploadedFile, FileDTO>().AfterMap(ToFileDTO);
            CreateMap<User, UserDTO>().AfterMap(ToUserDTO);

            CreateMap<RegisterRequestDTO, User>().ReverseMap();
            CreateMap<UserEditDTO, User>().ReverseMap();

            CreateMap<ProductCreateDTO, Product>().ReverseMap();
            CreateMap<ProductEditDTO, Product>().ReverseMap();
            CreateMap<Product, ProductDTO>().AfterMap(ToProductDTO);

            CreateMap<Tag, TagDTO>();

            CreateMap<ReportCreateDTO, Report>()
                .ForMember(m => m.Tags, t => t.Ignore())
                .ForMember(m => m.Attachments, t => t.Ignore());

            CreateMap<ReportEditDTO, Report>()
                .ForMember(m => m.Tags, t => t.Ignore())
                .ForMember(m => m.Attachments, t => t.Ignore());

            CreateMap<Report, ReportDTO>()
                .ForMember(m => m.Severity, t => t.Ignore())
                .ForMember(m => m.ProblemType, t => t.Ignore())
                .ForMember(m => m.Status, t => t.Ignore())
                .ForMember(m => m.Attachments, t => t.Ignore())
                .ForMember(m => m.Tags, t => t.Ignore())
                .ForMember(m => m.Reproduces, t => t.Ignore())
                .AfterMap(ToReportDTO);

            CreateMap<CommentCreateDTO, Comment>()
                .ForMember(m => m.Attachments, t => t.Ignore());

            CreateMap<CommentEditDTO, Comment>()
                .ForMember(m => m.Attachments, t => t.Ignore());

            CreateMap<Comment, CommentDTO>()
                .ForMember(m => m.NewSeverity, t => t.Ignore())
                .ForMember(m => m.NewStatus, t => t.Ignore())
                .ForMember(m => m.Attachments, t => t.Ignore())
                .ForMember(m => m.Author, t => t.Ignore())
                .AfterMap(ToCommentDTO);
        }

        private void ToFileDTO(UploadedFile file, FileDTO dto)
        {
            // Only file owner/creator can know its id
            int currentUserId = UserIdProvider.UserId;
            dto.FileId = currentUserId == file.CreatorId ? file.Id : null;

            var uri = _contextAccessor.HttpContext.Request;
            string link = $"{uri.Scheme}://{uri.Host}/api/v1/files/{file.Path}";
            dto.Url = link;
        }

        private void ToUserDTO(User user, UserDTO dto)
        {

            if (user.PhotoFile != null)
            {
                dto.Photo = Mapper.Map<FileDTO>(user.PhotoFile);
            }
        }

        private void ToProductDTO(Product product, ProductDTO dto)
        {
            if (product.ProductMembers != null)
            {
                int currentUserId = UserIdProvider.UserId;
                ProductMemberStatus status = product.ProductMembers
                    .Where(pm => pm.MemberId == currentUserId)
                    .Select(pm => pm.Status).FirstOrDefault();

                dto.MembershipStatus = status;
            }

            if (product.PhotoFile != null)
            {
                dto.Photo = Mapper.Map<FileDTO>(product.PhotoFile);
            }
        }

        private void ToReportDTO(Report report, ReportDTO dto)
        {
            int currentUserId = UserIdProvider.UserId;

            dto.Severity = report.Severity.GetTranslatedEnum();
            dto.ProblemType = report.ProblemType.GetTranslatedEnum();
            dto.Status = report.Status.GetTranslatedEnum();

            dto.Attachments = Mapper.Map<List<FileDTO>>(report.Attachments.Select(a => a.File));
            dto.IsAttachmentsPrivate = report.IsAttachmentsPrivate;

            dto.Tags = report.Tags.Select(t => t.Tag.Name).ToList();

            dto.CanDelete = report.CreatorId == currentUserId && report.Status == ReportStatus.Open
                && report.CreationTime.AddHours(24) >= DateTime.Now;
        }

        private void ToCommentDTO(Comment comment, CommentDTO dto)
        {
            int currentUserId = UserIdProvider.UserId;

            if (comment.NewSeverity.HasValue) dto.NewSeverity = comment.NewSeverity.Value.GetTranslatedEnum();
            if (comment.NewStatus.HasValue) dto.NewStatus = comment.NewStatus.Value.GetTranslatedEnum();

            dto.Attachments = Mapper.Map<List<FileDTO>>(comment.Attachments.Select(a => a.File));
            dto.IsAttachmentsPrivate = comment.IsAttachmentsPrivate;

            dto.CanDelete = comment.CreatorId == currentUserId && !comment.NewStatus.HasValue
                && comment.NewStatus.HasValue && comment.CreationTime.AddHours(24) >= DateTime.Now;
        }
    }
}
