using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Xml.Linq;
using Microsoft.Win32;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool unsavedChanges = false;
        public MainWindow()
        {
            InitializeComponent();

            tbxAchternaam.TextChanged += Card_Changed; // zal deze oproepen als er een wijziging is gebeurd
            tbxBedrijf.TextChanged += Card_Changed;
            tbxFacebook.TextChanged += Card_Changed;
            tbxInstagram.TextChanged += Card_Changed;
            tbxJobtitel.TextChanged += Card_Changed;
            tbxLinkedIn.TextChanged += Card_Changed;
            tbxPriveEmail.TextChanged += Card_Changed;
            tbxPriveTelefoon.TextChanged += Card_Changed;
            tbxTelefoon.TextChanged += Card_Changed;
            tbxVoornaam.TextChanged += Card_Changed;
            tbxWerkemail.TextChanged += Card_Changed;
            tbxYoutube.TextChanged += Card_Changed;
            rbnMan.Checked += Card_Changed;
            rbnOnbekend.Checked += Card_Changed;
            rbnVrouw.Checked += Card_Changed;
            datGeboorte.SelectedDateChanged += Card_Changed;
        }

        private void Card_Changed(object sender, EventArgs e)
        {
            unsavedChanges = true; // zet de vlag op true
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult afsluiten = MessageBox.Show("Ben je zeker dat je de applicatie wil afsluiten?", "Toepassing sluiten", MessageBoxButton.OKCancel);

            if (afsluiten == MessageBoxResult.OK)
            {
                Environment.Exit(0);
            }
            else if (afsluiten == MessageBoxResult.Cancel)
            {
                // niet sluiten
            }
        }

        private void MuAbout_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow1 popup = new PopupWindow1();
            popup.Show();
        }

        private void MniOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "vCard Files (*.vcf|*.vcf";

            string content;
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(folderPath, "vCard.vcf");

            txtKaart.Content = filePath;

            try
            {
                content = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException)
            { // file not found
                MessageBox.Show($"file {filePath} not found", "File not found", MessageBoxButton.OK);
            }
            catch (IOException)
            { // unable to open for reading
                MessageBox.Show($"Unable to open {filePath}", "Unable to open", MessageBoxButton.OK);
            }
            catch (Exception)
            { // use general Exception as fallback
                MessageBox.Show($"Unknown error reading {filePath}", "Unknown reading error", MessageBoxButton.OK);
            }

            bool? openFileDialogResult = openFileDialog.ShowDialog();
            if (openFileDialogResult == true)
            {
                SaveMenuItem.IsEnabled = true;
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                foreach (string line in lines)
                {
                    // bron https://learn.microsoft.com/en-us/dotnet/api/system.string.startswith?view=net-7.0
                    string[] volledigeNaam = line.Substring(line.LastIndexOf(":") + 1).Split(' '); // bron https://learn.microsoft.com/en-us/dotnet/api/system.string.lastindexof?view=net-8.0#system-string-lastindexof(system-string) en https://learn.microsoft.com/en-us/dotnet/api/system.string.split?view=net-7.0
                    string ingevuldItem = line.Substring(line.LastIndexOf(":") + 1);

                    if (line.StartsWith("FN;"))
                    {
                        tbxVoornaam.Text = volledigeNaam[0];
                        tbxAchternaam.Text = volledigeNaam[1];
                    }

                    // bron chat gpt
                    else if (line.StartsWith("PHOTO;ENCODING=b;TYPE=")) 
                    {
                        string base64String = line.Substring(line.IndexOf(":") + 1); // Haal de base64-gecodeerde string op uit de vCard-tekst.
                        byte[] imageBytes = Convert.FromBase64String(base64String); // Converteer de base64-gecodeerde string naar een byte-array.
                        MemoryStream ms = new MemoryStream(imageBytes); // Maak een MemoryStream-object aan en schrijf het byte-array erin.
                        BitmapImage bi = new BitmapImage(); // Maak een BitmapImage-object aan.
                        bi.BeginInit();
                        bi.StreamSource = ms; // Laad het MemoryStream-object in het BitmapImage-object.
                        bi.EndInit();
                        imgFoto.Source = bi; // Wijs het BitmapImage-object toe aan de Source-eigenschap van imgFoto.
                    }
                    else if (line.StartsWith("TEL;TYPE=HOME"))
                    {
                        tbxPriveTelefoon.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("TEL;TYPE=WORK"))
                    {
                        tbxTelefoon.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("BDAY:"))
                    {
                        string bday = ingevuldItem;
                        datGeboorte.SelectedDate = DateTime.ParseExact(bday, "yyyyMMdd", null);
                    }
                    else if (line.StartsWith("GENDER:"))
                    {
                        string gender = ingevuldItem;
                        if (gender == "M")
                        {
                            rbnMan.IsChecked = true;
                        }
                        else if (gender == "F")
                        {
                            rbnVrouw.IsChecked = true;
                        }
                        else
                        {
                            rbnOnbekend.IsChecked = true;
                        }
                    }
                    else if (line.StartsWith("EMAIL;CHARSET=UTF-8;type=HOME"))
                    {
                        tbxPriveEmail.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("ORG"))
                    {
                        tbxBedrijf.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("TITLE"))
                    {
                        tbxJobtitel.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("EMAIL;CHARSET=UTF-8;type=WORK"))
                    {
                        tbxWerkemail.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("X-SOCIALPROFILE;TYPE=facebook:"))
                    {
                        tbxFacebook.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("X-SOCIALPROFILE;TYPE=linkedin:"))
                    {
                        tbxLinkedIn.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("X-SOCIALPROFILE;TYPE=youtube:"))
                    {
                        tbxYoutube.Text = ingevuldItem;
                    }
                    else if (line.StartsWith("X-SOCIALPROFILE;TYPE=instagram:"))
                    {
                        tbxInstagram.Text = ingevuldItem;
                    }
                }
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bestand is opgeslagen", "Save");
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "vCard Files (*.vcf|*.vcf";
            saveFileDialog.FileName = "vCard.vcf";

            string gender = string.Empty;
            if (rbnVrouw.IsChecked == true)
            {
                gender = "F";
            }
            else if (rbnMan.IsChecked == true)
            {
                gender = "M";
            }
            else if (rbnOnbekend.IsChecked == true)
            {
                gender = "O";
            }

            if (saveFileDialog.ShowDialog() == true)
            {
                // open stream and start writing
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.WriteLine("BEGIN:VCARD");
                    writer.WriteLine("VERSION:3.0");
                    writer.WriteLine($"FN;CHARSET=UTF-8:{tbxVoornaam.Text} {tbxAchternaam.Text}");
                    writer.WriteLine($"N;CHARSET=UTF-8:{tbxAchternaam.Text};{tbxVoornaam.Text};;;");
                    writer.WriteLine($"NICKNAME;CHARSET=UTF-8:{tbxVoornaam.Text}");
                    writer.WriteLine($"GENDER:{gender}");
                    writer.WriteLine($"BDAY:{datGeboorte.SelectedDate?.ToString("yyyyMMdd")}");
                    writer.WriteLine($"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{tbxPriveEmail.Text}");
                    writer.WriteLine($"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{tbxWerkemail.Text}");
                    writer.WriteLine($"TEL;TYPE=HOME,VOICE:{tbxPriveTelefoon.Text}");
                    writer.WriteLine($"TEL;TYPE=WORK,VOICE:{tbxTelefoon.Text}");
                    writer.WriteLine($"ADR;CHARSET=UTF-8;TYPE=HOME:;;;;;;BelgiÃ«");
                    writer.WriteLine($"TITLE;CHARSET=UTF-8:{tbxJobtitel.Text}");
                    writer.WriteLine($"ORG;CHARSET=UTF-8:{tbxBedrijf.Text}");
                    writer.WriteLine($"X-SOCIALPROFILE;TYPE=facebook:{tbxFacebook.Text}");
                    writer.WriteLine($"X-SOCIALPROFILE;TYPE=linkedin:{tbxLinkedIn.Text}");
                    writer.WriteLine($"X-SOCIALPROFILE;TYPE=instagram:{tbxInstagram.Text}");
                    writer.WriteLine($"X-SOCIALPROFILE;TYPE=youtube:{tbxYoutube.Text}");
                    writer.WriteLine($"REV:{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");    // bron https://stackoverflow.com/questions/296920/how-do-you-get-the-current-time-of-day
                    
                    // bron grotendeels chat gpt
                    if (imgFoto.Source is BitmapSource bitmapSource)
                    {
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        using (MemoryStream ms = new MemoryStream())
                        {
                            encoder.Save(ms);
                            string base64String = Convert.ToBase64String(ms.ToArray());
                            writer.WriteLine($"PHOTO;ENCODING=b;TYPE=JPEG:{base64String}");
                        }
                    }
                    writer.WriteLine("END:VCARD");
                } // stream closes automatically
            }
        }

        private void MniNew_Click(object sender, RoutedEventArgs e)
        {
            if (unsavedChanges == true)
            {
                MessageBoxResult vraag = MessageBox.Show("Er zijn nog onopgeslagen wijzigingen, ben je zeker dat je wil verliezen?", "Wijzigingen !!", MessageBoxButton.YesNo);
                if (vraag == MessageBoxResult.Yes)
                {
                    unsavedChanges = false;
                }
                else if (vraag == MessageBoxResult.No)
                {
                    return;
                }

                tbxAchternaam.Text = "";
                tbxBedrijf.Text = "";
                tbxFacebook.Text = "";
                tbxInstagram.Text = "";
                tbxJobtitel.Text = "";
                tbxLinkedIn.Text = "";
                tbxPriveEmail.Text = "";
                tbxPriveTelefoon.Text = "";
                tbxTelefoon.Text = "";
                tbxVoornaam.Text = "";
                tbxWerkemail.Text = "";
                tbxYoutube.Text = "";

                unsavedChanges = false;
            }
        }

        private void BtnSelecteer_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filename = openFileDialog.FileName;
                txtSelectie.Content = filename;

                BitmapImage bitmap = new BitmapImage();   // leest geselecteerde bestand en zet het om naar een BitmapImage
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;     // bron chatgpt
                bitmap.UriSource = new Uri(filename);
                bitmap.EndInit();

                imgFoto.Source = bitmap; // koppelt BitmapImage aan Image control:
            }
        }
    }
}
