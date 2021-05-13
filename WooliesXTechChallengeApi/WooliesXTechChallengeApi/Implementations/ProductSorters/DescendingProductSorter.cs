
using System.Collections.Generic;
using System.Linq;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Implementations.ProductSorters
{
	public class DescendingProductSorter : IProductSorter
	{
		public string KeyName { get => "Descending"; }

		public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => products
																									.OrderByDescending(x => x.Name);
	}
}
