using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.Controls
{


    public partial class cMealItem : ContentView
    {

        private string _ImageSourceLink { get; set; }
        public string ImageSourceLink
        {
            get => _ImageSourceLink;
            set
            {
                _ImageSourceLink = value;
                ProductImage.LoadUriSource = ImageSourceLink;
            }
        }
        private string _MealName { get; set; }
        public string MealName
        {
            get => _MealName;
            set
            {
                _MealName = value;
                mealNameText.Text = value;
            }
        }

        private double _MealPrice { get; set; }
        public double MealPrice
        {
            get => _MealPrice;
            set
            {
                _MealPrice = value;
                mealPriceTxt.Text = "Rs: " + MealPrice.ToString();
            }
        }


        public cMealItem()
        {
            InitializeComponent();

            mealNameText.Text = MealName;
            mealPriceTxt.Text = "Rs: " + MealPrice.ToString();
            //SetRatingText();
            ProductImage.LoadUriSource = ImageSourceLink;
            //
            mainFrame.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => { cClicked?.Invoke(null, new EventArgs()); })
            });

        }

        public event EventHandler cClicked;
    }
}