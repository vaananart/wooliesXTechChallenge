using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using WooliesXTechChallengeApi.Controllers.ResultModels;
using WooliesXTechChallengeApi.DataModels;

namespace WooliesXTechChallengeApi.Uitls.Mapping
{
	public class ProductMappingProfile : Profile
	{
		public ProductMappingProfile() {
			CreateMap<ProductModel, ProductResultModel>();
		}
	}
}
