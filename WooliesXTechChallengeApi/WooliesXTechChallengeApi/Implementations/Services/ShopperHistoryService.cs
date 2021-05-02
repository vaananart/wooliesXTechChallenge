using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using WooliesXTechChallengeApi.Controllers.ResultModels;
using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Inferfaces.Helpers;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Implementations.Services
{
	public class ShopperHistoryService : IShopperHistoryService
	{
		private readonly ILogger<ShopperHistoryService> _logger;
		private readonly IHttpClientHelper _shopperHistoryResourceHttpClient;

		public ShopperHistoryService(ILogger<ShopperHistoryService> logger
									, IHttpClientHelper httpClientHelper)
		{
			_logger = logger;
			_shopperHistoryResourceHttpClient = httpClientHelper;
		}

		public async Task<IEnumerable<ShopperHistoryModel>> GetHistory()
		{
			var history = await _shopperHistoryResourceHttpClient.CallGet<ShopperHistoryService>();
			var deserialisedCollection = JsonConvert.DeserializeObject<IEnumerable<ShopperHistoryModel>>(history); 
			return deserialisedCollection;
		}
	}
}
