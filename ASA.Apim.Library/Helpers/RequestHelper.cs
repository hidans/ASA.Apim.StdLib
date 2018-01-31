using ASA.Apim.Library.Security;
using System.Net;

namespace ASA.Apim.Library.Helpers
{
    internal static class RequestHelper
    {
        internal static WebRequest AddAzureHeaders(this WebRequest request, ApiManagerCredentials credentials)
        {
            if(credentials != null)
            {
                if (!string.IsNullOrEmpty(credentials.AccountKey))
                {
                    request.OcpApimAccountKey(credentials.AccountKey);
                }

                if (!string.IsNullOrEmpty(credentials.SubscriptionKey))
                {
                    request.OcpApimSubscriptionKey(credentials.SubscriptionKey);
                }
            }         
            return request;
        }

        private static void OcpApimAccountKey(this WebRequest request, string key)
        {
            request.Headers.Add("Ocp-Apim-account-Key", key);
        }

        private static void OcpApimSubscriptionKey(this WebRequest request, string key)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
        }
    }
}
