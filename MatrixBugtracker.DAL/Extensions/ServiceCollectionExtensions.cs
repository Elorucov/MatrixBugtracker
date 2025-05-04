using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatrixBugtracker.DAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<BugtrackerContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
