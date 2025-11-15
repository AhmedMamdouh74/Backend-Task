using Application.DI;
using Infrastructure.DI;

namespace BackendTask
{
    public class Program
    {
        public static void Main(string[] args)

        {
            var builder = WebApplication.CreateBuilder(args);





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
         //   app.UseMiddleware<AdminSafeListMiddleware>(builder.Configuration["AdminSafeList"]);


            

            app.MapControllers();

            app.Run();
        }
    }
}
