using MealApp.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.VUser.uViews
{
    [QueryProperty("OrderID", nameof(OrderID))]
    public partial class uOrderDetailsPage : ContentPage
    {

        private string _OrderID { get; set; } = "";
        public string OrderID
        {
            get => _OrderID;
            set
            {
                _OrderID = Uri.UnescapeDataString(value);
                getOrderDetails();
            }
        }

        public uOrderDetailsPage()
        {
            InitializeComponent();
            getOrderDetails();
        }

        public uOrderDetailsPage(string OrderID)
        {
            InitializeComponent();
            this.OrderID = OrderID;
            getOrderDetails();
        }

        void getOrderDetails()
        {
            if (!string.IsNullOrEmpty(OrderID))
                new Thread(new ThreadStart(delegate
                {
                    string query = "EXEC ma_GetOrderDetails @OrderID = @O";
                    Hashtable hT = new Hashtable { { "O", RuntimeCache.UserName } };
                    var dS = InfoDataBase.GetDS(query, hT);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        double subTotal = 0, total = 0, deliveryCost = 50;
                        foreach (DataRow dR in dS.Tables[1].Rows)
                        {
                            cOrderMealItem cMI = new cOrderMealItem();
                            cMI.ProductName = dR["Name"].ToString();
                            cMI.Quantity = dR["Quantity"].ToString();
                            cMI.ItemPrice = dR["SalePrice"].ToString();
                            orderItemsList.Children.Add(cMI);
                            subTotal += (dR["Quantity"].ToDouble() * dR["SalePrice"].ToDouble());
                        }
                        total = subTotal + deliveryCost;
                        totalPriceLabel.Text = total.ToString();
                        subTotalPriceLabel.Text = subTotal.ToString();
                        deliveryCostPriceLabel.Text = deliveryCost.ToString();
                    });

                })).Start();
        }
    }
}