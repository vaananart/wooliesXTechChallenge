namespace WooliesXTechChallenge.Core.Inferfaces.Helpers;

public interface IHttpGETClientHelper
{
	Task<string> CallGet<TService>();
}
