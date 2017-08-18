using System;
using System.ComponentModel;

namespace RandomWordsApp
{
    public class WordWithTranslationModel
    {
        public WordWithTranslationModel()
        {
            Eng = string.Empty;
            Rus = string.Empty;
            Description = string.Empty;
            IsEngEntryEnabled = true;
            IsRusEntryEnabled = true;
            IsDescriptionEntryEnabled = true;
        }
        public string Eng { get; set; }
        public string Rus { get; set; }
        public string Description { get; set; }
        public bool IsEngEntryEnabled { get; set; }
        public bool IsRusEntryEnabled { get; set; }
        public bool IsDescriptionEntryEnabled { get; set; }
    }   
}
