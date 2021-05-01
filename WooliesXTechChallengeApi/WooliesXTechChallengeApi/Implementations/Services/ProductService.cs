using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WooliesXTechChallengeApi.Inferfaces.Helpers;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Implementations.Services
{
	public class ProductService : IProductService
	{
		private readonly IHttpClientHelper<ProductService> _httpClient;

		public ProductService(IHttpClientHelper<ProductService> httpClientHelper)
		{
			_httpClient = httpClientHelper;
		}

		
	}
}
