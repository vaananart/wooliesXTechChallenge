using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WooliesXTechChallengeApi.Inferfaces.Helpers
{
	/*
	 * NOTE: I want this interface and it's implementation to handle the 
	 * all the http calls and make it generic so that it taps into configuration
	 * information with the TService.
	 */
	public interface IHttpClientHelper
	{
		Task<string> CallGet<TService>();

		HttpClient CallPost();
	}
}
