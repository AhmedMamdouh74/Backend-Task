
using Application.Services;
using Infrastructure.DI;
using Application.DI;
using Microsoft.AspNetCore.Builder;

namespace BackendTask
{
    public class Program
    {
        public static void Main(string[] args)

        {
            var builder = WebApplication.CreateBuilder(args);
           // builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

           


            builder.Services.AddInfrastructure();


            builder.Services.AddApplication();



            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
           
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseForwardedHeaders();


            app.MapControllers();

            app.Run();
        }
    }
}
