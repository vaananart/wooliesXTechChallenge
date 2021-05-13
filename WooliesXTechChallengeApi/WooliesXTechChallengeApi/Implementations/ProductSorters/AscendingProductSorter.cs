
using System.Collections.Generic;
using System.Linq;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Implementations.ProductSorters
{
	public class AscendingProductSorter : IProductSorter
	{
		private const string name = "Ascending";

		public string KeyName { get => name;}

		public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => products
																									.OrderBy(x => x.Name);
	}
}
