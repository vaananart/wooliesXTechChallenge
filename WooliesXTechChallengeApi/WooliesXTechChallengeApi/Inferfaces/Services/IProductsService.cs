using System.Collections.Generic;
using System.Threading.Tasks;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Enums;

namespace WooliesXTechChallengeApi.Inferfaces.Services
{
	public interface IProductsService
	{
		public Task<IEnumerable<ProductModel>> GetSortedProducts(SortOptionEnums option);
	}
}
