using AutoMapper;

using WooliesXTechChallenge.DataModels.DataModels;
using WooliesXTechChallenge.DataModels.ResultModels;

namespace WooliesXTechChallenge.Util.Utils.Mapping;

public class UserDetailsMappingProfile : Profile
{
	public UserDetailsMappingProfile()
	{
		CreateMap<UserDetailsModel, UserDetailsResultModel>();
	}
}
