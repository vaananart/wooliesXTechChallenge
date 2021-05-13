
using System.Collections.Generic;
using System.Linq;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Implementations.ProductSorters
{
	public class HighProductSorter : IProductSorter
	{
		public string KeyName { get=>"High"; }

		public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => products
																									.OrderByDescending(x => x.Price);
	}
}
