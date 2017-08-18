using System;
using Xamarin.Forms;

namespace RandomWordsApp
{
    public partial class WordWithTranslationPage : ContentPage
    {
        private WordsProvider wordsProvider;
        private WordWithTranslationViewModel viewModel;
        public WordWithTranslationPage()
        {
            InitializeComponent();
            wordsProvider = WordsProvider.Instance;
            viewModel = new WordWithTranslationViewModel(wordsProvider);
            this.BindingContext = viewModel;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {

        }

        private void wordList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}