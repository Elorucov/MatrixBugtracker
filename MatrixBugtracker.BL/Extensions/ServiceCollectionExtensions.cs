using FluentValidation;
using FluentValidation.AspNetCore;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.Profiles;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.BL.Services.Implementations;
using MatrixBugtracker.BL.Validators.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MatrixBugtracker.BL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            IServiceProvider provider = services.BuildServiceProvider();

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<SetRoleRequestValidator>();

            services.AddAutoMapper(opt =>
            {
                opt.AddProfile(new DefaultProfile(provider.GetService<IHttpContextAccessor>()));
            });

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IGenerator, Generator>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccessService, AccessService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ITagsService, TagsService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IReportsService, ReportsService>();
        }
    }
}
