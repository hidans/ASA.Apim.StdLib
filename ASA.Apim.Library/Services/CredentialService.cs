using ASA.Apim.Library.Security;

namespace ASA.Apim.Library.Services
{
    internal static class CredentialService
    {
        internal static ApiManagerCredentials CreateApiCredentials(string accountKey, string subscriptionKey)
        {
            return new ApiManagerCredentials()
            {
                AccountKey = accountKey,
                SubscriptionKey = subscriptionKey
            };
        }
    }
}