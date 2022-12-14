using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Moq;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using WooliesXTechChallenge.Core.Implementations.Services;
using WooliesXTechChallenge.Core.Inferfaces.Helpers;
using WooliesXTechChallenge.Core.Inferfaces.Services;

namespace WooliesXTechChallange.Tests.ServicesTests
{
	[TestFixture]
	public class TrolleyServiceTests
	{
		private const string sampleTrolleyCalculationInputFileName = "TrolleyCalculationInput.json";
		private const string responseValue = "14";
		//
		private Mock<IHttpPOSTClientHelper> _mockHttpClient;
		private Mock<ILogger<TrolleyService>> _mockLogger;
		private JObject jsonPayload;

		[OneTimeSetUp]
		public void Setup()
		{
			_mockHttpClient = new Mock<IHttpPOSTClientHelper>();
			_mockHttpClient.Setup(x => x.CallPost<TrolleyService>(It.IsAny<JObject>()))
							.ReturnsAsync(responseValue);
			_mockLogger = new Mock<ILogger<TrolleyService>>();

			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames()
											.Single(str => str.EndsWith(sampleTrolleyCalculationInputFileName));
			var remoteContent = String.Empty;
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				remoteContent = reader.ReadToEnd();
			}
			jsonPayload = JObject.Parse(remoteContent);
		}

		[Test]
		public async Task CalculateLowestTotal_WhenTrolleyPayloadSent_ShouldReceivedTheLowestTotalResult()
		{
			//Arrange
			ITrolleyService service = new TrolleyService(_mockLogger.Object, _mockHttpClient.Object);

			//Action
			var result = await service.CalculateLowestTotal(jsonPayload);

			//Assert
			Assert.AreEqual(result, responseValue);
		}
	}
}
