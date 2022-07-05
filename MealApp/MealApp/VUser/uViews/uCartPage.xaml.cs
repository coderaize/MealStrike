using MealApp.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace MealApp.VUser.uViews
{
    public partial class uCartPage : ContentPage
    {
        private string DeliveryNote { get; set; }

        public uCartPage()
        {
            InitializeComponent();

            deliveryNoteAddNoteLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    DeliveryNote = await MaterialDialog.Instance.InputAsync("Delivery Note", "Please enter a delivery note", "", "", "OK", "Cancel");
                    SetDeliveryNote();
                })
            });
            SetCartItems();
            Appearing += (x, y) =>
            {
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    SetCartItems();
                    return false;
                });
            };
        }

        void SetDeliveryNote()
        {
            if (string.IsNullOrEmpty(DeliveryNote))
            {
                deliveryNoteRow.Height = 0;
                deliveryNoteLabel.Text = DeliveryNote;
            }
            else
            {
                deliveryNoteRow.Height = 40;
                deliveryNoteLabel.Text = DeliveryNote;
            }

        }

        void SetCartItems()
        {
            cartItemsStackPanel.Children.Clear();
            RuntimeCache.Cart.CartItems.ForEach(X =>
            {
                var cartItem = X;
                var cartItemView = new cCartItem(cartItem);

                cartItemView.OnCartItemRemoved += (x, y) =>
                {
                    cartItemsStackPanel.Children.Remove(cartItemView);
                    RuntimeCache.Cart.CartItems.Remove(cartItemView.CartItem);
                };

                cartItemView.OnCartItemRemoved += (x, y) =>
                {
                    CalculateTotals();
                };
                cartItemsStackPanel.Children.Add(cartItemView);

            });
            CalculateTotals();

        }

        void CalculateTotals()
        {
            double subtotal = RuntimeCache.Cart.CartItems.Sum(X => X.ProductTotal);
            double deliveryCharges = 50;
            subTotalPriceLabel.Text = subtotal.ToString();
            deliveryCostPriceLabel.Text = deliveryCharges.ToString();
            totalPriceLabel.Text = (subtotal + deliveryCharges).ToString();
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.Navigation.PopAsync();
            return true;
        }

        private async void checkoutButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new uCheckoutPage());
        }

        private async void HomePageIcon_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopToRootAsync();
        }
    }
}