using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WooliesXTechChallengeApi.Controllers.ResultModels;

namespace WooliesXTechChallengeApi.Inferfaces.Services
{
	public interface IUserService
	{
		UserDetails GetUser();
	}
}
