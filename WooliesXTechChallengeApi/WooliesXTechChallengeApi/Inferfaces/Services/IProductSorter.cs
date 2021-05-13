using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WooliesXTechChallengeApi.DataModels;

namespace WooliesXTechChallengeApi.Inferfaces.Services
{
	public interface IProductSorter
	{
		public string KeyName { get; }

		public IEnumerable<ProductModel> GetSortedProducts(IEnumerable<ProductModel> products);
	}
}
