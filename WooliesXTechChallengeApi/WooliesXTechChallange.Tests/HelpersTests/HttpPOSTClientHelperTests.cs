using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moq;
using Moq.Protected;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using WooliesXTechChallengeApi.Implementations.Helpers;
using WooliesXTechChallengeApi.Implementations.Services;
using WooliesXTechChallengeApi.Inferfaces.Helpers;

namespace WooliesXTechChallange.Tests.HelpersTests
{	[TestFixture]
	public class HttpPOSTClientHelperTests
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
		private const string sampleTrolleyCalculationInputFileName = "TrolleyCalculationInput.json";
		private const string responseValue = "14";

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
		
		[Test]
		public async Task CallPost_WhenCalledFromTrolleyService_ShouldSendTrolleyContent()
		{
			//Arrange
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames()
											.Single(str => str.EndsWith(sampleTrolleyCalculationInputFileName));
			var remoteContent = String.Empty;
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				remoteContent = reader.ReadToEnd();
			}
			var jsonPayload = JObject.Parse(remoteContent);
			var preparedEntity = SetupMocks();


			//Action
			var result = preparedEntity.CallPost<TrolleyService>(jsonPayload);
			Task.WaitAll(result);

			//Assert
			Assert.AreEqual(result.Result, responseValue);	
		}

		private IHttpPOSTClientHelper SetupMocks()
		{


			var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			handlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync"
													, ItExpr.IsAny<HttpRequestMessage>()
													, ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(responseValue)
				}).Verifiable();
			_mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
									.Returns(new HttpClient(handlerMock.Object));
			var mockLogger = new Mock<ILogger<HttpPOSTClientHelper>>();
			var helper = new HttpPOSTClientHelper(mockLogger.Object
													, _mockHttpClientFactory.Object
													, JsonConfigurationObject);
			return helper;
		}

	}
}
