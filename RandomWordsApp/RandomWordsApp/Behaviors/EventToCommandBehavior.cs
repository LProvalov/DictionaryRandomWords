using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace RandomWordsApp
{
    public class EventToCommandBehavior : Behavior<View>
    {
        public static readonly BindableProperty EventNameProperty =
            BindableProperty.Create("EventName", typeof(string),
                typeof(EventToCommandBehavior), null, propertyChanged: propertyChangedHandler);
                
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        private static void propertyChangedHandler(BindableObject bindable, object oldValue, object newValue)
        {
            Debug.WriteLine(string.Format("prop changed:{0}", (string)newValue));
        }
    }
}
