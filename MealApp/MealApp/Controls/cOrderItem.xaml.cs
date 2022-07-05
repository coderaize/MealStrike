using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.Controls
{
    public partial class cOrderItem : ContentView
    {

        private string _OrderID { get; set; } = "";
        public string OrderID
        {
            get => _OrderID;
            set
            {
                _OrderID = value;
                orderIDLabel.Text = OrderID;
            }
        }
        private string _CreationTime { get; set; } = "";
        public string CreationTime
        {
            get => _CreationTime;
            set
            {
                _CreationTime = value;
                orderCreationTimeLabel.Text = CreationTime;
            }
        }

        private string _DeliveryStatus { get; set; } = "";
        public string DeliveryStatus
        {
            get => _DeliveryStatus;
            set
            {
                _DeliveryStatus = value;
                deliveryLabel.Text = DeliveryStatus;
            }
        }

        public event EventHandler cClicked;

        public cOrderItem()
        {
            InitializeComponent();
            mainGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    cClicked?.Invoke(null, new EventArgs());
                })
            });

            orderIDLabel.Text = OrderID;
            deliveryLabel.Text = DeliveryStatus;
        }
    }
}