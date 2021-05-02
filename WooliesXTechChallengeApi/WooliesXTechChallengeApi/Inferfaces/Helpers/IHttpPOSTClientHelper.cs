using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace WooliesXTechChallengeApi.Inferfaces.Helpers
{
	public interface IHttpPOSTClientHelper
	{
		Task<string> CallPost<TService>(JObject jsonPayload);
	}
}
