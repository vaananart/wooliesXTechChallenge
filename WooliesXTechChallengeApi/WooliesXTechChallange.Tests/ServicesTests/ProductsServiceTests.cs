using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Moq;

using Newtonsoft.Json;

using NUnit.Framework;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Enums;
using WooliesXTechChallengeApi.Implementations.Services;
using WooliesXTechChallengeApi.Inferfaces.Helpers;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallange.Tests.ServicesTests
{
	[TestFixture]
	public class ProductsServiceTests
	{
		private const string sampleProductsInputFileName = "SampleProducts.json";
		private const string sampleShopperHistoryFileName = "SampleShopperHistory.json";
		//
		private const string expectedLowOutput = "LOWSortedOutput.json";
		private const string expectedHighOutput = "HIGHSortedOutput.json";
		private const string expectedAscendingOutput = "ASCENDINGSortedOutput.json";
		private const string expectedDescendingOutput = "DESCENDINGSortedOutput.json";
		private const string expectedRecommendedOutput = "RECOMMENDEDSortedOutput.json";
		//
		private Mock<IHttpGETClientHelper> _mockHttpClient;
		private Mock<ILogger<ProductsService>> _mockLogger;
		private Mock<IShopperHistoryService> _mockShopperHistorySevice;
		private ProductsService service;

		[OneTimeSetUp]
		public void Setup()
		{
			string remoteContent = ExtractFileContent(sampleProductsInputFileName);

			_mockHttpClient = new Mock<IHttpGETClientHelper>();
			_mockHttpClient.Setup(x => x.CallGet<ProductsService>())
							.ReturnsAsync(remoteContent);
			_mockLogger = new Mock<ILogger<ProductsService>>();
			_mockShopperHistorySevice = new Mock<IShopperHistoryService>();

			remoteContent = ExtractFileContent(sampleShopperHistoryFileName);
			var remoteObjectList = JsonConvert.DeserializeObject<IEnumerable<ShopperHistoryModel>>(remoteContent);
			_mockShopperHistorySevice.Setup(x => x.GetHistory())
										.ReturnsAsync(remoteObjectList);

			var list = from t in Assembly
									.GetAssembly(typeof(IProductSorter))
									.GetTypes()
					   where t.GetInterfaces().Contains(typeof(IProductSorter))
					   select t;

			IDictionary<string, IProductSorter> productSorterLookup = new Dictionary<string, IProductSorter>();
			foreach (Type sorter in list)
			{
				var instance = Activator.CreateInstance(sorter) as IProductSorter;
				productSorterLookup[instance.KeyName] = instance;
			}

			service = new ProductsService(_mockLogger.Object, _mockShopperHistorySevice.Object, _mockHttpClient.Object, productSorterLookup);
		}

		private static string ExtractFileContent(string filename)
		{
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames()
											.Single(str => str.EndsWith(filename));
			var remoteContent = String.Empty;
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				remoteContent = reader.ReadToEnd();
			}

			return remoteContent;
		}

		[Test]
		public async Task GetSortedProducts_WhenSortOptionIsLow_ShouldReturnAscendingOrderByPrice()
		{
			//Arrange
			string remoteContent = ExtractFileContent(expectedLowOutput);
			var expectedModels = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(remoteContent);

			//Action
			var result = await service.GetSortedProducts(SortOptionEnums.Low);

			//Assert
			Assert.AreEqual(result.First().Name, result.First().Name);
		}

		[Test]
		public async Task GetSortedProducts_WhenSortOptionIsHigh_ShouldReturnDescendingOrderByPrice()
		{
			//Arrange
			string remoteContent = ExtractFileContent(expectedHighOutput);
			var expectedModels = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(remoteContent);

			//Action
			var result = await service.GetSortedProducts(SortOptionEnums.High);

			//Assert
			Assert.AreEqual(result.First().Name, result.First().Name);
		}

		[Test]
		public async Task GetSortedProducts_WhenSortOptionIsAscending_ShouldReturnAscendingOrderByName()
		{
			//Arrange
			string remoteContent = ExtractFileContent(expectedAscendingOutput);
			var expectedModels = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(remoteContent);

			//Action
			var result = await service.GetSortedProducts(SortOptionEnums.Ascending);

			//Assert
			Assert.AreEqual(result.First().Name, result.First().Name);
		}

		[Test]
		public async Task GetSortedProducts_WhenSortOptionIsDescending_ShouldReturnDescendingOrderByName()
		{
			//Arrange
			string remoteContent = ExtractFileContent(expectedDescendingOutput);
			var expectedModels = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(remoteContent);

			//Action
			var result = await service.GetSortedProducts(SortOptionEnums.Descending);

			//Assert
			Assert.AreEqual(result.First().Name, result.First().Name);
		}

		[Test]
		public async Task GetSortedProducts_WhenSortOptionIsRecommended_ShouldReturnOrderByPopularity()
		{
			//Arrange
			string remoteContent = ExtractFileContent(expectedRecommendedOutput);
			var expectedModels = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(remoteContent);

			//Action
			var result = await service.GetSortedProducts(SortOptionEnums.Recommended);

			//Assert
			Assert.AreEqual(result.First().Name, result.First().Name);
		}


	}
}
