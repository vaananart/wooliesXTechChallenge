using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallenge.Core.Inferfaces.Services;

public interface IProductSorter
{
	public string KeyName { get; }

	public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products);
}
