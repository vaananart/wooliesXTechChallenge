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
		private readonly IHttpClientHelper _productResourceHttpClient;

		public ProductsService(ILogger<ProductsService> logger
								, IShopperHistoryService shopperHistoryService
								, IHttpClientHelper httpClientHelper)
		{
			_logger = logger;
			_shopperHistoryService = shopperHistoryService;
			_productResourceHttpClient = httpClientHelper;
		}

		public async Task<IEnumerable<ProductModel>> GetSortedProducts(SortOptionEnums option)
		{
			_logger.LogInformation($"{typeof(ProductsService).Name}:GetSortedProducts: Received option: {option}");

			string result = string.Empty;

			if (option == SortOptionEnums.Recommended)
			{
				var rawProductList = (await _shopperHistoryService.GetHistory())
									.SelectMany(x => x.Products);

				//NOTE: Definition of "Recommended" => based on popularity
				//This means the number of product lines * quantity to find
				// the product which is reducing in the stock greatly in the history
				//REVIEW:TEST: test this with quantity and number of product repeatitions per customer order
				return GetProductsByPopularitySorted(rawProductList);
			}

			try
			{
				result = await _productResourceHttpClient.CallGet<ProductsService>();
				_logger.LogDebug($"{ typeof(ProductsService).Name}:GetSortedProducts: received payload : {result}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"{ typeof(ProductsService).Name}:GetSortedProducts: received error : {ex}");
			}

			var products = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(result);

			switch (option)
			{
				case SortOptionEnums.Low:
					return GetProductsByPriceAscending(products);
				case SortOptionEnums.High:
					return GetProductsByPriceDescending(products);
				case SortOptionEnums.Ascending:
					return GetProductsByNameAscending(products);
				case SortOptionEnums.Descending:
					return GetProductsByNameDescending(products);
			}

			return new List<ProductModel>();
		}

		private IEnumerable<ProductModel> GetProductsByPopularitySorted(IEnumerable<ProductModel> rawProductList) => (
																													from product in rawProductList
																													group product by new { product.Name }
																													into GroupByName
																													select new
																													{
																														Name = GroupByName.Key.Name,
																														Count = GroupByName.Count() * rawProductList
																																						.Where(x => x.Name == GroupByName.Key.Name)
																																						.Sum(y => y.Quantity),
																														Price = rawProductList
																																.Where(x => x.Name == GroupByName.Key.Name)
																																.FirstOrDefault().Price
																													}
																												)
																												.OrderByDescending(x => x.Count)
																												.Select(y =>
																													new ProductModel
																													{
																														Name = y.Name,
																														Price = y.Price
																													}
																												);
		private IEnumerable<ProductModel> GetProductsByPriceAscending(IEnumerable<ProductModel> rawProductList) => rawProductList
																												.OrderBy(x => x.Price);

		private IEnumerable<ProductModel> GetProductsByPriceDescending(IEnumerable<ProductModel> rawProductList) => rawProductList
																													.OrderByDescending(x => x.Price);
		private IEnumerable<ProductModel> GetProductsByNameAscending(IEnumerable<ProductModel> rawProductList) => rawProductList
																													.OrderBy(x => x.Name);
		private IEnumerable<ProductModel> GetProductsByNameDescending(IEnumerable<ProductModel> rawProductList) => rawProductList
																												.OrderByDescending(x => x.Name);
	}
}
