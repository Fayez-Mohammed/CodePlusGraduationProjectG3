
using LearnSphere.API.Filters;
using LearnSphere.API.MiddleWare;
using LearnSphere.API.Services;
using LearnSphere.DAL.Context;
using LearnSphere.DAL.Data;
using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.Repo.Implementations;
using LearnSphere.Services.AssemblyMarker;
using LearnSphere.Services.Hubs;
using LearnSphere.Services.Implementations;
using LearnSphere.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace LearnSphere
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
                    //policy
                    //    .WithOrigins(
                    //        "http://localhost:3000",
                    //        "http://localhost:5173"
                    //    )
                    //    .AllowAnyHeader()
                    //    .AllowAnyMethod();
                });
            });
            const long maxVideoUploadSize = 1000L * 1024L * 1024L;

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = maxVideoUploadSize;
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = maxVideoUploadSize;
            });
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddSignalR();
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
                await IdentitySeeder.AddAdminAsync(scope.ServiceProvider);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Base API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlingMiddleWare>();
            app.UseStaticFiles();
            app.UseCors("AllowFrontend");
           
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<ChatHub>("/chatHub");

            app.MapControllers();

            app.Run();
        }
    }
}
