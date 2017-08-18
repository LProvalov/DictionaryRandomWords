using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RandomWordsApp
{
    public class WordWithTranslationViewModel : INotifyPropertyChanged
    {
        private const string DefaultButtonName = "Update/Add";
        private const string AddButtonName = "Add word";
        private const string UpdateButtonName = "Update word";
        public enum WWTViewModelStates
        {
            Main,
            //SearchRus,
            //SearchEng,
            AddingNew,
            Updating
        }
        public enum ValueChanged
        {
            Rus,
            Eng,
            Description,
            WordSelected
        }

        private WordWithTranslationModel wordModel;
        private WordsProvider wordsProvider;
        private WWTViewModelStates state;
        private int selectedItemId = -1;
        protected WWTViewModelStates State
        {
            get { return state; }
            set
            {
                Debug.WriteLine(string.Format("State, old:{0} new:{1}", state.ToString(), value.ToString()));
                if (state != value)
                {
                    //if(state == WWTViewModelStates.Main && value == WWTViewModelStates.SearchRus)
                    //{
                    //    IsDescriptionEntryEnabled = false;
                    //    IsEngEntryEnabled = false;
                    //}
                    //if(state == WWTViewModelStates.Main && value == WWTViewModelStates.SearchEng)
                    //{
                    //    IsDescriptionEntryEnabled = false;
                    //    IsRusEntryEnabled = false;
                    //}

                    if (value == WWTViewModelStates.Main)
                    {
                        IsRusEntryEnabled = true;
                        IsEngEntryEnabled = true;
                        IsDescriptionEntryEnabled = false;
                        ButtonText = DefaultButtonName;
                        IsButtonEnabled = false;
                    }
                    if (value == WWTViewModelStates.Updating)
                    {
                        IsRusEntryEnabled = true;
                        IsEngEntryEnabled = true;
                        IsDescriptionEntryEnabled = true;
                        ButtonText = UpdateButtonName;
                        IsButtonEnabled = true;
                    }
                    if (value == WWTViewModelStates.AddingNew)
                    {
                        IsRusEntryEnabled = true;
                        IsEngEntryEnabled = true;
                        IsDescriptionEntryEnabled = true;
                        ButtonText = AddButtonName;
                        IsButtonEnabled = true;
                    }
                    state = value;
                }
            }
        }

        public WordWithTranslationViewModel(WordsProvider wordsProvider)
        {
            state = WWTViewModelStates.Main;
            wordModel = new WordWithTranslationModel();
            this.wordsProvider = wordsProvider;
            this.WordDetailCommand = new Command<Word>(WordDetail);
            this.ClearRusEntryCommand = new Command(ClearRusEntry);
            Words = wordsProvider.GetAllObservableCollection();
        }
        public ICommand WordDetailCommand { protected set; get; }
        private void WordDetail(Word selectedWord)
        {
            wordModel.Rus = selectedWord.Rus;
            wordModel.Eng = selectedWord.Eng;
            wordModel.Description = selectedWord.Description;
            OnPropertyChanged("Rus");
            OnPropertyChanged("Eng");
            OnPropertyChanged("Description");
            if (selectedItemId != -1 && selectedItemId == selectedWord.Id)
            {
                SelectedItem = null;
                selectedItemId = -1;
            }
            else
            {
                selectedItemId = selectedWord.Id;
            }
            FilterDictionary();
            //OnPropertyChanged("Words");
            CheckState();
            Debug.WriteLine(string.Format("Tapped item: {0} - {1}, {2}",
                selectedWord.Id, selectedWord.Rus, selectedWord.Eng));
        }

        public ICommand ClearRusEntryCommand { protected set; get; }
        private void ClearRusEntry()
        {
            Rus = string.Empty;
        }
        public ICommand ClearEngEntryCommand { protected set; get; }
        private void ClearEngEntry()
        {
            Eng = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public string Eng
        {
            get { return this.wordModel.Eng; }
            set
            {
                if (this.wordModel.Eng != value)
                {
                    Debug.WriteLine(string.Format("Eng prop changed: {0}", value));
                    this.wordModel.Eng = value;
                    OnPropertyChanged("Eng");

                    FilterDictionary();
                    CheckState();

                    //if (State == WWTViewModelStates.Main)
                    //{
                    //    State = WWTViewModelStates.SearchEng;
                    //}
                    //else if (FilterDictionaryUsingState(value) == 0)
                    //{
                    //    if (IsMainState()) State = WWTViewModelStates.Main;
                    //    else State = WWTViewModelStates.AddingNew;
                    //}                    
                }
            }
        }
        public string Rus
        {
            get { return this.wordModel.Rus; }
            set
            {
                if (this.wordModel.Rus != value)
                {
                    Debug.WriteLine(string.Format("Rus prop changed: {0}", value));
                    this.wordModel.Rus = value;
                    OnPropertyChanged("Rus");

                    FilterDictionary();
                    CheckState();


                    //if (State == WWTViewModelStates.Main)
                    //{
                    //    State = WWTViewModelStates.SearchRus;
                    //}
                    //else if (FilterDictionaryUsingState(value) == 0)
                    //{
                    //    if (IsMainState()) State = WWTViewModelStates.Main;
                    //    else State = WWTViewModelStates.AddingNew;
                    //}                   
                }
            }
        }
        public string Description
        {
            get { return this.wordModel.Description; }
            set
            {
                if (this.wordModel.Description != value)
                {
                    this.wordModel.Description = value;
                    OnPropertyChanged("Description");
                }
            }

        }
        public bool IsEngEntryEnabled
        {
            get
            {
                return wordModel.IsEngEntryEnabled;
            }
            set
            {
                if (wordModel.IsEngEntryEnabled != value)
                {
                    Debug.WriteLine(string.Format("IsEngEntryEnabled:{0}", value));
                    wordModel.IsEngEntryEnabled = value;
                    OnPropertyChanged("IsEngEntryEnabled");
                }
            }
        }
        public bool IsRusEntryEnabled
        {
            get { return wordModel.IsRusEntryEnabled; }
            set
            {
                if (wordModel.IsRusEntryEnabled != value)
                {
                    Debug.WriteLine(string.Format("IsRusEntryEnabled:{0}", value));
                    wordModel.IsRusEntryEnabled = value;
                    OnPropertyChanged("IsRusEntryEnabled");
                }
            }
        }
        public bool IsDescriptionEntryEnabled
        {
            get { return wordModel.IsDescriptionEntryEnabled; }
            set
            {
                if (wordModel.IsDescriptionEntryEnabled != value)
                {
                    Debug.WriteLine(string.Format("IsDescriptionEntryEnabled:{0}", value));
                    wordModel.IsDescriptionEntryEnabled = value;
                    OnPropertyChanged("IsDescriptionEntryEnabled");
                }
            }
        }
        private bool isButtonEnabled = false;
        public bool IsButtonEnabled
        {
            get { return isButtonEnabled; }
            set
            {
                if (isButtonEnabled != value)
                {
                    Debug.WriteLine(string.Format("IsButtonEnabled: {0}", isButtonEnabled));
                    isButtonEnabled = value;
                    OnPropertyChanged("IsButtonEnabled");
                }
            }
        }
        private string buttonText = DefaultButtonName;
        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                if (buttonText != value)
                {
                    Debug.WriteLine(string.Format("ButtonText:{0}", buttonText));
                    buttonText = value;
                    OnPropertyChanged("ButtonText");
                }
            }
        }

        private Word selectedItem;
        public Word SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        private ObservableCollection<Word> words;
        public ObservableCollection<Word> Words
        {
            get { return words; }
            set
            {
                if (words != value)
                {
                    words = value;
                    OnPropertyChanged("Words");
                }
            }
        }

        //private int FilterDictionaryUsingState(string partOfWord)
        //{
        //    switch (State)
        //    {
        //        case WWTViewModelStates.SearchEng:
        //            Words = wordsProvider.GetFilteredObservableCollection(partOfWord, "Eng");
        //            return Words.Count;
        //        case WWTViewModelStates.SearchRus:
        //            Words = wordsProvider.GetFilteredObservableCollection(partOfWord, "Rus");
        //            return Words.Count;
        //        default:
        //            return -1;
        //    }            
        //}
        private void FilterDictionary()
        {
            Words = wordsProvider.GetFilteredObservableCollectionByInput(wordModel.Rus, wordModel.Eng);
        }

        private void CheckState()
        {
            if (selectedItemId == -1 && words.Count == 0 && (wordModel.Eng.Length > 0 || wordModel.Rus.Length > 0))
            {
                State = WWTViewModelStates.AddingNew;
            }
            else if (selectedItemId != -1)
            {
                State = WWTViewModelStates.Updating;
            }
            else
            {
                State = WWTViewModelStates.Main;
            }
        }
        //private bool IsMainState()
        //{
        //    if (Rus == string.Empty && Eng == string.Empty && Description == string.Empty /*&& selectedWord == null*/)
        //    {
        //        Debug.WriteLine("IsMainState: true");
        //        return true;
        //    }
        //    else
        //    {
        //        Debug.WriteLine("IsMainState: false");
        //        return false;
        //    }
        //}
    }
}
