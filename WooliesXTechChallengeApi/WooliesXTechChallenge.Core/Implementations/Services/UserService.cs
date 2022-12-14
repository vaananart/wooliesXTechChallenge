using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using WooliesXTechChallenge.Core.Inferfaces.Services;
using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallenge.Core.Implementations.Services;

public class UserService : IUserService
{
	private const string baseConfig = "wooliesXApis:Resources:User:";
	private const string nameConfig = baseConfig + "Name";
	private const string tokenConfig = baseConfig + "ReturnToken";
	//
	private readonly ILogger _logger;
	private readonly IConfiguration _configuration;

	public UserService(ILogger<UserService> logger
						, IConfiguration configuration)
	{
		_logger = logger;
		_configuration = configuration;
	}
	public async Task<UserDetailsModel> GetUser()
	{
		_logger.LogInformation("UserService:GetUser: Returning user name and token.");
		return await Task.FromResult(new UserDetailsModel
		{
			Name = _configuration[nameConfig],
			Token = _configuration[tokenConfig]
		});
	}
}
