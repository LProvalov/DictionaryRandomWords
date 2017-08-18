using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WordProvider wordProvider = new WordProvider(string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), "Dictionary.txt"));
        private Brush defaultColorBrush;
        private int enterPressedCount = 0;
        private bool engToRus = true;
        public MainWindow()
        {
            InitializeComponent();
            defaultColorBrush = this.wordTextBox.Background;
            nextWord();
        }

        private void wordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                if (enterPressedCount == 0)
                {
                    checkWord();   
                }
                else
                {
                    nextWord();
                }

                enterPressedCount++;
                if (enterPressedCount == 2) enterPressedCount = 0;
            }
        }

        private void nextWord()
        {
            WordWithTranslation wwt = wordProvider.GetNextRandom();
            this.wordLable.Content = this.engToRus ? wwt.Eng : wwt.Rus;
            this.wordTextBox.Text = "";
            this.wordTextBox.Background = defaultColorBrush;
        }

        private void checkWord()
        {
            string inputValue = this.wordTextBox.Text;
            var wwt = wordProvider.Current();
            if (inputValue != (this.engToRus ? wwt.Rus : wwt.Eng))
            {
                this.wordTextBox.Background = Brushes.Red;
            }
            else
            {
                this.wordTextBox.Background = Brushes.Green;
            }
            this.wordLable.Content = string.Format("{0} - {1}", this.wordLable.Content,
                    this.engToRus ? wwt.Rus : wwt.Eng);
        }

        private void dirCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            switch(cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "Rus to Eng":
                    engToRus = false;
                    break;
                case "Eng to Rus":
                    engToRus = true;
                    break;
            }
            nextWord();
        }

        private void decreasePartButton_Click(object sender, RoutedEventArgs e)
        {
            this.wordProvider.PartPrev();
            this.partLable.Content = wordProvider.GetPartIndex().ToString();
            nextWord();
        }

        private void increasePartButton_Click(object sender, RoutedEventArgs e)
        {
            this.wordProvider.PartNext();
            this.partLable.Content = wordProvider.GetPartIndex().ToString();
            nextWord();
        }
    }
}
