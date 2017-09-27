using CrossPlatformAppXamarin.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrossPlatformAppXamarin
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            Title = "Translation Page";
            InitializeComponent();
            //translatorService =  new TranslatorService.TranslatorServiceClient("d3cd4a73016c451a92e5835654145162");
        }

        private void TranslateBtn_Clicked(object sender, EventArgs e)
        {
            AzureAuthToken azureAuthToken = new AzureAuthToken("731cf5d466e543409989ce06c9499979");
            var authToken = azureAuthToken.GetAccessToken();

            LanguageDetector languageDetector = new LanguageDetector(authToken);
            var lang = languageDetector.Detect(toTranslate.Text);

            Translator translator = new Translator(authToken);
            var result = translator.Translate(toTranslate.Text, lang, languagePicker.SelectedItem.ToString());

            translated.Text = result;

            App.Database.SaveItem(new Models.Translation { FromLang = languagePicker.SelectedItem.ToString(), ToLang = lang , FromText = toTranslate.Text , ToText = result });
        }

        private async void HistoryBtn_Clicked(object sender, EventArgs e)
        {
            var historyPage = new History();

            await Navigation.PushAsync(historyPage);
        }

        private async void OCRBtn_Clicked(object sender, EventArgs e)
        {
            var ocr = new OCR();
            await Navigation.PushAsync(ocr);
        }
    }
}
