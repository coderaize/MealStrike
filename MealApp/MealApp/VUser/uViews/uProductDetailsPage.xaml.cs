using System;
using System.Data;
using Xamarin.Forms;

namespace MealApp.VUser.uViews
{
    [QueryProperty("ProductID", nameof(ProductID))]
    public partial class uProductDetailsPage : ContentPage
    {
        private string _ProductID { get; set; } = "";
        public string ProductID
        {
            get => _ProductID;
            set
            {
                _ProductID = Uri.UnescapeDataString(value);
                SetPageDetails();
            }
        }

        //public string ProductName { get; set;}


        public uProductDetailsPage()
        {

            InitializeComponent();
            countLabel.Text = "1";

            iconBack.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SendBackButtonPressed())
            });
            iconCart.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    await Shell.Current.Navigation.PushAsync(new uCartPage());
                })
            });
        }


        public uProductDetailsPage(string ProductID)
        {

            InitializeComponent();
            countLabel.Text = "1";

            iconBack.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SendBackButtonPressed())
            });

            iconCart.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    await Shell.Current.Navigation.PushAsync(new uCartPage());
                })
            });

            _ProductID = ProductID;
            //Appearing += (x, y) =>
            //{

            SetPageDetails();
            //};
        }

        void SetPageDetails()
        {
            // Get Details for product
            if (string.IsNullOrEmpty(ProductID)) return;
            var dS = InfoDataBase.GetDS("EXEC ma_GetMealDetails @ID = @ID ", new System.Collections.Hashtable
            {
                {"ID",ProductID }
            });
            //////

            if (dS.Tables[0].Rows.Count > 0)
            {
                DataRow MealDR = dS.Tables[0].Rows[0];

                ProductNameTxt.Text = MealDR["Name"].ToString();
                ProductPriceTxt.Text = MealDR["Price"].ToString();
                ProductDescriptionTxt.Text = MealDR["Description"].ToString();
                //ProductImage.Source = ScalarFunctions.LoadImageFromBase64("");
                ProductImage.LoadUriSource = InfoDataBase.WebBaseUrl + MealDR["ImagePath"].ToString();
                TotalPriceTxt.Text = (countLabel.Text.ToInt32() * ProductPriceTxt.Text.ToDouble()).ToString();
                //if (ProductID == "1")
                //{
                //    ProductNameTxt.Text = "Habenero Kick";
                //    ProductPriceTxt.Text = "899";
                //    ProductDescriptionTxt.Text = "Dow men Cheeze";
                //    //ProductImage.Source = ScalarFunctions.LoadImageFromBase64("");
                //    ProductImage.LoadUriSource = "meal/Images/food1.png";
                //}
            }
            else
            {

                SendBackButtonPressed();
            }

        }

        private void increaseCountBtn_Clicked(object sender, EventArgs e)
            => ChangeQuantity(true);

        private void deccreaseCountBtn_Clicked(object sender, EventArgs e)
            => ChangeQuantity(false);


        void ChangeQuantity(bool Add)
        {
            if (string.IsNullOrEmpty(countLabel.Text))
                countLabel.Text = "1";
            int count = countLabel.Text.ToInt32();
            if (Add) count++;
            else count--;
            if (count >= 1)
                countLabel.Text = count.ToString();
            TotalPriceTxt.Text = (countLabel.Text.ToInt32() * ProductPriceTxt.Text.ToDouble()).ToString();
        }

        private async void AddToCartBtn_Clicked(object sender, EventArgs e)
        {
            if (countLabel.Text.ToInt32() < 1)
            {
                await DisplayAlert("No Count", "Please Set Quantity to atleast 1", "OK");
                return;
            }
            if (RuntimeCache.Cart.CartItems.Exists(X => X.ProductID == ProductID))
                RuntimeCache.Cart.CartItems.Find(X => X.ProductID == ProductID).ProductQuantity = countLabel.Text.ToInt32();
            else
                RuntimeCache.Cart.CartItems.Add(new CartItem
                {
                    ProductID = ProductID,
                    ProductName = ProductNameTxt.Text,
                    ProductPice = ProductPriceTxt.Text.ToDouble(),
                    ProductQuantity = countLabel.Text.ToInt32()
                });
            await Shell.Current.Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.Navigation.PopAsync();
            return true;
        }
    }
}