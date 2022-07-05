using System;
using Xamarin.Forms;

namespace MealApp.Controls
{
    public partial class cCartItem : ContentView
    {

        private string _CartItemProductID { get; set; }
        public string CartItemProductID
        {
            get => _CartItemProductID;
            set
            {
                _CartItemProductID = value;
                SetCartDetail();
            }
        }

        private void SetCartDetail()
        {


            if (CartItem == null && !string.IsNullOrEmpty(_CartItemProductID))
                CartItem = RuntimeCache.Cart.CartItems.Find(X => X.ProductID == _CartItemProductID);


            productNameLabel.Text = $"{CartItem.ProductName} X {CartItem.ProductQuantity}";
            productPortionPrice.Text = CartItem.ProductPice.ToString();
            totalPriceLabel.Text = CartItem.ProductTotal.ToString();
            countLabel.Text = CartItem.ProductQuantity.ToString();
            
            if (CartItem.ProductQuantity == 1)
                deccreaseCountBtn.BackgroundColor = Color.Red;
            else
                deccreaseCountBtn.BackgroundColor = (Color)Application.Current.Resources["Primary"];

            OnCartItemSet?.Invoke(null, new EventArgs());
        }

        public CartItem CartItem { get; set; }

        public cCartItem()
        {
            InitializeComponent();
            SetCartDetail();
        }
        public cCartItem(CartItem cartItem)
        {
            InitializeComponent();
            CartItem = cartItem;
            SetCartDetail();
            //ProductTitleRowGrid.GestureRecognizers.Add(new TapGestureRecognizer
            //{
            //    Command = new Command(() =>
            //    {
            //        if (productDetailsRow.Height.Value == 50)
            //        {
            //            productDetailsRow.Height = 0;
            //        }
            //        else
            //        {
            //            productDetailsRow.Height = 50;
            //        }
            //    })
            //});

        }

        public event EventHandler OnCartItemSet;
        public event EventHandler OnCartItemRemoved;

        private void deccreaseCountBtn_Clicked(object sender, EventArgs e)
            => ChangeCartItem(-1);

        private void increaseCountBtn_Clicked(object sender, EventArgs e)
            => ChangeCartItem(1);

        void ChangeCartItem(int modifier)
        {

            CartItem.ProductQuantity = CartItem.ProductQuantity + (modifier);


            if (CartItem.ProductQuantity == (1 | 0))
                //Danger
                deccreaseCountBtn.BackgroundColor = Color.FromHex("#D61010");
            else
                //Primary
                deccreaseCountBtn.BackgroundColor = Color.FromHex("#7b34aa");

            SetCartDetail();
            if (CartItem.ProductQuantity == 0)
                OnCartItemRemoved?.Invoke(null, new EventArgs());



        }


    }
}