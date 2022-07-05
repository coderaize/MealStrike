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
    public partial class uOrdersPage : ContentPage
    {
        public uOrdersPage()
        {
            InitializeComponent();
            getOrdersList();
            Appearing += (a, b) =>
            {
                getOrdersList();
            };
            refresher.Refreshing += (x, y) =>
            {
                getOrdersList();
            };
        }


        void getOrdersList()
        {
            new Thread(new ThreadStart(delegate
            {
                string query = "EXEC ma_GetUserOrders @UserName = @U";
                Hashtable hT = new Hashtable { { "U", RuntimeCache.UserName } };
                var dS = InfoDataBase.GetDS(query, hT);
                //
                Device.BeginInvokeOnMainThread(() =>
                {
                    ordersList.Children.Clear();
                    foreach (DataRow dR in dS.Tables[0].Rows)
                    {
                        cOrderItem cI = new cOrderItem();
                        cI.OrderID = dR["OrderID"].ToString();
                        cI.DeliveryStatus = dR["DeliveryStatus"].ToString();
                        cI.CreationTime = dR["CreationTime"].ToDateTime().ToString("yyyy/MM/dd");
                        cI.cClicked += async (x, y) =>
                        {
                            await Shell.Current.Navigation.PushAsync(new uOrderDetailsPage(dR["OrderID"].ToString()));
                        };
                        ordersList.Children.Add(cI);
                    }
                });
            })).Start();
        }
    }
}