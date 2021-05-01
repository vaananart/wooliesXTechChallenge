using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using WooliesXTechChallengeApi.Inferfaces.Helpers;

namespace WooliesXTechChallengeApi.Implementations.Helpers
{
	public class HttpClientHelper<TService> : IHttpClientHelper<TService>
	{
		private const string serviceKeyword = "Service";
		private const string httpResourceBaseAddressConfigName = "wooliesXapis:BaseUrl";
		private const string httpTokenConfigName = "wooliesXApis:Token";
		private const string requiredParameterString = "?token=";
		//
		private readonly IHttpClientFactory _clientFactory;
		private readonly IConfiguration _configuration;
		

		public HttpClientHelper(IHttpClientFactory clientFactory,
								IConfiguration configuration)
		{
			_clientFactory = clientFactory;
			_configuration = configuration;
		}
		
		public async Task<string> CallGet()
		{
			var serviceName = nameof(TService);
			var resourceName = serviceName.Substring(0, serviceName.IndexOf(serviceKeyword));

			var httpClient = _clientFactory.CreateClient();
			 var requestString = _configuration[httpResourceBaseAddressConfigName]
						+ requiredParameterString 
						+_configuration["httpTokenConfigName"];

			var result = await httpClient.GetAsync(requestString);
			return await result.Content.ReadAsStringAsync();
		}

		public HttpClient CallPost()
		{
			throw new NotImplementedException();
		}
	}
}
