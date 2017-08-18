using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class WordWithTranslation
    {
        public WordWithTranslation(string eng, string rus, string description)
        {
            this.engWord = eng;
            this.rusWord = rus;
            this.description = description;
        }
        private string engWord;
        private string rusWord;
        private string description;
        public string Eng { get { return this.engWord; } }
        public string Rus { get { return this.rusWord; } }
        public string Description {  get { return this.description; } }
        
    }
}
