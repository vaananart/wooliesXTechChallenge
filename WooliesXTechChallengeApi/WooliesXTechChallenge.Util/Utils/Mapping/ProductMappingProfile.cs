using AutoMapper;

using WooliesXTechChallenge.DataModels.DataModels;
using WooliesXTechChallenge.DataModels.ResultModels;

namespace WooliesXTechChallenge.Util.Utils.Mapping;

public class ProductMappingProfile : Profile
{
	public ProductMappingProfile()
	{
		CreateMap<ProductModel, ProductResultModel>();
	}
}
