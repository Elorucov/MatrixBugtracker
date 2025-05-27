using FluentValidation;
using FluentValidation.AspNetCore;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.Profiles;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.BL.Services.Implementations;
using MatrixBugtracker.BL.Validators;
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
            services.AddValidatorsFromAssemblyContaining<TestValidator>();

            services.AddAutoMapper(opt =>
            {
                opt.AddProfile(new DefaultProfile(provider.GetService<IHttpContextAccessor>()));
            });

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPlatformService, PlatformService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccessService, AccessService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITagsService, TagsService>();
        }
    }
}
