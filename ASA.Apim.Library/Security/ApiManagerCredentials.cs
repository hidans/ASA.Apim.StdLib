using System.ComponentModel.DataAnnotations;

namespace ASA.Apim.Library.Security
{
    public class ApiManagerCredentials
    {
        [Required]
        public string AccountKey { get; set; }

        [Required]
        public string SubscriptionKey { get; set; }
    }
}