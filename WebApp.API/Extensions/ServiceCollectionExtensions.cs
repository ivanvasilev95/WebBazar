using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApp.API.Data;
using WebApp.API.Helpers;
using WebApp.API.Models;
using WebApp.API.Services;
using WebApp.API.Services.Interfaces;

namespace WebApp.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration Configuration)
                => services
                    .AddDbContext<DataContext>(options => options
                        .UseMySql(Configuration.GetConnectionString("DbConnection")));
        
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            IdentityBuilder builder = services.AddIdentityCore<User>(opt => {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                // opt.User.RequireUniqueEmail = true;
            });
            
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            return services;
        }
        
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration Configuration)
        {
            var key = Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            return services;
        }

        public static IServiceCollection AddPolicyAuthorization(this IServiceCollection services)
            => services
                .AddAuthorization(options => {
                    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                    options.AddPolicy("ModerateAdRole", policy => policy.RequireRole("Admin", "Moderator"));
                });

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            => services
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<IAdminService, AdminService>()
                .AddTransient<ICategoryService, CategoryService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<ILikeService, LikeService>()
                .AddTransient<IPhotoService, PhotoService>()
                .AddTransient<IMessageService, MessageService>()
                .AddTransient<IAdService, AdService>()
                .AddScoped<LogUserActivity>();
                // .AddTransient<Seed>();

        public static IServiceCollection AddCloudinarySettingsConfiguration(
            this IServiceCollection services,
            IConfiguration Configuration)
                => services
                    .Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

        public static void AddApiControllers(this IServiceCollection services)
            => services
                .AddControllers(options => {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddNewtonsoftJson(x =>
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    }
}