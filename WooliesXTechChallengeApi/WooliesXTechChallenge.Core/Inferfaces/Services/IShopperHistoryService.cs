using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallenge.Core.Inferfaces.Services;

public interface IShopperHistoryService
{
	Task<IEnumerable<ShopperHistoryModel>> GetHistory();
}
