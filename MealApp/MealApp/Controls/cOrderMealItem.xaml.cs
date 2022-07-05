using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.Controls
{
    public partial class cOrderMealItem : ContentView
    {

        public string ProductName
        {
            get => _ProductName;
            set
            {
                _ProductName = value;
                productNameLabel.Text = value;
            }
        }
        private string _ProductName
        {
            get; set;
        }

        public string Quantity
        {
            get => _Quantity;
            set
            {
                _Quantity = value;
                quantityLabel.Text = value;
                SetTotal();
            }
        }
        private string _Quantity
        {
            get; set;
        }

        public string ItemPrice
        {
            get => _ItemPrice;
            set
            {
                _ItemPrice = value;
                priceLabel.Text = value;
                SetTotal();
            }
        }
        private string _ItemPrice
        {
            get; set;
        }

        public cOrderMealItem()
        {
            InitializeComponent();

            priceLabel.Text = ItemPrice;
            quantityLabel.Text = Quantity;
        }

        void SetTotal()
        {
            totalPriceLabel.Text = (ItemPrice.ToDouble() * Quantity.ToDouble()).ToString();
        }
    }
}