namespace WooliesXTechChallenge.DataModels.DataModels;

public class ShopperHistoryModel
{
	public long CustomerId { get; set; }
	public IEnumerable<ProductModel> Products { get; set; }
}
