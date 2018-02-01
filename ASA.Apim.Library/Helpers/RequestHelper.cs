using ASA.Apim.Library.Security;
using System.Net;

namespace ASA.Apim.Library.Helpers
{
    internal static class RequestHelper
    {
        internal static WebRequest AddHeaders(this WebRequest request, ApiManagerCredentials credentials)
        {
            if(credentials != null)
            {
                if (!string.IsNullOrEmpty(credentials.AccountKey))
                {
                    AddHeader(request, "Ocp-Apim-account-Key", credentials.AccountKey);
                }

                if (!string.IsNullOrEmpty(credentials.SubscriptionKey))
                {
                    AddHeader(request, "Ocp-Apim-Subscription-Ke", credentials.SubscriptionKey);
                }
            }         
            return request;
        }

        private static void AddHeader(WebRequest request, string name, string value)
        {
            request.Headers.Add(name, value);
        }
    }
}
