using MealApp.Views;
using MealApp.VUser.uViews;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MealApp.VUser
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(uProductDetailsPage), typeof(uProductDetailsPage));
            Routing.RegisterRoute(nameof(uCartPage), typeof(uCartPage));
            Routing.RegisterRoute(nameof(uCheckoutPage), typeof(uCheckoutPage));
            OnLoad();
        }

        void OnLoad()
        {
            UserName_FlyoutLabel.Text = RuntimeCache.UserName;
        }

        private void OnMenuItemClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}
