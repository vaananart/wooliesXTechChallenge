using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using WooliesXTechChallenge.Core.Inferfaces.Helpers;

namespace WooliesXTechChallenge.Core.Implementations.Helpers;

public class HttpPOSTClientHelper : IHttpPOSTClientHelper
{
	private const string serviceKeyword = "Service";
	private const string httpResourceBaseAddressConfigName = "wooliesXApis:BaseUrl";
	private const string httpTokenConfigName = "wooliesXApis:Token";
	private const string requiredParameterString = "?token=";
	private const string resourceSection = "wooliesXApis:Resources:";
	private const string mediaType = "application/json";
	//
	private readonly ILogger _logger;
	private readonly IHttpClientFactory _clientFactory;
	private readonly IConfiguration _configuration;

	public HttpPOSTClientHelper(ILogger<HttpPOSTClientHelper> logger
					, IHttpClientFactory clientFactory
					, IConfiguration configuration)
	{
		_logger = logger;
		_clientFactory = clientFactory;
		_configuration = configuration;
	}

	public async Task<string> CallPost<TService>(JObject jsonPayload)
	{
		var service = typeof(TService);
		var resourceName = service.Name.Substring(0, service.Name.IndexOf(serviceKeyword));

		var httpClient = _clientFactory.CreateClient();
		var requestString = _configuration[httpResourceBaseAddressConfigName]
							+ (_configuration[resourceSection + resourceName] ?? resourceName.ToLower())
							+ requiredParameterString
							+ _configuration[httpTokenConfigName];
		var stringPayload = jsonPayload.ToString();
		var stringContent = new StringContent(stringPayload, Encoding.UTF8, mediaType);
		var response = string.Empty;

		try
		{
			using (var result = await httpClient.PostAsync(requestString, stringContent))
			{
				if (result.IsSuccessStatusCode)
				{
					response = await result.Content.ReadAsStringAsync();
					_logger.LogInformation($"HttpPOSTClientHelper:CallPost<{service.Name}>() : Posted data successfully.");
				}
				else
				{
					_logger.LogError($"HttpPOSTClientHelper:CallPost<{service.Name}>() : failed on POST call.");

				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception($"HttpPOSTClientHelper:CallPost<{service.Name}>(): " +
								$"following exception thrown:{ex.Message}" +
								$", when calling following url :{requestString} and the payload as follows: {stringPayload}");

		}

		return response;
	}
}
