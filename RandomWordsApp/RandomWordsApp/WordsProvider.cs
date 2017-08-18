using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RandomWordsApp
{
    public class WordsProvider
    {
        private const int PART_SIZE = 5;
        int dictionaryPartsCount = 0;
        IList<Word> currentPart = null;

        char chachedWordsChar;
        IList<Word> chachedWords = null;

        private int currentWordIndex = -1;
        private int currentPartIndex = 0;

        public const string DATABASE_NAME = "words.db";
        private static WordRepository repository;
        public static WordRepository Repository
        {
            get
            {
                if (repository == null)
                {
                    repository = new WordRepository(DATABASE_NAME);
                }
                return repository;
            }
        }

        protected WordsProvider()
        {
            Debug.WriteLine("WordsProvider, constructor");
            if (Repository.GetLength() == 0)
            {
                Repository.SaveItem(new Word() { Rus = "скользкий", Eng = "slippery", Description = "Описание, синонимы, примеры." });
                Repository.SaveItem(new Word() { Rus = "безразличный", Eng = "indifferent", Description = "Описание, синонимы, примеры 2." });
                Repository.SaveItem(new Word() { Rus = "смягчение", Eng = "mitigation", Description = "Описание, синонимы, примеры 3." });
                Repository.SaveItem(new Word() { Rus = "вязать, соединять", Eng = "knit", Description = "Описание, синонимы, примеры 4." });
                Repository.SaveItem(new Word() { Rus = "различать", Eng = "Distinguish", Description = "Описание, синонимы, примеры 5." });
                Repository.SaveItem(new Word() { Rus = "хвалить", Eng = "praice", Description = "Описание, синонимы, примеры 6." });
                Repository.SaveItem(new Word() { Rus = "нежный", Eng = "affectionate", Description = "Описание, синонимы, примеры 7." });
                Repository.SaveItem(new Word() { Rus = "случайная встреча", Eng = "encounter", Description = "Описание, синонимы, примеры 8." });
                Repository.SaveItem(new Word() { Rus = "развиваться", Eng = "evolve", Description = "Описание, синонимы, примеры 9." });
                Repository.SaveItem(new Word() { Rus = "дальше", Eng = "further", Description = "Описание, синонимы, примеры 10." });
                Repository.SaveItem(new Word() { Rus = "изолировать", Eng = "insulate", Description = "Описание, синонимы, примеры 11." });
                Repository.SaveItem(new Word() { Rus = "подробный", Eng = "explicit", Description = "Описание, синонимы, примеры 12." });
                Repository.SaveItem(new Word() { Rus = "определять", Eng = "determine", Description = "Описание, синонимы, примеры 13." });
                Repository.SaveItem(new Word() { Rus = "энергичный", Eng = "vigorous", Description = "Описание, синонимы, примеры 14." });
                Repository.SaveItem(new Word() { Rus = "возмездие", Eng = "retribution", Description = "Описание, синонимы, примеры 15." });
                Repository.SaveItem(new Word() { Rus = "против", Eng = "against", Description = "Описание, синонимы, примеры 16." });
                Repository.SaveItem(new Word() { Rus = "вражеский", Eng = "hostile", Description = "Описание, синонимы, примеры 17." });
                Repository.SaveItem(new Word() { Rus = "намерение", Eng = "intention", Description = "Описание, синонимы, примеры 18." });
                Repository.SaveItem(new Word() { Rus = "ранить", Eng = "injure", Description = "Описание, синонимы, примеры 19." });
            }

            dictionaryPartsCount = Repository.GetLength() / PART_SIZE;
            if (Repository.GetLength() % PART_SIZE > 0) dictionaryPartsCount++;
            Debug.WriteLine("WordsProvider, trying to get first part");
            currentPart = Repository.GetPart(PART_SIZE, 0);
            Debug.WriteLine("WordsProvider, success");
        }

        private sealed class WordsProviderCreator
        {
            private static readonly WordsProvider instance = new WordsProvider();
            public static WordsProvider Instance { get { return instance; } }
        }

        public static WordsProvider Instance
        {
            get { return WordsProviderCreator.Instance; }
        }

        public Word GetNextRandom()
        {
            Debug.WriteLine("WordsProvider, getNextRandom");
            if (currentPart != null && currentPart.Count > 0)
            {
                Debug.WriteLine(string.Format("WordsProvider, currentPart != null, currentWordIndex:{0}", currentWordIndex));
                Random rnd = new Random((int)DateTime.Now.Ticks);
                int position = 0;
                do
                {
                    position = rnd.Next(currentPart.Count);
                } while (position == currentWordIndex);
                currentWordIndex = position;

                Debug.WriteLine(string.Format("WordsProvider, trying to take {0} position", position));
                return currentPart[position];
            }
            else
            {
                Debug.WriteLine("WordsProvider, return empty word");
                return new Word();
            }
        }

        public void PartNext()
        {
            Debug.WriteLine("WordsProvider, partNext");
            currentPartIndex++;
            if (currentPartIndex >= dictionaryPartsCount) currentPartIndex = 0;
            currentPart = Repository.GetPart(PART_SIZE, currentPartIndex * PART_SIZE);
            currentWordIndex = -1;
        }

        public void PartPrev()
        {
            Debug.WriteLine("WordsProvider, partPrev");
            currentPartIndex--;
            if (currentPartIndex < 0) currentPartIndex = dictionaryPartsCount - 1;
            currentPart = Repository.GetPart(PART_SIZE, currentPartIndex * PART_SIZE);
            currentWordIndex = -1;
        }

        public int GetPartIndex()
        {
            return currentPartIndex;
        }
        public int GetPartsCount()
        {
            return dictionaryPartsCount;
        }
        public Word Current()
        {
            Debug.WriteLine("WordsProvider, current");
            if (currentPart != null && currentPart.Count > 0)
                return currentPart[currentWordIndex != -1 ? currentWordIndex : 0];
            else
                return new Word();
        }

        public ObservableCollection<Word> GetAllObservableCollection()
        {
            Debug.WriteLine("WordsProvider, get all observable collection");
            ObservableCollection<Word> collection = new ObservableCollection<Word>(Repository.GetItems());
            return collection;
        }
        public ObservableCollection<Word> GetFilteredObservableCollection(string filter, string prop)
        {
            Debug.WriteLine("WordsProvider, get filtered observable collection");
            WordRepository.WordPropertyName propName = WordRepository.WordPropertyName.Eng;
            if (prop.Equals("Rus")) propName = WordRepository.WordPropertyName.Rus;
            
            ObservableCollection<Word> collection;
            if (string.IsNullOrEmpty(filter))
            {
                collection = GetAllObservableCollection();
            }
            else
            {
                if (chachedWords != null && chachedWords.Count != 0 && chachedWordsChar == filter[0])
                {
                    collection = new ObservableCollection<Word>(ApplyFilter(chachedWords, filter, propName));
                }
                else
                {
                    chachedWords = Repository.GetWordStartWith(filter.Substring(0, 1), propName);
                    if (chachedWords.Count > 0) chachedWordsChar = filter[0];
                    if (filter.Length > 1)
                        collection = new ObservableCollection<Word>(ApplyFilter(chachedWords, filter, propName));
                    else
                        collection = new ObservableCollection<Word>(chachedWords);
                }
            }
            return collection;
        }

        public ObservableCollection<Word> GetFilteredObservableCollectionByInput(string rusInput, string engInput)
        {
            ObservableCollection<Word> collection = new ObservableCollection<Word>();
            IList<Word> words = Repository.GetItems();
            foreach(Word word in words)
            {
                if(word.Rus.ToLower().StartsWith(rusInput.ToLower()) &&
                    word.Eng.ToLower().StartsWith(engInput.ToLower()))
                {
                    collection.Add(word);
                }
            }
            return collection;
        }

        private IList<Word> ApplyFilter(IList<Word> words, string filter, WordRepository.WordPropertyName prop)
        {
            IList<Word> filtered = new List<Word>();
            foreach(var word in words)
            {
                string comparingWordString;
                switch (prop)
                {
                    case WordRepository.WordPropertyName.Rus:
                        comparingWordString = word.Rus;
                        break;
                    case WordRepository.WordPropertyName.Eng:
                        comparingWordString = word.Eng;
                        break;
                    default:
                        comparingWordString = word.Description;
                        break;
                }
                if(comparingWordString.StartsWith(filter))
                {
                    filtered.Add(word);
                } 
            }
            return filtered;
        }
    }
}
