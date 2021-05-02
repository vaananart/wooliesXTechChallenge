using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using WooliesXTechChallengeApi.Inferfaces.Helpers;

namespace WooliesXTechChallengeApi.Implementations.Helpers
{
	public class HttpGETClientHelper : IHttpGETClientHelper
	{
		private const string serviceKeyword = "Service";
		private const string httpResourceBaseAddressConfigName = "wooliesXapis:BaseUrl";
		private const string httpTokenConfigName = "wooliesXApis:Token";
		private const string requiredParameterString = "?token=";
		private readonly ILogger<HttpGETClientHelper> _logger;

		//
		private readonly IHttpClientFactory _clientFactory;
		private readonly IConfiguration _configuration;

		public HttpGETClientHelper(ILogger<HttpGETClientHelper> logger
								, IHttpClientFactory clientFactory
								, IConfiguration configuration)
		{
			_logger = logger;
			_clientFactory = clientFactory;
			_configuration = configuration;
		}

		public async Task<string> CallGet<TService>()
		{
			var service = typeof(TService);
			var resourceName = service.Name.Substring(0, service.Name.IndexOf(serviceKeyword));

			var httpClient = _clientFactory.CreateClient();
			var requestString = _configuration[httpResourceBaseAddressConfigName]
								+ resourceName.ToLower()
								+ requiredParameterString
								+ _configuration[httpTokenConfigName];

			var response = String.Empty;
			try
			{
				using (var result = await httpClient.GetAsync(requestString))
				{
					if (result.IsSuccessStatusCode)
					{
						response = await result.Content.ReadAsStringAsync();
						_logger.LogInformation($"HttpGETClientHelper:CallGet<{service.Name}>() : Retrieved data successfully.");
					}
					else
					{
						_logger.LogError($"HttpGETClientHelper:CallGet<{service.Name}>() : Failed on GET call.");
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"HttpGETClientHelper:CallGet<{service.Name}>(): following exception thrown:{ ex.Message },  when calling following url :{requestString}");
			}
			return response;
		}

		public HttpClient CallPost()
		{
			throw new NotImplementedException();
		}
	}
}
