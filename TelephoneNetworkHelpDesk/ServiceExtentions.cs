using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Entities;
using Entities.Models;
using Repository;
using Repository.AdministratorRepository;
using Repository.CustomerRepository;
using Repository.GuestRepository;
using Repository.OperatorRepository;
using Repository.CustomerAcquisitionRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BussinessLogic;
using TelephoneNetworkProvider.ActionFilters;
using Microsoft.OpenApi.Models;
using Logger;

namespace TelephoneNetworkProvider
{
    public static class ServiceExtentions
    {
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<ICustomerDataAcquisitionRepository, CustomerDataAcquisitionRepository>();
            services.AddScoped<IAdministratorManager, AdministratorManager>();
            services.AddScoped<IOperatorManager, OperatorManager>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IGuestManager, GuestManager>();
        }

        public static void ConfigureBussinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IUserManipulationLogic, UserManipulationLogic>();
            services.AddScoped<IAuthenticationLogic, AuthenticationLogic>();
            services.AddScoped<IAdministratorLogic, AdministratorLogic>();
            services.AddScoped<IOperatorLogic, OperatorLogic>();
            services.AddScoped<ICustomerLogic, CustomerLogic>();
            services.AddScoped<IGuestLogic, GuestLogic>();
        }

        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<DtoValidationFilterAttribute>();
            services.AddScoped<ParametersValidationFilterAttribute>();
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), 
                b => b.MigrationsAssembly("TelephoneNetworkProvider")));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(o =>
            {
                //o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                //o.User.RequireUniqueEmail = true;
                o.User.RequireUniqueEmail = false;
                //o.Lockout.
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Code Maze API",
                    Version = "v1"
                });
                s.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Code Maze API",
                    Version = "v2"
                });
            });
        }

        public static void ConfigureLogger(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
        }
    }
}
