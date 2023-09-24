using Microsoft.EntityFrameworkCore;

namespace UserTaskApi.Controllers
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddScoped(typeof(ICrudRepository<,>), typeof(CrudRepository<,>));
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // This check ensures you're not using SQLite during design-time
            if (!AppDomain.CurrentDomain.BaseDirectory.Contains("VisualStudio"))
            {
                services.AddDbContext<UserDbContext>(options =>
                    options.UseSqlite(config.GetConnectionString("DefaultConnection")));
            }
            
            return services;
        }
    }
}