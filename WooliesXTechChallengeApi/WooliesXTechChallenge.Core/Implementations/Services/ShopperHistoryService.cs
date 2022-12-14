using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using WooliesXTechChallenge.Core.Inferfaces.Helpers;
using WooliesXTechChallenge.Core.Inferfaces.Services;
using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallenge.Core.Implementations.Services;

public class ShopperHistoryService : IShopperHistoryService
{
	private readonly ILogger<ShopperHistoryService> _logger;
	private readonly IHttpGETClientHelper _shopperHistoryResourceHttpClient;

	public ShopperHistoryService(ILogger<ShopperHistoryService> logger
								, IHttpGETClientHelper httpClientHelper)
	{
		_logger = logger;
		_shopperHistoryResourceHttpClient = httpClientHelper;
	}

	public async Task<IEnumerable<ShopperHistoryModel>> GetHistory()
	{
		var history = string.Empty;
		IEnumerable<ShopperHistoryModel> deserialisedCollection = null;
		try
		{
			history = await _shopperHistoryResourceHttpClient.CallGet<ShopperHistoryService>();
			deserialisedCollection = JsonConvert.DeserializeObject<IEnumerable<ShopperHistoryModel>>(history);
			_logger.LogInformation($"ShopperHistoryService:GetHistory: payload is processed successfully.");

		}
		catch (Exception ex)
		{
			_logger.LogError($"ShopperHistoryService:GetHistory: Error: {ex.Message}");
			if (!string.IsNullOrEmpty(history))
				_logger.LogError($"ShopperHistoryService:GetHistory: Captured result from the query: {history}");

			throw new Exception($"ShopperHistoryService:GetHistory: caught the following exception: {ex.Message}");

		}

		return deserialisedCollection;
	}
}
