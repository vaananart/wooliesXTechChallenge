
using System.Collections.Generic;
using System.Linq;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Implementations.ProductSorters
{
	public class LowProductSorter : IProductSorter
	{
		public string KeyName { get => "Low"; }

		public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => products
																									.OrderBy(x => x.Price);
	}
}
