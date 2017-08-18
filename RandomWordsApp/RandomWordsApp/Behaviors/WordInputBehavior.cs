using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace RandomWordsApp
{
    public class WordInputBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.Completed += OnEntryCompleted;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Completed -= OnEntryCompleted;
            base.OnDetachingFrom(bindable);
        }

        void OnEntryCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("OnEntryCompleted method called...");
        }
    }
}
