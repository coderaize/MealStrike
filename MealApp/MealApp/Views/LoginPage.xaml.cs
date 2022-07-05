using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            resetPasswordPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new ResetPasswordPage();
                })
            });

            signupPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new RegisterPage();
                })
            });
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            string userName = userNameTxt.Text;
            string password = passwordTxt.Text;
            //
            string Query = " EXEC [ma_AuthenticateUser] @UserName = @UserName , @Password = @Password ";
            var authResult = InfoDataBase.GetDS(Query, new System.Collections.Hashtable {
                {"UserName", userName},
                {"Password", password}
            });

            var result = authResult.GetScalarByDataSet();
            if (result == "Authenticated")
            {
                RuntimeCache.UserName = authResult.Tables[0].Rows[0]["UserName"].ToString();
                Application.Current.MainPage = new MealApp.VUser.AppShell();
            }
            else
            {
                DisplayAlert("Auth Error", result.Replace("<error>", ""), "OK");
            }


        }

        private void RegisterButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MealApp.Views.RegisterPage();
        }
    }
}