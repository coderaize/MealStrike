using System;
using System.Collections;
using System.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace MealApp.VUser.uViews
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class uCheckoutPage : ContentPage
    {
        public uCheckoutPage()
        {
            InitializeComponent();

            changeAddressLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    addressLabel.Text =
                    await MaterialDialog.Instance.InputAsync("Delivery Note", "Please enter a delivery note", "", "", "OK", "Cancel");
                })
            });

            addCardLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    await Shell.Current.Navigation.PushAsync(new uAddCardPage());
                })
            });
            changePaymentMehtodLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    var jobs = new string[]
                    {
                        "Cash On Delivery",
                        "21123***123",
                    };

                    //Show confirmation dialog for choosing one.
                    var result = await MaterialDialog.Instance.SelectChoiceAsync(title: "Select a job", choices: jobs);
                    string res = jobs[result];
                    paymentMehthodLabel.Text = res;
                })
            });



            ///
            // Set Cart an price Details
            double subTotal = 0d, deliveryCost = 0d, total = 0d;
            RuntimeCache.Cart.CartItems.ForEach(item =>
            {
                subTotal += (item.ProductQuantity * item.ProductPice);
            });
            deliveryCost = 50;
            total = deliveryCost + subTotal;
            //
            subTotalLabel.Text = subTotal.ToString();
            deliveryLabel.Text = deliveryCost.ToString();
            totalLabel.Text = total.ToString();
            Appearing += async (x, y) =>
            {

                DataSet dS = await InfoDataBase.GetDSAsync("EXEC ma_GetUserAddress @UserName = @U", new Hashtable { { "U", RuntimeCache.UserName } });
                string address = dS.GetScalarByDataSet();
                if (address == "{GEO}")
                {
                    Location location = await ScalarFunctions.GetGeoLoctionAsync();
                    Hashtable ahT = await InfoDataBase.GetDetailsByLocationCodesAsync(RuntimeCache.UserName, location.Latitude.ToString(), location.Longitude.ToString());
                    if (ahT.Contains("Address"))
                        addressLabel.Text = ahT["Address"].ToString();
                }
                else
                {
                    addressLabel.Text = address;
                }
            };
        }



        protected override bool OnBackButtonPressed()
        {
            Shell.Current.Navigation.PopAsync();
            return true;
        }

        private async void PlaceOrderButton_Clicked(object sender, EventArgs e)
        {
            string cartTimesJson = RuntimeCache.Cart.CartItems.AsJson();
            string address = addressLabel.Text;
            string Query = "EXEC ma_SubmitOrder @UserName=@U,@Address=@A,@CartItemJSON=@Cart";
            Hashtable hT = new Hashtable
            {
                { "U",RuntimeCache.UserName},
                { "A",address},
                { "Cart",cartTimesJson}
            };
            var dS = await InfoDataBase.GetDSAsync(Query, hT);
            if (dS.GetScalarByDataSet() == "Submitted")
            {
                var DeliveryTime = dS.Tables[0].Rows[0]["DeliveryTime"].ToDateTime();
                await DisplayAlert("Order Placed", $"Delivery time is {DeliveryTime}", "OK");
                RuntimeCache.Cart.CartItems.Clear();
                await Shell.Current.Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", $"{dS.GetScalarByDataSet()}", "OK");
            }
        }
    }
}