using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Identity;

namespace WooliesXTechChallengeApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.ConfigureLogging(builder => {
					builder.AddApplicationInsights("b2be7480-3c70-4e70-88c7-ec2036ce8eed");
					builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights
										.ApplicationInsightsLoggerProvider>("", LogLevel.Information);
					builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights
									.ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Error);
				});
	}
}
