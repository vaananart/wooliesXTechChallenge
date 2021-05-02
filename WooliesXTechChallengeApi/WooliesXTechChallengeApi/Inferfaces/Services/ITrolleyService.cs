using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace WooliesXTechChallengeApi.Inferfaces.Services
{
	public interface ITrolleyService
	{
		Task<string> CalculateLowestTotal(JObject trolleyPayload);
	}
}
