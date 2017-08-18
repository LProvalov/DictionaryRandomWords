using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    public class WordProvider
    {
        private const int PART_SIZE = 10;
        private IList<WordWithTranslation> words = new List<WordWithTranslation>();
        private int currentWordIndex = 0;
        private int currentPartIndex = 0;
        int dictLength = 0;

        public WordProvider(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                IEnumerable<string> lines = File.ReadLines(fileInfo.FullName, Encoding.Unicode);
                foreach(var line in lines)
                {
                    string[] split = line.Split('—');
                    string[] rusAndDescr = split.Last().Split('#');
                    words.Add(new WordWithTranslation(split.First().Trim(), rusAndDescr.First().Trim(), rusAndDescr.Length > 1 ? rusAndDescr.Last().Trim() : ""));
                }

                dictLength = words.Count / PART_SIZE;
                if (words.Count % PART_SIZE > 0) dictLength++;
            }
        }

        public WordWithTranslation GetNextRandom()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            int position = 0;
            do
            {
                position = rnd.Next(GetPartSize());
            } while (position == currentWordIndex);
            currentWordIndex = position;

            return words[currentPartIndex * 10 + position];
        }

        public void PartNext()
        {
            currentPartIndex++;
            if (currentPartIndex >= dictLength) currentPartIndex = 0;
        }

        public void PartPrev()
        {
            currentPartIndex--;
            if (currentPartIndex < 0) currentPartIndex = dictLength - 1;
        }

        public int GetPartIndex()
        {
            return currentPartIndex;
        }

        public WordWithTranslation Current()
        {
            return words[currentPartIndex * 10 + currentWordIndex];
        }

        public int GetPartSize()
        {
            if (currentPartIndex < dictLength - 1) return PART_SIZE;
            else return words.Count - (PART_SIZE * (dictLength - 1));
        }
    }
}
