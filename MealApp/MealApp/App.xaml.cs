using MealApp.Views;
using System;
using System.Net;
using System.Threading;
using Xamarin.Forms;
namespace MealApp
{

    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            System.Net.ServicePointManager.DnsRefreshTimeout = 0;
            ServicePointManager.ServerCertificateValidationCallback += (s, certificate, chain, sslPolicyErrors) => true;
            XF.Material.Forms.Material.Init(this);
            //
            MainPage = new EntryPage();

        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {

        }

    }
}
