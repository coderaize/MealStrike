using MealApp.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealApp.VUser.uViews
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class uHomePage : ContentPage
    {

        DataTable MealsDT { get; set; }
        DataTable CategoryDT { get; set; }

        string currentCategory;

        public uHomePage()
        {
            InitializeComponent();

            

            userNameLabel.Text = RuntimeCache.UserName;
            startup();
            refresher.Refreshing += (x, y) =>
            {
                searchTxt.Text = "";
                currentCategory = "All";
                SetCategoryActive();
                startup();
                refresher.IsRefreshing = false;
            };
        }

        void startup()
        {

            new Thread(new ThreadStart(delegate
            {

                var dS = InfoDataBase.GetDS("EXEC ma_GetHomePageData", new System.Collections.Hashtable());
                Device.BeginInvokeOnMainThread(() =>
                {
                    MealsDT = dS.Tables[1];
                    CategoryDT = dS.Tables[0];
                    SetCategories();
                    //SetMeals(MealsDT.Select());
                    SetMealsAfterLoading();
                });
                Thread.Sleep(3600);


            })).Start();
        }

        private async void CartItem_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new uCartPage());
        }


        void SetCategories()
        {
            //
            optionValues.Children.Clear();
            cIconOption cIcon = new cIconOption
            {
                WidthRequest = 120,
                HeightRequest = 110,
                LabelText = "All",
                ImageSourceLink = InfoDataBase.WebBaseUrl + "Images/AppImages/meal_all.jpg"
            };
            cIcon.cClicked += (x, y) =>
            {
                searchTxt.Text = "";
                currentCategory = cIcon.LabelText;
                SetMealsAfterLoading();
                SetCategoryActive();
            };
            optionValues.Children.Add(cIcon);
            foreach (DataRow CategoryDR in CategoryDT.Rows)
            {
                cIconOption cIconOption = new cIconOption
                {
                    WidthRequest = 120,
                    HeightRequest = 110,
                    LabelText = CategoryDR["Category"].ToString(),
                    ImageSourceLink = InfoDataBase.WebBaseUrl + CategoryDR["ImagePath"].ToString(),
                };
                cIconOption.cClicked += (xx, yy) =>
                {
                    currentCategory = cIconOption.LabelText;
                    SetMealsAfterLoading();
                    SetCategoryActive();
                };
                optionValues.Children.Add(cIconOption);
            }
        }

        void SetMealsAfterLoading()
        {
            var dataRows = MealsDT.Select();
            string query = "";
            if (!string.IsNullOrEmpty(currentCategory) && currentCategory != "All")
                dataRows = dataRows.Where(X => X["Category"].ToString() == currentCategory).ToArray();
            //query += $" Category = '{currentCategory}'  AND";
            if (!string.IsNullOrEmpty(searchTxt.Text))
                dataRows = dataRows.Where(X => X["Name"].ToString().Contains(searchTxt.Text)).ToArray();
            //query += $" Name Like '%{currentCategory}%'";
            DataRow[] rows = MealsDT.Select(query);
            SetMeals(rows);
        }

        void SetMeals(DataRow[] mealsDrArr)
        {

            mealItems.Children.Clear();
            if (mealsDrArr.Count() == 0)
            {
                mealItems.Children.Add(new Label { Text = "Nothing in this context" });
                return;
            }
            foreach (DataRow MealDR in mealsDrArr)
            {
                cMealItem cMI = new cMealItem
                {
                    MealName = MealDR["Name"].ToString(),
                    MealPrice = MealDR["Price"].ToDouble(),
                    ImageSourceLink = InfoDataBase.WebBaseUrl + MealDR["ImagePath"].ToString()
                };
                cMI.cClicked += async (xx, yy) =>
                {
                    await Shell.Current.Navigation.PushAsync(new uProductDetailsPage(MealDR["ID"].ToString()));
                };
                mealItems.Children.Add(cMI);
            }

        }


        void SetCategoryActive()
        {
            foreach (View v in optionValues.Children)
            {
                var iconOpt = (cIconOption)v;
                iconOpt.ActiveColor = Color.FromHex("#FEFEFE");
                if (iconOpt.LabelText == currentCategory)
                    iconOpt.ActiveColor = Color.FromHex("#F3F3F3");
            }

        }

        private void MaterialTextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetMealsAfterLoading();
        }
    }
}