using Newtonsoft.Json.Linq;

namespace WooliesXTechChallenge.Core.Inferfaces.Services;

public interface ITrolleyService
{
	Task<string> CalculateLowestTotal(JObject trolleyPayload);
}
