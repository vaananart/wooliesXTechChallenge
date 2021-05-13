
using System.Collections.Generic;
using System.Linq;

using WooliesXTechChallengeApi.DataModels;
using WooliesXTechChallengeApi.Inferfaces.Services;

namespace WooliesXTechChallengeApi.Implementations.ProductSorters
{
	public class RecommendedProductSorter : IProductSorter
	{
		public string KeyName { get => "Recommended"; }

		public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products) => (
																									from product in products
																									group product by new { product.Name }
																									into GroupByName
																									select new
																									{
																										Name = GroupByName.Key.Name,
																										Count = GroupByName.Count() * products
																																		.Where(x => x.Name == GroupByName.Key.Name)
																																		.Sum(y => y.Quantity),
																										Price = products
																												.Where(x => x.Name == GroupByName.Key.Name)
																												.FirstOrDefault().Price
																									}
																								)
																								.OrderByDescending(x => x.Count)
																								.Select(y =>
																									new ProductModel
																									{
																										Name = y.Name,
																										Price = y.Price
																									}
																								);
	}
}
