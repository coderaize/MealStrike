using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace MealApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {


        public RegisterPage()
        {
            InitializeComponent();
            loginPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new LoginPage();
                })
            });
            addressTxt.TrailingIconCommand = new Command(async () =>
            {
                double lat = 0, lon = 0;
                try
                {
                    Location g = await ScalarFunctions.GetGeoLoctionAsync();
                    lat = g.Latitude;
                    lon = g.Longitude;

                    Hashtable addressDetails = await InfoDataBase.GetDetailsByLocationCodesAsync(RuntimeCache.UserName, g.Latitude.ToString(), g.Longitude.ToString());
                    if (addressDetails.ContainsKey("Address"))
                        addressTxt.Text = addressDetails["Address"].ToString();
                }
                catch (Exception t) { await DisplayAlert("", $"{t}", "OK"); }
                await DisplayAlert("", $"{lat},{lon}", "OK");
            });

        }


        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            //Create actions
            var OPTActions = new string[] {
                "SMS",
                "EMAIL",
                "Skip"
            };

            //Show simple dialog
            var OPTTypeResult = await MaterialDialog.Instance.SelectActionAsync(title: "Contact Confirmation Method", actions: OPTActions);
            if (OPTTypeResult == 0 || OPTTypeResult == 1)
            {
                string Code = "123456";
            // Generate Code
            // Send Confirmation Code
            // 
            TryAgain:
                var ConfirmationCodeResult = await MaterialDialog.Instance.InputAsync(
                    title: "Confirmation Code",
                    message: $"Please Enter 6 Digit Code Received via {OPTActions[OPTTypeResult]}",
                    null, "Enter Code", "OK", "Cancel", new XF.Material.Forms.UI.Dialogs.Configurations.MaterialInputDialogConfiguration
                    {
                        InputMaxLength = 6,
                        InputType = XF.Material.Forms.UI.MaterialTextFieldInputType.Numeric,
                    }
                );
                if (ConfirmationCodeResult == string.Empty)
                    return;
                if (ConfirmationCodeResult != Code)
                    goto TryAgain;
                //return if code not confirmed

            }

            string Query = "EXEC ma_InsertUser @FullName =@FullName ," +
                "@UserName =@UserName   ,@PrimaryEmail =@PrimaryEmail ," +
                "@Telephone =@Telephone ,@Address =@Address ,@Password =@Password ," +
                "@VerificationType=@VerificationType,@isActive =@isActive ";

            Hashtable hD = new Hashtable
            {
                {"FullName",fullNameTxt.Text},
                {"UserName",userNameTxt.Text},
                {"PrimaryEmail",emailTxt.Text},
                {"Telephone",phoneTxt.Text},
                {"Address",addressTxt.Text},
                {"Password",passwordTxt.Text},
                {"VerificationType",OPTActions[OPTTypeResult]},
                {"isActive","1"},
            };
            var result = InfoDataBase.GetDS(Query, hD, this).GetScalarByDataSet();
            if (result.Contains("<error>"))
            {
                await DisplayAlert("Error", result.Replace("<error>", ""), "OK");
            }
            else
            {
                await DisplayAlert("Success", result, "OK");
                Application.Current.MainPage = new LoginPage();
            }

        }


    }
}