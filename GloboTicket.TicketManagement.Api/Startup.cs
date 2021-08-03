using GloboTicket.TicketManagement.Api.Middleware;
using GloboTicket.TicketManagement.Api.Services;
using GloboTicket.TicketManagement.Application;
using GloboTicket.TicketManagement.Application.Contracts;
using GloboTicket.TicketManagement.Infrastructure;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GloboTicket.TicketManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Register services from external projects/libraries
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);
            services.AddPersistenceServices(Configuration);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILoggedInUserService, LoggedInUserService>();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GloboTicket.TicketManagement.Api",
                    Version = "v1"
                });

                // Ability to test excel file download with Swagger
                //c.OperationFilter<FileResultContentTypeOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GloboTicket.TicketManagement.Api v1"));
            }

            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("Open");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
