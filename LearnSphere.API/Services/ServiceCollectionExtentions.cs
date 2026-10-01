using FluentValidation;
using FluentValidation.AspNetCore;
using LearnSphere.API.Filters;
using LearnSphere.DAL.Context;
using LearnSphere.Repo.Implementations;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Services.AssemblyMarker;
using LearnSphere.Services.Implementations;
using LearnSphere.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
namespace LearnSphere.API.Services
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddInfrastructureServices(configuration);
            services.AddSwaggerWithJWT();
            services.AddAuthenticationWithJWT(configuration);
            services.AddAPIBehavior(configuration);
            return services;
           
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services,IConfiguration configuration)
        {
            services
                 .AddDbContext<ApplicationDbContext>(op => op
                 .UseLazyLoadingProxies(true)
                 .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJWTService,JWTService>();
            services.AddScoped<IFileService,FileService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<ILessonsService, LessonService>();
            services.AddScoped<IProgressService, ProgressService>();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();
            services.AddAutoMapper(au => { },typeof(ApplicationAssemblyMarker).Assembly);
            return services;
        }
        public static IServiceCollection AddAPIBehavior(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddControllers(x =>
            {
                x.Filters.Add<ErrorFilter>();
                x.Filters.Add<ResponseWrapperFilter>();
            }).AddJsonOptions(op =>
            {
                op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            return services;
        }
        public static IServiceCollection AddSwaggerWithJWT(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LearnSphere API",
                    Version = "v1",
                    Description = "LearnSphere API with JWT Authentication"
                });

                // Include XML Comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // JWT Authentication Scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token"
                });

                // Security Requirement (OpenAPI v2 style)
                c.AddSecurityRequirement(document =>
                {
                    return new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                    };
                });
            });

            return services;
        }
        //public static IServiceCollection AddSwaggerWithJWT(this IServiceCollection services)
        //{
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new OpenApiInfo
        //        {
        //            Title = "LearnSphere API",
        //            Version = "v1",
        //            Description = "LearnSphere API with JWT Authentication"
        //        });

        //        // JWT Authentication
        //        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //        {
        //            Name = "Authorization",
        //            Type = SecuritySchemeType.Http,
        //            Scheme = "Bearer",
        //            BearerFormat = "JWT",
        //            In = ParameterLocation.Header,
        //            Description = "Enter your JWT token"
        //        });

        //        c.AddSecurityRequirement(document =>
        //        {
        //            return new OpenApiSecurityRequirement
        //            {
        //                [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        //            };
        //        });
        //    });

        //        return services;
        //}
        public static IServiceCollection AddAuthenticationWithJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Auth:JWT:Key"]
                    ?? throw new InvalidOperationException("JWT Key is missing")));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    ValidateIssuer = false,
                   // ValidIssuer = configuration["Auth:JWT:Issuer"],

                    ValidateAudience = false,
                   // ValidAudience = configuration["Auth:JWT:Audience"],

                    ValidateLifetime = false,
                   // ClockSkew = TimeSpan.Zero
                };
            });


            return services;
        }
        //public static IServiceCollection AddAuthenticationWithJWT(this IServiceCollection services,IConfiguration configuration)
        //{
        //    services.AddAuthentication("MySchema")
        //        .AddJwtBearer("MySchema", op =>
        //        {
        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:JWT:Key"] ?? "this is my secret key it must be very secure and hidden"));
        //            op.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                IssuerSigningKey = key,
        //                ValidateAudience = false,
        //                ValidateLifetime=false,
        //                ValidateIssuer=false
        //            };
        //        });
        //    return services;

        //}
    }
}
