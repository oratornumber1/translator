using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace CrossPlatformAppXamarin.Translation
{
    public class LanguageDetector
    {
        public string AuthToken { get; set; }
        public LanguageDetector(string authToken)
        {
            this.AuthToken = authToken;
        }

        public string Detect(string text)
        {
            var lang = "";
            var uri = new Uri("https://api.microsofttranslator.com/v2/Http.svc/Detect?text=" + text);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", AuthToken);
                var stream = client.GetStreamAsync(uri).Result;

                DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                lang = (string)dcs.ReadObject(stream);
            }

            return lang;
        }
    }
}
