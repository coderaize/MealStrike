using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage
    {
        public EntryPage()
        {
            InitializeComponent();

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                InfoDataBase.GetDSAsync("SELECT GETDATE()", new System.Collections.Hashtable());
                // do something every 60 seconds
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new HomePage();
                    return;
                });
                return false; // runs again, or false to stop
            });

        }
    }
}