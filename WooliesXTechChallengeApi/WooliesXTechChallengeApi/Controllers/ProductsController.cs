using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WooliesXTechChallengeApi.Controllers.ResultModels;
using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Enums;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IMapper _mapper;
		private readonly IProductsService _productService;

		public ProductsController(ILogger<ProductsController> logger
									, IMapper mapper
									, IProductsService productService)
		{
			_logger = logger;
			_mapper = mapper;
			_productService = productService;
		}

		[HttpGet]
		public async Task<IEnumerable<ProductResultModel>> Get(SortOptionEnums sortOption)
		{
			_logger.LogInformation($"ProductController:Get: Received sortOption:{sortOption}");
			var result = await _productService.GetSortedProducts(sortOption);
			var convertedResult = _mapper.Map<IEnumerable<ProductModel>, IEnumerable<ProductResultModel>>(result);
			return convertedResult;
		}
	}
}
