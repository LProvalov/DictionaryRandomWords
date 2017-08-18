using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace RandomWordsApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var tabbedPage = new RandomWordsApp.TabbedPage1();
            NavigationPage navigationPage = new NavigationPage(tabbedPage);
            NavigationPage.SetHasNavigationBar(tabbedPage, false);
            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
