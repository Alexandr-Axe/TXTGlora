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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace TXT
{
    /// <summary>
    /// Interakční logika pro MoveToLine.xaml
    /// </summary>
    public partial class MoveToLine : Window
    {
        public MoveToLine()
        {
            InitializeComponent();
            linenumber.Focus();
        }

        private void Movetobutton_Click(object sender, RoutedEventArgs e)
        {
            Button Pracujici = (Button)sender;
            if (Pracujici.Name == "Přejít")
            {
                string[] TR = new TextRange(MainWindow.TextMainWindow.Document.ContentStart, MainWindow.TextMainWindow.Document.ContentEnd).Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                for (int i = 0; i < TR.Length; i++)
                {

                }
            }
            Close();
        }

        private void Linebox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
