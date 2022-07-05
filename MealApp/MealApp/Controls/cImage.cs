using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MealApp.Controls
{
    class cImage : Image
    {
        public cImage() { }


        private string ImageURL { get; set; }

        private string _LoadUriSource { get; set; }
        public string LoadUriSource
        {
            get => _LoadUriSource;
            set
            {
                _LoadUriSource = value;
                SetUriImages();
            }
        }

        void SetUriImages()
        {

            if (string.IsNullOrEmpty(_LoadUriSource))
            {
                this.Source = "mealapp_loading_image.gif";
                return;
            }
            else
            {
                _LoadUriSource = _LoadUriSource.Replace("(WebBaseUrl)", InfoDataBase.WebBaseUrl);
                ImageURL = _LoadUriSource;
            }

            //var imgUrl = _LoadUriSource.Replace("https://208.78.220.245/", "https://tickers.abmalik.com/");
            //this.Source = new UriImageSource
            //{
            //    Uri = new Uri(imgUrl),
            //    CachingEnabled = true,
            //    CacheValidity = new TimeSpan(5, 0, 0, 0)
            //};

            try
            {
                WebClient wC = new WebClient();
                wC.DownloadDataAsync(new Uri(ImageURL));
                wC.DownloadDataCompleted += (x, y) =>
                {
                    this.Source = StreamImageSource.FromStream(() => new MemoryStream(y.Result));
                    wC.Dispose();
                };

            }
            catch
            {
                this.Source = "mealapp_loading_image.gif";// ScalarFunctions.LoadImageFromBase64(ScalarFunctions.NoImagePath);
            }

        }

    }
}
