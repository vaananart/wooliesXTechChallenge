using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IProductService _productService;

		public ProductsController(ILogger<ProductsController> logger
									, IProductService productService)
		{
			_logger = logger;
			_productService = productService;
		}

		[HttpGet]
		public ActionResult Get()
		{

			return null;
		}
	}
}
