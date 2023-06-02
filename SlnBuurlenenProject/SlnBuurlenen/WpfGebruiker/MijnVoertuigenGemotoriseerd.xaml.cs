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
using Microsoft.Win32;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for MijnVoertuigenGemotoriseerd.xaml
    /// </summary>
    public partial class MijnVoertuigenGemotoriseerd : Page
    {
        public MijnVoertuigenGemotoriseerd()
        {
            InitializeComponent();
        }

        private void BtnAfbeeldingen_Click(object sender, RoutedEventArgs e)
        {
            // Open een open-bestandsdialog om een afbeeldingsbestand te selecteren
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Afbeeldingsbestanden|*.jpg;*.jpeg;*.png;*.gif";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                // Verkrijg de geselecteerde bestanden
                string[] selectedFiles = openFileDialog.FileNames;

                // Beperk het aantal geselecteerde bestanden tot maximaal 3
                int maxImages = 3;
                if (selectedFiles.Length > maxImages)
                {
                    MessageBox.Show($"Je kunt maximaal {maxImages} afbeeldingen selecteren.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Maak een nieuwe horizontale StackPanel voor de afbeeldingen
                StackPanel imageStackPanel = new StackPanel();
                imageStackPanel.Orientation = Orientation.Horizontal;
                imageStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                imageStackPanel.Margin = new Thickness(0, 10, 0, 0);

                // Laad de geselecteerde afbeeldingen in de StackPanel
                for (int i = 0; i < selectedFiles.Length; i++)
                {
                    string imagePath = selectedFiles[i];
                    BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));

                    // Creëer een nieuwe Image-controle voor elke afbeelding
                    Image imageControl = new Image();
                    imageControl.Width = 200;
                    imageControl.Source = bitmapImage;

                    // Voeg de Image-controle toe aan de StackPanel
                    imageStackPanel.Children.Add(imageControl);
                }

                // Voeg de StackPanel met afbeeldingen toe aan de Grid
                Grid.SetColumn(imageStackPanel, 1);
                Grid.SetRow(imageStackPanel, 2);
                Grid.SetColumnSpan(imageStackPanel, 2);
                grid.Children.Add(imageStackPanel);
            }
        }

        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            // Verkrijg de invoerwaarden uit de tekstvakken en comboboxen
            string naam = tbxNaam.Text;
            string beschrijving = tbxBeschrijving.Text;
            string merk = tbxMerk.Text;
            int bouwjaar = int.Parse(tbxBouwjaar.Text);
            string model = tbxModel.Text;
            Brandstof brandstof = (Brandstof)cbxBrandstof.SelectedIndex;
            Transmissie transmissie = (Transmissie)cbxTransmissie.SelectedIndex;
            int eigenaarId = 1; // Vervang dit met de daadwerkelijke gebruiker-id

            // Maak een nieuw voertuigobject met de ingevoerde waarden
            Voertuig voertuig = new Voertuig
            {
                Naam = naam,
                Beschrijving = beschrijving,
                Merk = merk,
                Bouwjaar = bouwjaar,
                Brandstof = brandstof,
                Transmissie = transmissie,
                Model = model,
                EigenaarId = eigenaarId,
                Type = 2
            };

            int newId = voertuig.InsertToDB();


            // Voeg de afbeelding toe aan het voertuigobject (indien beschikbaar)
            if (imgVehicle.Source != null)
            {
                BitmapImage bitmapImage = (BitmapImage)imgVehicle.Source;
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder(); // Pas het juiste type encoder aan op basis van het afbeeldingsformaa, gebruik gemaa
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(stream);
                    voertuig.ImageData = stream.ToArray();
                }
            }
        }
    }
}
