using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MealApp
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    }


    public class CartItem
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }

        public List<CartItemAddon> ProductAddons { get; set; } = new List<CartItemAddon>();
        public double ProductPice { get; set; }

        public int ProductQuantity { get; set; }
        public double ProductTotal
        {
            get
            {
                return (ProductAddons.Sum(X => X.AddonPrice) + ProductPice) * ProductQuantity;
            }
        }


    }

    public class CartItemAddon
    {
        public string AddonID { get; set; }
        public string AddonName { get; set; }
        public double AddonPrice { get; set; }
    }


}
