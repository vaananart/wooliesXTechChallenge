using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WooliesXTechChallengeApi.DataModels
{
	public class ShopperHistoryModel
	{
		public long CustomerId { get; set; }
		public IEnumerable<ProductModel> Products { get; set; }
	}
}
