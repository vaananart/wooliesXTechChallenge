using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WooliesXTechChallenge.Core.Inferfaces.Services;
using WooliesXTechChallenge.DataModels.DataModels;
using WooliesXTechChallenge.DataModels.ResultModels;

namespace WooliesXTechChallengeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{


	private readonly ILogger _logger;
	private readonly IMapper _mapper;
	private readonly IUserService _userService;

	public UserController(ILogger<UserController> logger
							, IMapper mapper
							, IUserService userService)
	{
		_logger = logger;
		_mapper = mapper;
		_userService = userService;
	}

	[HttpGet]
	public async Task<ActionResult> Get()
	{
		_logger.LogInformation("UserController:GET: Logged before executing UserService.");

		var result = await _userService.GetUser();

		var convertedResult = _mapper.Map<UserDetailsModel, UserDetailsResultModel>(result);
		return Ok(convertedResult);
	}
}
