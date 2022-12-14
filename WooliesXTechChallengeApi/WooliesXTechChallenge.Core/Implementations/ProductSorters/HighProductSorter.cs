using WooliesXTechChallenge.Core.Inferfaces.Services;
using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallenge.Core.Implementations.ProductSorters;

public class HighProductSorter : IProductSorter
{
	public string KeyName { get => "High"; }

	public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => products
																								.OrderByDescending(x => x.Price);
}
