using WooliesXTechChallenge.DataModels.DataModels;

namespace WooliesXTechChallenge.Core.Inferfaces.Services;

public interface IUserService
{
	Task<UserDetailsModel> GetUser();
}
