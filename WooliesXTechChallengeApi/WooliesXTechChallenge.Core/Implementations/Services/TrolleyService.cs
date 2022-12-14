using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using WooliesXTechChallenge.Core.Inferfaces.Helpers;
using WooliesXTechChallenge.Core.Inferfaces.Services;

namespace WooliesXTechChallenge.Core.Implementations.Services;

public class TrolleyService : ITrolleyService
{
	private readonly ILogger _logger;
	private readonly IHttpPOSTClientHelper _trolleyResourceHttpClient;

	public TrolleyService(ILogger<TrolleyService> logger
							, IHttpPOSTClientHelper httpClientHelper)
	{
		_logger = logger;
		_trolleyResourceHttpClient = httpClientHelper;
	}

	public async Task<string> CalculateLowestTotal(JObject trolleyPayload)
	{
		var result = string.Empty;
		try
		{
			result = await _trolleyResourceHttpClient.CallPost<TrolleyService>(trolleyPayload);
			_logger.LogInformation($"TrolleyService:CalculateLowestTotal: payload is processed successfully.");
		}
		catch (Exception ex)
		{
			_logger.LogError($"TrolleyService:CalculateLowestTotal: Error: {ex.Message}");
			_logger.LogError($"TrolleyService:CalculateLowestTotal: Payload: {trolleyPayload.ToString()}");
			throw new Exception($"TrolleyService:CalculateLowestTotal: caught the following exception: {ex.Message}");
		}
		return result;
	}
}
