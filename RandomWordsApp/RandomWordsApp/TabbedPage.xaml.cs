using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RandomWordsApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        public TabbedPage1()
        {
            InitializeComponent();
            var pages = Children.GetEnumerator();
            pages.MoveNext();
            pages.MoveNext();
            CurrentPage = pages.Current;
        }
    }
}