using System;
using System.Collections.Generic;
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
using System.Drawing;

using System.IO;
using Microsoft.Win32;

namespace TXT
{
    /// <summary>
    /// Interakční logika pro NajitText.xaml
    /// </summary>
    public partial class NajitText : Window
    {
        public NajitText()
        {
            InitializeComponent();
            findtext.Focus();
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            Button Tenhle = (Button)sender;
            if (Tenhle.Content.ToString() == "Najít")
            {
                if (RTB.GetText(MainWindow.TextMainWindow) != "")
                {
                    //MainWindow.TextMainWindow.Document = RTB.SearchText(MainWindow.TextMainWindow, findtext.Text);
                    MainWindow.TextMainWindow.Document = RTB.Experiment(MainWindow.TextMainWindow, findtext.Text);
                    Close();
                }
                else MessageBox.Show($"Textové pole je prázdné!", "Text nebyl nalezen", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else MainWindow.TextMainWindow.Document = RTB.ReplaceWord(MainWindow.TextMainWindow, findtext.Text, replacetext.Text);
        }
    }
}