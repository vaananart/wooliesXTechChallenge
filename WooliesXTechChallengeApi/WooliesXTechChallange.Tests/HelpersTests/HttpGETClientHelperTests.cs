using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moq;
using Moq.Protected;

using Newtonsoft.Json;

using NUnit.Framework;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Implementations.Helpers;
using WooliesXTechChallengeApi.Implementations.Services;
using WooliesXTechChallengeApi.Inferfaces.Helpers;

namespace WooliesXTechChallange.Tests.HelpersTests
{
	[TestFixture]
	public class HttpGETClientHelperTests
	{
		private Mock<IHttpClientFactory> _mockHttpClientFactory;
		private Mock<HttpClient> _mockHttpClient;
		private Mock<HttpResponseMessage> _mockHttpResponseMessage;
		private IConfiguration JsonConfigurationObject;

		//
		private const string serviceKeyword = "Service";
		private const string httpResourceBaseAddressConfigName = "wooliesXapis:BaseUrl";
		private const string httpTokenConfigName = "wooliesXApis:Token";
		private const string requiredParameterString = "?token=";
		private const string jsonConfigFileName = "configurations.json";
		private const string sampleRemoteProductsFileName = "SampleProducts.json";
		private const string sampleRemoteShopperHistoryFileName = "SampleShopperHistory.json";

		[OneTimeSetUp]
		public void Setup()
		{
			_mockHttpClientFactory = new Mock<IHttpClientFactory>();
			_mockHttpClient = new Mock<HttpClient>();
			_mockHttpResponseMessage = new Mock<HttpResponseMessage>();
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(jsonConfigFileName));
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				JsonConfigurationObject = new ConfigurationBuilder()
												.AddJsonStream(stream)
												.Build();

			}


		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		[Test]
		public async Task CallGet_WhenCalledFromProductService_ShouldReturnProducts()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
			//Arrange
			var preparedEntities = SetupMocks(sampleRemoteProductsFileName);


			//Action
			var result = preparedEntities.entityUnderTest.CallGet<ProductsService>();
			Task.WaitAll(result);

			//Assert
			Assert.AreEqual(result.Result, preparedEntities.expected);
		}

		[Test]
		public async Task CallGet_WhenCalledFromShopperHistoryService_ShouldReturnShoppersHistories()
		{
			//Arrange
			var preparedEntities = SetupMocks(sampleRemoteShopperHistoryFileName);

			//Action
			var result = preparedEntities.entityUnderTest.CallGet<ShopperHistoryService>();
			Task.WaitAll(result);

			//Assert
			Assert.AreEqual(result.Result, preparedEntities.expected);

		}

		private (string expected, IHttpGETClientHelper entityUnderTest) SetupMocks(string sampleFileName)
		{
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames()
											.Single(str => str.EndsWith(sampleFileName));
			var remoteContent = String.Empty;
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				remoteContent = reader.ReadToEnd();
			}

			var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			handlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync"
													, ItExpr.IsAny<HttpRequestMessage>()
													, ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(remoteContent)
				});
			_mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
									.Returns(new HttpClient(handlerMock.Object));
			var mockLogger = new Mock<ILogger<HttpGETClientHelper>>();
			var helper = new HttpGETClientHelper(mockLogger.Object
													, _mockHttpClientFactory.Object
													, JsonConfigurationObject);
			return (remoteContent, helper);
		}




	}
}
