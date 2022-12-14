using WooliesXTechChallenge.Core.Inferfaces.Services;
using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallenge.Core.Implementations.ProductSorters;

public class AscendingProductSorter : IProductSorter
{
	private const string name = "Ascending";

	public string KeyName { get => name; }

	public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => products
																								.OrderBy(x => x.Name);
}
