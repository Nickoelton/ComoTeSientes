using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ComoTeSientes.Class;  // RECUERDA que esta es la referencia de la clase de tu proyecto

namespace ComoTeSientes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmotionsPage : ContentPage
    {

        static Stream streamCopy;
        public EmotionsPage()
        {
            InitializeComponent();
        }
        async void btnPicture_Clicked(object sender, EventArgs e)
        {
            var useCam = ((Button)sender).Text.Contains("Cámara");
            var file = await ServiceImage.TakePicture(useCam);
            Results.Children.Clear();
            lblResult.Text = "La imagen está lista, ahora analízala";

            imgPicture.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                streamCopy = new MemoryStream();
                stream.CopyTo(streamCopy);
                stream.Seek(0, SeekOrigin.Begin);
                file.Dispose();
                return stream;
            });
        }

        async void btnAnalysisEmotions_Clicked(object sender, EventArgs e)
        {
            if (streamCopy != null)
            {
                streamCopy.Seek(0, SeekOrigin.Begin);
                var emotions = await ServiceEmotions.GetEmotions(streamCopy);

                if (emotions != null)
                {
                    lblResult.Text = "Este es el resultado de como te sientes:";
                    DrawResults(emotions);
                }
                else lblResult.Text = "Ups! no detectamos ningun rostro :(";
            }
            else lblResult.Text = "No has seleccionado ninguna imagen";
        }

        void DrawResults(Dictionary<string, float> emotions)
        {
            Results.Children.Clear();
            foreach (var emotion in emotions)
            {
                Label lblEmotion = new Label()
                { 
                    Text = emotion.Key,
                    TextColor = Color.White,
                    WidthRequest = 90

                };
                
                BoxView box = new BoxView()
                {
                    Color = Color.GreenYellow,
                    WidthRequest = 150 * emotion.Value,
                    HeightRequest = 30,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                Label lblPercent = new Label()
                {
                    Text = emotion.Value.ToString("P0"),
                    TextColor = Color.White
                };

                StackLayout stack = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal
                };
                stack.Children.Add(lblEmotion);
                stack.Children.Add(box);
                stack.Children.Add(lblPercent);

                Results.Children.Add(stack);

                switch (lblEmotion.Text)
                {
                    case "Happiness":
                        lblEmotion.Text = "Feliz".ToString();
                        break;
                    case "Neutral":
                        lblEmotion.Text = "Serio".ToString();
                        break;
                    case "Contempt":
                        lblEmotion.Text = "Despreciativo".ToString();
                        break;
                    case "Surprise":
                        lblEmotion.Text = "Sorprendido".ToString();
                        break;
                    case "Disgust":
                        lblEmotion.Text = "Disgustado".ToString();
                        break;
                    case "Anger":
                        lblEmotion.Text = "Enojado".ToString();
                        break;
                    case "Sadness":
                        lblEmotion.Text = "Triste".ToString();
                        break;
                    case "Fear":
                        lblEmotion.Text = "Con miedo".ToString();
                        break;

                }
            }
        }
    }
}