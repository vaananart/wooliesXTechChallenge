using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WooliesXTechChallengeApi.Controllers.InputModel;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Controllers
{
	[Route("api/trolleyTotal")]
	[ApiController]
	public class TrolleyController : ControllerBase
	{
		private readonly ILogger<TrolleyController> _logger;
		private readonly ITrolleyService _trolleyService;

		public TrolleyController(ILogger<TrolleyController> logger
									, ITrolleyService trolleyService)
		{
			_logger = logger;
			_trolleyService = trolleyService;
		}
		[HttpPost]
		public ActionResult GetTotal([FromBody] IEnumerable<ShoppingLineItem> shoppingList)
		{
			return null;
		}
	}
}
