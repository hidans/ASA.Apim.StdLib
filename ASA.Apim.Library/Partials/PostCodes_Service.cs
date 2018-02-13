namespace ASA.Apim.Library.PostCodes_Service
{
    using ASA.Apim.Library.Helpers;
    using ASA.Apim.Library.Security;
    using System;

    public partial class PostCodes_Service
    {
        public ApiManagerCredentials ApiCredentials { get; set; }

        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            return base.GetWebRequest(uri).AddHeaders(ApiCredentials);
        }
    }
}