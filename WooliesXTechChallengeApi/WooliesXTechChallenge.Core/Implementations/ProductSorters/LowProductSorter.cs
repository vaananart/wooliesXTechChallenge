using WooliesXTechChallenge.Core.Inferfaces.Services;
using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallengeApi.Implementations.ProductSorters
{
	public class LowProductSorter : IProductSorter
	{
		public string KeyName { get => "Low"; }

		public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => products
																									.OrderBy(x => x.Price);
	}
}
