using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WooliesXTechChallengeApi.Controllers.ResultModels;
using WooliesXTechChallengeApi.DataModels;

namespace WooliesXTechChallengeApi.Inferfaces.Services
{
	public interface IUserService
	{
		Task<UserDetailsModel> GetUser();
	}
}
