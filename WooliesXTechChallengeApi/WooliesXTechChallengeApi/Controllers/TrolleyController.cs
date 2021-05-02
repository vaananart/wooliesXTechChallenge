using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Controllers
{
	[Route("api/cart")]
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
		[HttpPost("trolleyTotal")]
		public async Task<ActionResult> ComputeTotal([FromBody] JObject order)
		{
			_logger.LogInformation($"TrolleyController:ComputeTotal:order => {order.ToString()}");
			string result = string.Empty;
			try
			{
				result = await _trolleyService.CalculateLowestTotal(order);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
			return Ok(result);
		}
	}
}
