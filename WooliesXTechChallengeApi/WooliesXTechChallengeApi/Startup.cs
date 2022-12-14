using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using WooliesXTechChallenge.Core.Implementations.Helpers;
using WooliesXTechChallenge.Core.Implementations.Services;
using WooliesXTechChallenge.Core.Inferfaces.Helpers;
using WooliesXTechChallenge.Core.Inferfaces.Services;

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
					.AddAutoMapper(typeof(WooliesXTechChallenge.Util.Utils.Mapping.ProductMappingProfile)
					, typeof(WooliesXTechChallenge.Util.Utils.Mapping.UserDetailsMappingProfile));

			services.AddMvc()
					.AddNewtonsoftJson();
			services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//NOTE:JG: Uncomment the following to diagnose the raw HTTP request
			//app.UseMiddleware<TracingMiddleware>();

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
