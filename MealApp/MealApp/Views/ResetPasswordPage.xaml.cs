using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        private string UserNameOrEmail { get; set; }
        private string OTPPin { get; set; }
        public ResetPasswordPage()
        {
            InitializeComponent();

            loginPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new LoginPage();
                })
            });
            getOTPAgainLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(getOPTAgain)
            });
            correctUserNameOrEmailLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(correctEmailOrUserName)
            });
        }

        private async void fetchOPTButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userNameOrEmailTxt.Text))
            {
                await DisplayAlert("Invalid Data", "Please Enter Email or UserName To Continue", "Ok");
                return;
            }
            //////////
            OTPPin = "123456";
            SendOTP();
            otpTxt.Text = "";
            firstStepPanel.IsVisible = false;
            secondStepPanel.IsVisible = true;
            thirdStepPanel.IsVisible = false;
        }

        public void getOPTAgain()
        {
            OTPPin = "123456";
            otpTxt.Text = "";
            SendOTP();
        }

        public void correctEmailOrUserName()
        {
            userNameOrEmailTxt.Text = "";
            otpTxt.Text = "";
            firstStepPanel.IsVisible = true;
            secondStepPanel.IsVisible = false;
            thirdStepPanel.IsVisible = false;
        }

        private async void CheckOTPButton_Clicked(object sender, EventArgs e)
        {
            if (otpTxt.Text != OTPPin)
            {
                await DisplayAlert("Invalid OTP", "Please Enter valid OTP", "OK");

            }
            else
            {
                firstStepPanel.IsVisible = false;
                secondStepPanel.IsVisible = false;
                thirdStepPanel.IsVisible = true;
                NewPasswordTxt.Text = "";
                ConfirmPasswordTxt.Text = "";
            }
        }


        private  bool SendOTP()
        {
            return false;
        }

        private async void SaveNewPasswordBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NewPasswordTxt.Text))
            {
                await DisplayAlert("Invalid Data", "Please Enter New Password", "OK");
                return;
            }
            if (NewPasswordTxt.Text == ConfirmPasswordTxt.Text)
            {
                await DisplayAlert("Invalid Passwords", "New and Confirm password do not match", "OK");
                return;
            }

            string Query = "EXEC app_ChangePassword @UserNameOrEmail = @UserNameOrEmail,@NewPassword = @NewPassword";
            Hashtable hD = new Hashtable {
                {"UserNameOrEmail",userNameOrEmailTxt.Text } ,
                {"NewPassword",ConfirmPasswordTxt.Text }
            };
            var DS = await InfoDataBase.GetDSAsync(Query, hD);
            await DisplayAlert("Alert", DS.Tables[0].Rows[0][0].ToString(), "OK");
            Application.Current.MainPage = new LoginPage();
        }
    }

}