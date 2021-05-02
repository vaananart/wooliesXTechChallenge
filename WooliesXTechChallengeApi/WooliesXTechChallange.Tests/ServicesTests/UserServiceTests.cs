using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moq;

using Newtonsoft.Json;

using NUnit.Framework;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Implementations.Services;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallange.Tests.ServicesTests
{	[TestFixture]
	public class UserServiceTests
	{
		private const string jsonConfigFileName = "configurations.json";
		private const string expectedOutputFile = "ExpectedUserDetails.json";
		private IConfiguration JsonConfigurationObject;
		private UserDetailsModel userDetailExpectedModel;

		[OneTimeSetUp]
		public void Setup()
		{
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(jsonConfigFileName));
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				JsonConfigurationObject = new ConfigurationBuilder()
												.AddJsonStream(stream)
												.Build();

			}

			var content = String.Empty;
			resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(expectedOutputFile));
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				content = reader.ReadToEnd();  

			}

			userDetailExpectedModel = JsonConvert.DeserializeObject<UserDetailsModel>(content);
		}

		[Test]
		public async Task GetUser_WhenRequested_ShouldReturnUserInfo_From_ConfigProperties()
		{
			//Arrange
			var mockLogger = new Mock<ILogger<UserService>>();
			IUserService service = new UserService(mockLogger.Object, JsonConfigurationObject);

			//Action
			var result = await service.GetUser();

			//Assert
			Assert.AreEqual(result.Name, userDetailExpectedModel.Name);
			Assert.AreEqual(result.Token, userDetailExpectedModel.Token);
		}
	}
}
