using BingoMaster_API.HubConfig;
using BingoMaster_Entities;
using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using BingoMaster_API.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using BingoMaster_Logic.Exceptions;
using System.Net;

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
			var builder = new SqlConnectionStringBuilder(
				Configuration.GetConnectionString("Database"));
			builder.Password = Configuration["DbPassword"];
			builder.UserID = Configuration["UserId"];

			services.AddControllers();
			services.AddDbContext<BingoMasterDbContext>(
				options => options.UseSqlServer(builder.ConnectionString));

			services.AddTransient<IBingoCardLogic, BingoCardLogic>();
			services.AddTransient<IBingoGameLogic, BingoGameLogic>();
			services.AddTransient<IPasswordLogic, PasswordLogic>();
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

			app.UseMiddleware<JwtMiddleware>();

			app.UseExceptionHandler(builder => builder.Run(async context => 
			{
				var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
				var exception = exceptionHandlerPathFeature.Error;

				if (exception is UserAlreadyExistsException)
				{
					await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorModel()
					{
						StatusCode = (int)HttpStatusCode.BadRequest,
						Message = exception.Message
					}));
				}

				await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorModel()
				{
					StatusCode = context.Response.StatusCode,
					Message = exception.Message
				}));
			}));

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
