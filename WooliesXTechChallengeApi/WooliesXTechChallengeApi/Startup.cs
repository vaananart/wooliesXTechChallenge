using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using WooliesXTechChallengeApi.Implementations.Helpers;
using WooliesXTechChallengeApi.Implementations.Services;
using WooliesXTechChallengeApi.Inferfaces.Helpers;
using WooliesXTechChallengeApi.Inferfaces.Services;
using WooliesXTechChallengeApi.Middlewares;

namespace WooliesXTechChallengeApi
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
			services.AddApplicationInsightsTelemetry();
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WooliesXTechChallengeApi", Version = "v1" });
			});
			services.AddSingleton<IProductsService, ProductsService>()
					.AddSingleton<IShopperHistoryService, ShopperHistoryService>()
					.AddSingleton<ITrolleyService, TrolleyService>()
					.AddSingleton<IUserService, UserService>()
					.AddSingleton<IProductsService, ProductsService>()
					.AddSingleton<IShopperHistoryService, ShopperHistoryService>()
					.AddSingleton<ITrolleyService, TrolleyService>()
					.AddSingleton<IHttpGETClientHelper, HttpGETClientHelper>()
					.AddSingleton<IHttpPOSTClientHelper, HttpPOSTClientHelper>()
					.AddHttpClient()
					.AddAutoMapper(typeof(Startup));

			services.AddMvc()
					.AddNewtonsoftJson();
			services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseMiddleware<TracingMiddleware>();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WooliesXTechChallengeApi v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
