using System;
using System.Diagnostics;
using System.ComponentModel;
using Xamarin.Forms;

namespace RandomWordsApp
{
    public partial class MainPage : ContentPage
    {
        private WordsProvider wordsProvider;
        private MainPageViewModel mainPageVM;
        
        public MainPage()
        {
            InitializeComponent();

            Debug.WriteLine("Main page");   
            wordsProvider = WordsProvider.Instance;
            mainPageVM = new MainPageViewModel(wordsProvider);
            mainPageVM.WordEnteredChangeEvent += this.WordEnteredPropertyChangeHandler;
            mainPageVM.InputTypeChangeEvent += this.InputTypeChangedHandler;
            this.BindingContext = mainPageVM;
            
            TapGestureRecognizer tapGesture = new TapGestureRecognizer() { NumberOfTapsRequired = 1 };
            tapGesture.Tapped += (object sender, EventArgs args) => { mainPageVM.NextWord(); };
            this.wordStack.GestureRecognizers.Add(tapGesture);
            Resources["wordEntryStyle"] = Resources["DefaultInput"];
        }

        void OnComplitedHandler(object sender, EventArgs args)
        {
            mainPageVM.WordEntry();
        }

        void WordEnteredPropertyChangeHandler(object sender, PropertyChangedEventArgs args)
        {
            
        }

        void InputTypeChangedHandler(object sender, InputTypeChangeEventArgs args)
        {
            switch (args.newValue)
            {
                case InputType.Correct:
                    Resources["wordEntryStyle"] = Resources["SuccessInput"];
                    break;
                case InputType.Default:
                    Resources["wordEntryStyle"] = Resources["DefaultInput"];
                    break;
                case InputType.Error:
                    Resources["wordEntryStyle"] = Resources["ErrorInput"];
                    break;
            }            
        }

        private async void ImageButtonBehavior_OnClickEvent(object sender, EventArgs e)
        {
            Debug.WriteLine("ImageButtonBehavior_OnClickEvent");
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}
