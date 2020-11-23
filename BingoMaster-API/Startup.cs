using BingoMaster_API.HubConfig;
using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BingoMaster_API
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IBingoCardLogic, BingoCardLogic>();
            services.AddTransient<IBingoGameLogic, BingoGameLogic>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IBingoNumberLogic, BingoNumberLogic>();

			services.Configure<JwtSettingsModel>(Configuration.GetSection("Jwt"));

			services.AddCors();
            services.AddSignalR();

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Title = "BingoMaster";
                    document.Info.Description = "Application that helps you organize a bingo game";
                    document.Info.Version = "v1";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Stijn van de Sande"
                    };
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(builder => builder
                    .WithOrigins("http://localhost:4200")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<BingoGameHub>("/BingoGame");
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
