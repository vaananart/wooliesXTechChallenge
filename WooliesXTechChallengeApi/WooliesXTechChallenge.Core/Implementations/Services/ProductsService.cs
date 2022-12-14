using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using WooliesXTechChallenge.Core.Inferfaces.Helpers;
using WooliesXTechChallenge.Core.Inferfaces.Services;
using WooliesXTechChallenge.DataModels.DataModels;
using WooliesXTechChallenge.DataModels.Enums;

namespace WooliesXTechChallenge.Core.Implementations.Services;

public class ProductsService : IProductsService
{
	private readonly ILogger _logger;
	private readonly IShopperHistoryService _shopperHistoryService;
	private readonly IHttpGETClientHelper _productResourceHttpClient;
	private readonly IDictionary<string, IProductSorter> _productSorters;

	public ProductsService(ILogger<ProductsService> logger
							, IShopperHistoryService shopperHistoryService
							, IHttpGETClientHelper httpClientHelper
							, IDictionary<string, IProductSorter> productSorters)
	{
		_logger = logger;
		_shopperHistoryService = shopperHistoryService;
		_productResourceHttpClient = httpClientHelper;
		_productSorters = productSorters;
	}

	public async Task<IEnumerable<ProductModel>> GetSortedProducts(SortOptionEnums option)
	{
		_logger.LogInformation($"{typeof(ProductsService).Name}:GetSortedProducts: Received option: {option}");

		try
		{

			/**NOTE:
			 * This needs to be seperated contained. It's not a one line execution.
			 * It needs to perform serveral steps to get to end result like the 
			 * rest of the switch-case results. 
			 */
			if (option == SortOptionEnums.Recommended)
			{
				var rawPopularityData = await GetProductsForPopularityAnalysis();
				var orderedProductRankkedResult = GetSortedProductsByPopularity(rawPopularityData);
				_logger.LogDebug($"{typeof(ProductsService).Name}:GetSortedProducts:SortedOption=Recommended:received payload : {orderedProductRankkedResult}");

				return orderedProductRankkedResult;
			}

			var result = await _productResourceHttpClient.CallGet<ProductsService>();
			_logger.LogDebug($"{typeof(ProductsService).Name}:GetSortedProducts: received payload : {result}");

			//await Task.WhenAll(result);
			var products = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(result);

			if (_productSorters.ContainsKey(option.ToString()))
				return _productSorters[option.ToString()].GetSortedProducts(products);
		}
		catch (Exception ex)
		{
			_logger.LogError($"{typeof(ProductsService).Name}:GetSortedProducts: Error : {ex}");
			throw new Exception($"{typeof(ProductsService).Name}:GetSortedProducts: Error : {ex}");
		}

		return new List<ProductModel>();
	}

	private async Task<(IEnumerable<ProductModel> Products, IEnumerable<ProductModel> HistoricalProducts)> GetProductsForPopularityAnalysis()
	{

		IEnumerable<ProductModel> products = null;
		IEnumerable<ProductModel> historicalProducts = null;

		try
		{
			var shopperHistoryRecords = _shopperHistoryService.GetHistory();
			var result = _productResourceHttpClient.CallGet<ProductsService>();

			await Task.WhenAll(shopperHistoryRecords, result);

			products = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(result.Result);
			historicalProducts = shopperHistoryRecords.Result.SelectMany(x => x.Products);

		}
		catch (Exception ex)
		{
			_logger.LogError($"ProductsService:GetSortedProductsByPopularity: Error: {ex.Message}");
			throw new Exception($"ProductsService:GetSortedProductsByPopularity: Error: {ex.Message}");
		}

		return (products, historicalProducts);
	}

	private IEnumerable<ProductModel> GetSortedProductsByPopularity((IEnumerable<ProductModel> Products, IEnumerable<ProductModel> HistoricalProducts) productCollection)
	{
		var localResult = _productSorters[SortOptionEnums.Recommended.ToString()].GetSortedProducts(productCollection.HistoricalProducts).ToList();

		var localResultNames = localResult.Select(x => x.Name);
		var setDifference = productCollection.Products.Where(x => !localResultNames.Contains(x.Name));
		localResult.AddRange(setDifference);
		return localResult.AsEnumerable();
	}
}
