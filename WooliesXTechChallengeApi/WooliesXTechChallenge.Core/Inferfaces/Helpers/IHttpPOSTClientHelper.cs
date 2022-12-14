using Newtonsoft.Json.Linq;

namespace WooliesXTechChallenge.Core.Inferfaces.Helpers;

public interface IHttpPOSTClientHelper
{
	Task<string> CallPost<TService>(JObject jsonPayload);
}
