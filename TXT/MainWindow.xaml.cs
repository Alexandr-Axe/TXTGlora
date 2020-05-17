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
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool Ulozeno = true;
        public static RichTextBox TextMainWindow;
        public static string TextRTB = string.Empty;
        public static string NewRTB = string.Empty;
        OpenFileDialog ofd = new OpenFileDialog() { Filter = "Textový dokument Glora (*.gte)|*.gte|Poznámkový blok (*.txt)|*.txt|Microsoft Word (*.docx)|*.docx|Všechny soubory (*.*)|*.*" };
        SaveFileDialog sfd = new SaveFileDialog() { Filter = "Textový dokument Glora (*.gte)|*.gte|Poznámkový blok (*.txt)|*.txt|Microsoft Word (*.docx)|*.docx|Všechny soubory (*.*)|*.*" };
        NajitText NT = new NajitText();
        MoveToLine MTL = new MoveToLine();

        string AdresaSouboru = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            //Title = $"{(Title == "" ? "Nový textový dokument" : JmenoSouboru)}.gte - Textový dokument";
            foreach (FontFamily item in Fonts.SystemFontFamilies)
            {
                kombik.Items.Add(item.Source.ToString());
            }
            TextMainWindow = TextSouboru;
            TextMainWindow.Document.PageWidth = 2000;
        }

        private void MenuItem_Soubor_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Pracujici = (MenuItem)sender;
            string[] JmenoSouboru;
            char lomitko = Convert.ToChar(92);
            if (Pracujici.Header.ToString() == "Ukončit") Close();
            else if (Pracujici.Header.ToString() == "Nový")
            {
                if (Ulozeno)
                {
                    TextMainWindow = RTB.SetText(TextMainWindow, "");
                    Title = "Nový soubor";
                }
                else
                {
                    MessageBoxResult mbr = MessageBox.Show("Tento soubor nebyl uložen! Přejete si ho uložit?", "Upozornění", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        sfd.ShowDialog();
                        try
                        {
                            File.WriteAllText(sfd.FileName, RTB.GetText(TextSouboru));
                            Ulozeno = true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        TextMainWindow = RTB.SetText(TextMainWindow, "");
                        Title = "Nový soubor";
                        Ulozeno = true;
                    }
                }
            }
            else if (Pracujici.Header.ToString() == "Otevřít...")
            {
                ofd.ShowDialog();
                AdresaSouboru = $"{ofd.InitialDirectory}{ofd.FileName}";
                if (File.Exists(AdresaSouboru))
                {
                    TextMainWindow = RTB.SetText(TextMainWindow, File.ReadAllText(AdresaSouboru, Encoding.UTF8));
                    JmenoSouboru = ofd.FileName.Split(lomitko);
                    Title = JmenoSouboru[JmenoSouboru.Length - 1];
                }
            }
            else if (Pracujici.Header.ToString() == "Uložit")
            {
                sfd.ShowDialog();
                try
                {
                    File.WriteAllText(sfd.FileName, RTB.GetText(TextMainWindow));
                }
                catch (Exception)
                {
                }
            }
            else if (Pracujici.Header.ToString() == "Tisk...")
            {
                PrintDialog PD = new PrintDialog();
                if ((bool)PD.ShowDialog().GetValueOrDefault())
                {
                    FlowDocument FD = new FlowDocument();
                    foreach (string Odstavec in RTB.GetText(TextMainWindow).Split('\n'))
                    {
                        Paragraph Paragraf = new Paragraph();
                        Paragraf.Margin = new Thickness(0);
                        Paragraf.Inlines.Add(new Run(Odstavec));
                        FD.Blocks.Add(Paragraf);
                    }
                    DocumentPaginator DP = ((IDocumentPaginatorSource)FD).DocumentPaginator;
                    PD.PrintDocument(DP, Title);
                }
            }
        }
        private void MenuItem_Upravy_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Pracujici = (MenuItem)sender;
            if (Pracujici.Header.ToString() == "Najít...")
            {
                NT.replacetext.Visibility = Visibility.Hidden;
                NT.replacebutton.Visibility = Visibility.Hidden;
                NT.nahradit.Visibility = Visibility.Hidden;
                NT.findbutton.Margin = new Thickness(0, 4, 33, 15);
                NT.Height = 85;
                NT.Show();
            }
            else if (Pracujici.Header.ToString() == "Nahradit...")
            {
                NT.replacetext.Visibility = Visibility.Visible;
                NT.replacebutton.Visibility = Visibility.Visible;
                NT.nahradit.Visibility = Visibility.Visible;
                NT.findbutton.Margin = new Thickness(0, 4, 33, 114);
                NT.Height = 180;
                NT.Show();
            }
            else if (Pracujici.Header.ToString() == "Přejít...") MTL.Show();
        }
        private void Cas(object sender, RoutedEventArgs e)
        {
            TextMainWindow = RTB.GetTime(TextMainWindow);
            NewRTB = RTB.GetText(TextMainWindow);
        }

        private void TextSouboru_TextChanged(object sender, TextChangedEventArgs e)
        {
            Ulozeno = false;
            TextSouboru = TextMainWindow;
            TextRTB = RTB.GetText(TextMainWindow);
        }

        private void MenuItem_Zobrazeni_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Pracujici = (MenuItem)sender;
            if (Pracujici.Header.ToString() == "+")
            {
                if (TextMainWindow.FontSize < 50) TextMainWindow.FontSize += 5;
            }
                else if (Pracujici.Header.ToString() == "-")
            {
                if (TextMainWindow.FontSize > 10) TextMainWindow.FontSize -= 5;
            }
                else if (Pracujici.Header.ToString() == "Obnovit výchozí") TextMainWindow.FontSize = 13;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                sfd.ShowDialog();
                try
                {
                    File.WriteAllText(sfd.FileName, RTB.GetText(TextMainWindow));
                }
                catch (Exception)
                {
                }
            }
            else if(e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                NT.replacetext.Visibility = Visibility.Hidden;
                NT.replacebutton.Visibility = Visibility.Hidden;
                NT.nahradit.Visibility = Visibility.Hidden;
                NT.findbutton.Margin = new Thickness(0, 4, 33, 15);
                NT.Height = 85;
                NT.Show();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (File.Exists(AdresaSouboru))
            {
                string text = File.ReadAllText(AdresaSouboru, Encoding.UTF8);
                if (text != RTB.GetText(TextMainWindow))
                {
                    MessageBoxResult Vysledek = MessageBox.Show("Soubor byl změněn! Přejete si ho uložit?", "Neuložený soubor", MessageBoxButton.YesNo);
                    switch (Vysledek)
                    {
                        case MessageBoxResult.Yes:
                            try
                            {
                                File.WriteAllText(AdresaSouboru, RTB.GetText(TextMainWindow));
                            }
                            catch (Exception)
                            {
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
            else
            {
                MessageBoxResult Vysledek = MessageBox.Show("Přejete si soubor uložit?", "Neuložený soubor", MessageBoxButton.YesNo);
                switch (Vysledek)
                {
                    case MessageBoxResult.Yes:
                        sfd.ShowDialog();
                        try
                        {
                            File.WriteAllText(sfd.FileName, RTB.GetText(TextMainWindow));
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case MessageBoxResult.No:
                        Application.Current.Shutdown();
                        break;
                }
            }
        }
    }
    public class IsText
    {
        public string ProhlizetText { get; set; }
        public bool ProhlizeTextIsTrueFalse(string p)
        {
            if (p == null) return false;
            return true;
        }
        public bool TextNalezenTrueFalse(string a, string b)
        {
            if (a == b) return true;
            return false;
        }
        public bool CasPridanTrueFalse(string a, string b)
        {
            if (a != b) return true;
            return false;
        }
    }
}