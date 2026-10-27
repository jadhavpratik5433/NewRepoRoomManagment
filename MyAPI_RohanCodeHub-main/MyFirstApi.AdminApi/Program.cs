
using Microsoft.EntityFrameworkCore;
using MyFirstApi.AdminApi.Entities;
using MyFirstApi.AdminApi.IService;
using MyFirstApi.AdminApi.Services;



namespace MyFirstApi.AdminApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


            builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
