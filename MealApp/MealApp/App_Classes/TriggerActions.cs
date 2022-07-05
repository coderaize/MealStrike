using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MealApp
{
    public class MonkeyTriggerAction : TriggerAction<Entry>
    {
        protected override void Invoke(Entry sender)
        {
            sender.BackgroundColor = Color.Gray;
            //((Frame)sender.Parent).
        }
    }
}
