using System;
using System.Windows.Input;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;

namespace RandomWordsApp
{
    public enum InputType
    {
        Error,
        Correct,
        Default
    }
    public class InputTypeChangeEventArgs : EventArgs
    {
        public InputType oldValue;
        public InputType newValue;
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageModel model;
        private WordsProvider wordsProvider;
        private string wordEntryText;

        private bool isWordEntered;
        public bool IsWordEntered
        {
            get { return isWordEntered; }
            set
            {
                if(isWordEntered != value)
                {
                    isWordEntered = value;
                    if (WordEnteredChangeEvent != null)
                        WordEnteredChangeEvent(this, new PropertyChangedEventArgs("IsWordEntered"));
                }
            }
        }

        private InputType inputType = InputType.Default;
        private InputType InputTypeProp
        {
            get { return inputType; }
            set
            {
                if(inputType != value)
                {
                    var args = new InputTypeChangeEventArgs() {
                        oldValue = inputType, newValue = value
                    };
                    inputType = value;
                    if (InputTypeChangeEvent != null)
                        InputTypeChangeEvent(this, args);
                }
            }
        }
        
        public event PropertyChangedEventHandler WordEnteredChangeEvent;
        public event EventHandler<InputTypeChangeEventArgs> InputTypeChangeEvent;

        public ICommand NextPartCommand { protected set; get; }
        public ICommand PrevPartCommand { protected set; get; }
        private void NextPart()
        {
            Debug.WriteLine("Next part method called...");
            wordsProvider.PartNext();
            isWordEntered = false;
            NextWord();
            OnPropertyChanged("NumberOfPartString");
        }
        private void PrevPart()
        {
            Debug.WriteLine("Prev part method called...");
            wordsProvider.PartPrev();
            isWordEntered = false;
            NextWord();
            OnPropertyChanged("NumberOfPartString");
        }
        private void CheckWord()
        {
            Debug.WriteLine("CheckWord method called...");
            var wwt = wordsProvider.Current();
            if (wordEntryText.Trim().ToUpper() != wwt.Rus.ToUpper())
            {
                InputTypeProp =  InputType.Error;
            }
            else
            {
                InputTypeProp = InputType.Correct;
            }
            Word = string.Format("{0} - {1}", wwt.Eng, wwt.Rus);
            Description = wwt.Description;
        }
        public void NextWord()
        {
            Debug.WriteLine(string.Format("NextWord method called, isWordEntered: {0}", isWordEntered));
            if (isWordEntered == false)
            {                
                Word word = wordsProvider.GetNextRandom();
                Word = word.Eng;
                InputTypeProp = InputType.Default;
                Description = "";
                WordEntryText = "";
                isWordEntered = true;                
            }
        }
        public MainPageViewModel(WordsProvider wordsProvider)
        {
            model = new MainPageModel();
            this.wordsProvider = wordsProvider;

            this.NextPartCommand = new Command(NextPart);
            this.PrevPartCommand = new Command(PrevPart);

            model.Word = wordsProvider.GetNextRandom().Eng;
            model.Description = string.Empty;

            Debug.WriteLine(string.Format("MainPageViewModel constructor, word: {0}, desc: {1}", 
                model.Word, model.Description));
        }
        public string Word
        {
            get { return model.Word; }
            set
            {
                if (model.Word != value)
                {
                    Debug.WriteLine(string.Format("Set new word: {0}", value));
                    model.Word = value;
                    OnPropertyChanged("Word");
                }
            }
        }
        public string Description
        {
            get { return model.Description; }
            set
            {
                if (model.Description != value)
                {
                    Debug.WriteLine(string.Format("Set new description: {0}", value));
                    model.Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public string NumberOfPartString
        {
            get
            {
                return string.Format("{0} / {1}", 
                    wordsProvider.GetPartIndex() + 1, wordsProvider.GetPartsCount());
            }
        }
        public string WordEntryText
        {
            get { return wordEntryText; }
            set
            {                
                if (wordEntryText != value)
                {
                    Debug.WriteLine(string.Format("wordEntryText: {0}", value));
                    wordEntryText = value;
                    isWordEntered = true;
                    OnPropertyChanged("WordEntryText");
                }
            }
        }
        public void WordEntry()
        {
            isWordEntered = false;
            CheckWord();
        }
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
