using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WooliesXTechChallengeApi.Controllers.ResultModels;
using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Controllers
{
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
		[ProducesResponseType(typeof(UserDetailsResultModel),200)]
		public async Task<ActionResult> Get()
		{
			_logger.LogInformation("UserController:GET: Logged before executing UserService.");

			var result = await _userService.GetUser();

			var convertedResult = _mapper.Map<UserDetailsModel, UserDetailsResultModel>(result);
			return Ok(convertedResult);
		}
	}
}
