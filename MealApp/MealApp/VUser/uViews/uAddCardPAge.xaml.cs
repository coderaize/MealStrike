using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.VUser.uViews
{
    public partial class uAddCardPage : ContentPage
    {
        public uAddCardPage()
        {
            InitializeComponent();
        }

        private async void SaveCardButton_Clicked(object sender, EventArgs e)
        {
            string UserName = RuntimeCache.UserName;
            string CardNumber = cardNumberTxt.Text;
            string CardTitle = cardTitleTxt.Text;
            string ExpiryYear = expiryYearTxt.Text;
            string CVVPin = cvv2Txt.Text;
            string Description = descriptionTxt.Text;
            ///
            //Save to DB
            string query = "EXEC [dbo].[m_AddPaymentCard] @UserName = @UserName, " +
                "@CardTitle =@CardTitle, @CardNumber =@CardNumber, @CardExpiryYear=@CardExpiryYear," +
                " @CardCVVPin =@CardCVVPin , @Description=@Description";

            var dS = await InfoDataBase.GetDSAsync(query, new System.Collections.Hashtable
            {
                { "UserName",UserName },
                { "CardTitle",CardTitle },
                { "CardNumber",CardNumber },
                { "CardExpiryYear",ExpiryYear },
                { "CardCVVPin",CVVPin },
                { "Description",Description},

            });

            var res = dS.GetScalarByDataSet();
            await DisplayAlert("Message", res, "OK");
            //
            await Shell.Current.Navigation.PopAsync();
        }
    }
}