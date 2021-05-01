using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{


		private readonly ILogger _logger;
		private readonly IUserService _userService;

		public UserController(ILogger<UserController> logger
								, IUserService userService)
		{
			_logger = logger;
			_userService = userService;
		}

		[HttpGet]
		public IEnumerable<UserDetails> Get()
		{
			return null;
		}
	}
}
