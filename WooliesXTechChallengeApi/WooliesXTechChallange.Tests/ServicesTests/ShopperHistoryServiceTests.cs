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
using Newtonsoft.Json.Linq;

using NUnit.Framework;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Implementations.Services;
using WooliesXTechChallengeApi.Inferfaces.Helpers;

namespace WooliesXTechChallange.Tests.ServicesTests
{	
	[TestFixture]
	public class ShopperHistoryServiceTests
	{
		private const string sampleShopperHistoryPayloadFileName = "SampleShopperHistory.json";
		private const string responseValue = "14";
		//
		private Mock<IHttpGETClientHelper> _mockHttpClient;
		private Mock<ILogger<ShopperHistoryService>> _mockLogger;
		private IEnumerable<ShopperHistoryModel> deserialisedList;
		private string remoteContent;

		[OneTimeSetUp]
		public void Setup()
		{
			_mockHttpClient = new Mock<IHttpGETClientHelper>();
			
			_mockLogger = new Mock<ILogger<ShopperHistoryService>>();

			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames()
											.Single(str => str.EndsWith(sampleShopperHistoryPayloadFileName));
			remoteContent = String.Empty;
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				remoteContent = reader.ReadToEnd();
			}
			deserialisedList = JsonConvert.DeserializeObject<IEnumerable<ShopperHistoryModel>>(remoteContent);
			_mockHttpClient.Setup(x => x.CallGet<ShopperHistoryService>())
							.ReturnsAsync(remoteContent);
		}

		[Test]
		public async Task GetHistory_WhenInvoked_ShouldReturnShopperHistory()
		{
			//Arrange
			var service = new ShopperHistoryService(_mockLogger.Object, _mockHttpClient.Object);

			//Action
			var result = await service.GetHistory();

			//Assert
			Assert.AreEqual(result.First().CustomerId, deserialisedList.First().CustomerId);
		}
	}
}
