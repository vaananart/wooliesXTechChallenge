using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using WooliesXTechChallengeApi.Controllers.ResultModels;
using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Enums;
using WooliesXTechChallengeApi.Inferfaces.Helpers;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Implementations.Services
{
	public class ProductsService : IProductsService
	{
		private readonly ILogger _logger;
		private readonly IShopperHistoryService _shopperHistoryService;
		private readonly IHttpGETClientHelper _productResourceHttpClient;
		private readonly IDictionary<string, IProductSorter> _productSorters;

		public ProductsService(ILogger<ProductsService> logger
								, IShopperHistoryService shopperHistoryService
								, IHttpGETClientHelper httpClientHelper
								, IDictionary<string,IProductSorter> productSorters)
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
					_logger.LogDebug($"{ typeof(ProductsService).Name}:GetSortedProducts:SortedOption=Recommended:received payload : {orderedProductRankkedResult}");

					return orderedProductRankkedResult;
				}

				var result = _productResourceHttpClient.CallGet<ProductsService>();
				_logger.LogDebug($"{ typeof(ProductsService).Name}:GetSortedProducts: received payload : {result}");

				await Task.WhenAll(result);
				var products = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(result.Result);

				if(_productSorters.ContainsKey(option.ToString()))
					return _productSorters[option.ToString()].GetSortedProducts(products);
			}
			catch (Exception ex)
			{
				_logger.LogError($"{ typeof(ProductsService).Name}:GetSortedProducts: Error : {ex}");
				throw new Exception($"{ typeof(ProductsService).Name}:GetSortedProducts: Error : {ex}");
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

		private  IEnumerable<ProductModel> GetSortedProductsByPopularity((IEnumerable<ProductModel> Products, IEnumerable<ProductModel> HistoricalProducts) productCollection)
		{
			var localResult = _productSorters[SortOptionEnums.Recommended.ToString()].GetSortedProducts(productCollection.HistoricalProducts).ToList();

			var localResultNames = localResult.Select(x => x.Name);
			var setDifference = productCollection.Products.Where(x => !localResultNames.Contains(x.Name));
			localResult.AddRange(setDifference);
			return localResult.AsEnumerable();
		}

		//private IEnumerable<ProductModel> GetShopperHistoryProductsByPopularitySorted(IEnumerable<ProductModel> rawProductList) => (
		//																											from product in rawProductList
		//																											group product by new { product.Name }
		//																											into GroupByName
		//																											select new
		//																											{
		//																												Name = GroupByName.Key.Name,
		//																												Count = GroupByName.Count() * rawProductList
		//																																				.Where(x => x.Name == GroupByName.Key.Name)
		//																																				.Sum(y => y.Quantity),
		//																												Price = rawProductList
		//																														.Where(x => x.Name == GroupByName.Key.Name)
		//																														.FirstOrDefault().Price
		//																											}
		//																										)
		//																										.OrderByDescending(x => x.Count)
		//																										.Select(y =>
		//																											new ProductModel
		//																											{
		//																												Name = y.Name,
		//																												Price = y.Price
		//																											}
		//																										);
		//private IEnumerable<ProductModel> GetProductsByPriceAscending(IEnumerable<ProductModel> rawProductList) => rawProductList
		//																										.OrderBy(x => x.Price);

		//private IEnumerable<ProductModel> GetProductsByPriceDescending(IEnumerable<ProductModel> rawProductList) => rawProductList
		//																											.OrderByDescending(x => x.Price);
		//private IEnumerable<ProductModel> GetProductsByNameAscending(IEnumerable<ProductModel> rawProductList) => rawProductList
		//																											.OrderBy(x => x.Name);
		//private IEnumerable<ProductModel> GetProductsByNameDescending(IEnumerable<ProductModel> rawProductList) => rawProductList
		//																										.OrderByDescending(x => x.Name);
	}
}
