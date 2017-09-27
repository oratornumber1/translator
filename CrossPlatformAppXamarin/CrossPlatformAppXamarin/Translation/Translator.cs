using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;


namespace CrossPlatformAppXamarin.Translation
{
    public class Translator
    {
        public string AuthToken { get; set; }
        public Translator(string authToken)
        {
            this.AuthToken = authToken;
        }

        public string Translate(string text, string from, string to)
        {
            string translatedText = "";
            var uri = new Uri("https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Net.WebUtility.UrlEncode(text) + "&from=" + from + "&to=" + to);
            string requestBody = GenerateTranslateOptionsRequestBody("general", "text/plain", "", "", "", "TestUserId");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", AuthToken);
                var stream = client.GetStreamAsync(uri).Result;

                DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                translatedText = (string)dcs.ReadObject(stream);
            }

            return translatedText;
        }

        private string GenerateTranslateOptionsRequestBody(string category, string contentType, string reservedFlags, string state, string uri, string user)
        {
            string body =
                "<TranslateOptions xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">" +
                "  <Category>{0}</Category>" +
                "  <ContentType>{1}</ContentType>" +
                "  <ReservedFlags>{2}</ReservedFlags>" +
                "  <State>{3}</State>" +
                "  <Uri>{4}</Uri>" +
                "  <User>{5}</User>" +
                "</TranslateOptions>";
            return string.Format(body, category, contentType, reservedFlags, state, uri, user);
        }
    }
}
