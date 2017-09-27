using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Net.Http;

namespace CrossPlatformAppXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OCR : ContentPage
    {
        public OCR()
        {
            //InitializeComponent();
            Button takePhotoBtn = new Button { Text = "Take photo" };
            Button getPhotoBtn = new Button { Text = "Choose from gallery" };
            Button getTextBtn = new Button { Text = "Get text" };
            Image img = new Image();
            Picker pckr = new Picker { Title = "Choose language"};
            pckr.Items.Add("ru");pckr.Items.Add("en");pckr.Items.Add("fr");pckr.Items.Add("uk");
            pckr.SelectedIndex = 0;
            Editor editor = new Editor();

            // выбор фото
            getPhotoBtn.Clicked += async (o, e) =>
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                    img.Source = ImageSource.FromFile(photo.Path);

                    var byteArray = MediaFileToByteArray(photo);

                    var ocrResults = Recognize(byteArray, pckr.SelectedItem.ToString());
                    var res = CombineOcrResults(ocrResults);
                    System.Diagnostics.Debug.WriteLine(res);
                    editor.Text = res;
                }
            };

            // съемка фото
            takePhotoBtn.Clicked += async (o, e) =>
            {
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        SaveToAlbum = true,
                        Directory = "Sample",
                        Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                    });

                    if (file == null)
                        return;

                    img.Source = ImageSource.FromFile(file.Path);

                    var byteArray = MediaFileToByteArray(file);

                    var ocrResults = Recognize(byteArray, pckr.SelectedItem.ToString());
					var res = CombineOcrResults(ocrResults);
					System.Diagnostics.Debug.WriteLine(res);
					editor.Text = res;
                }
            };

            getTextBtn.Clicked += async (o, e) =>
            {
                await DisplayAlert("Message", "The text has been copied", "Ok");
            };

            Content = new ScrollView {
                Orientation = ScrollOrientation.Vertical,
				Content = new StackLayout
				{
					HorizontalOptions = LayoutOptions.Center,
					Children = {
					pckr,
					new StackLayout
					{
						 Children = {takePhotoBtn, getPhotoBtn},
						 Orientation =StackOrientation.Horizontal,
						 HorizontalOptions = LayoutOptions.CenterAndExpand
					},
					img,
					editor
				}
				}
            };



        }

		byte[] MediaFileToByteArray(MediaFile photoMediaFile)
		{
			using (var memStream = new MemoryStream())
			{
				photoMediaFile.GetStream().CopyTo(memStream);
				return memStream.ToArray();
			}
		}


        private OcrResults Recognize(byte[] byteArray, string lang)
        {
            Stream stream = new MemoryStream(byteArray);

			VisionServiceClient VisionServiceClient = new VisionServiceClient("ada7bf602f064298a7f233f2d689696b", "https://northeurope.api.cognitive.microsoft.com/vision/v1.0");

            OcrResults ocrResult = VisionServiceClient.RecognizeTextAsync(stream, lang).Result;




            return ocrResult;    
        }

		protected string CombineOcrResults(OcrResults results)
		{
            System.Diagnostics.Debug.WriteLine(results.Regions.Count().ToString());
            
			StringBuilder stringBuilder = new StringBuilder();

			if (results != null && results.Regions != null)
			{
				stringBuilder.AppendLine();
				foreach (var item in results.Regions)
				{
					foreach (var line in item.Lines)
					{
						foreach (var word in line.Words)
						{
							stringBuilder.Append(word.Text);
							stringBuilder.Append(" ");
						}

						stringBuilder.AppendLine();
					}

					stringBuilder.AppendLine();
				}
			}

            System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());
			return stringBuilder.ToString();
		}
    }
}