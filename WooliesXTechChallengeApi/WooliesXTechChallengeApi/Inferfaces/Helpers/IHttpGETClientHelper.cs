using System.Threading.Tasks;

namespace WooliesXTechChallengeApi.Inferfaces.Helpers
{
	public interface IHttpGETClientHelper
	{
		Task<string> CallGet<TService>();
	}
}
