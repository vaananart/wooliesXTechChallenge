using WooliesXTechChallenge.Core.Inferfaces.Services;
using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallenge.Core.Implementations.ProductSorters;

public class DescendingProductSorter : IProductSorter
{
	public string KeyName { get => "Descending"; }

	public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => products
																								.OrderByDescending(x => x.Name);
}
