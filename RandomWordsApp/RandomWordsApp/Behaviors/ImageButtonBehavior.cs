using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace RandomWordsApp
{
    public class ImageButtonBehavior : Behavior<Image>
    {
        private TapGestureRecognizer tapGesteture;
        public event EventHandler OnClickEvent;
        public ImageButtonBehavior() : base()
        {
            tapGesteture = new TapGestureRecognizer() { NumberOfTapsRequired = 1 };
            tapGesteture.Tapped += OnClick;
        }
        protected override void OnAttachedTo(Image bindable)
        {
            bindable.GestureRecognizers.Add(tapGesteture);
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Image bindable)
        {
            bindable.GestureRecognizers.Remove(tapGesteture);
            base.OnDetachingFrom(bindable);
        }

        void OnClick(object sender, EventArgs e)
        {
            Debug.WriteLine("On image click");
            if (this.OnClickEvent != null) OnClickEvent(sender, e);            
        }
    }
}
