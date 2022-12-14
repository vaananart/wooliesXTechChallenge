using WooliesXTechChallenge.DataModels.DataModels;
using WooliesXTechChallenge.DataModels.Enums;

namespace WooliesXTechChallenge.Core.Inferfaces.Services;

public interface IProductsService
{
	public Task<IEnumerable<ProductModel>> GetSortedProducts(SortOptionEnums option);
}
