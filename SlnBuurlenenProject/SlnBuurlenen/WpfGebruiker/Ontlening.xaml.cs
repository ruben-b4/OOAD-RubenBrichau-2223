using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for Ontlening.xaml
    /// </summary>
    public partial class Ontlening : Page
    {
        private List<Voertuig> voertuigen;

        private List<MyClassLibrary.Ontlening> ontleningen;
        private Gebruiker currentUser;

        public Ontlening(Gebruiker user)
        {
            InitializeComponent();

            currentUser = user;
            ontleningen = MyClassLibrary.Ontlening.GetOntleningenByGebruikerId(currentUser.Id);

            UpdateMijnOntleningDisplay();
            UpdateMijnAanvragenDisplay();
        }

        private void UpdateMijnOntleningDisplay()
        {
            lbxOntleend.Items.Clear();
            List<MyClassLibrary.Ontlening> ontleningen = MyClassLibrary.Ontlening.GetAllOntleningen();

            ontleningen.Sort((o1, o2) => o2.Vanaf.CompareTo(o1.Vanaf)); // sort gevonden via chatgpt

            foreach (MyClassLibrary.Ontlening ontlening in ontleningen)
            {
                Voertuig voertuig = Voertuig.GetAllVoertuigen().FirstOrDefault(v => v.Id == ontlening.VoertuigId);
                if (ontlening.AanvragerId == currentUser.Id)
                {
                    string voertuigNaam = voertuig.Naam;
                    string eigenaarNaam = currentUser.GetEigenaarNaam(ontlening.AanvragerId);
                    {
                        string itemText = $"{voertuig.Naam} - {ontlening.Vanaf} tot {ontlening.Tot}";
                        ListBoxItem item = new ListBoxItem();
                        item.Content = itemText;
                        item.Tag = ontlening;

                        switch (ontlening.Status)
                        {
                            case MyClassLibrary.Status.InBehandeling:
                                item.Background = Brushes.Yellow;
                                break;
                            case MyClassLibrary.Status.Goedgekeurd:
                                item.Background = Brushes.LightSeaGreen;
                                break;
                            case MyClassLibrary.Status.Verworpen:
                                item.Background = Brushes.IndianRed;
                                break;
                            default:
                                item.Background = Brushes.Transparent;
                                break;
                        }
                        lbxOntleend.Items.Add(item);
                    }
                }
            }
        }
        private void UpdateMijnAanvragenDisplay()
        {
            lbxAanvragen.Items.Clear();
            voertuigen = Voertuig.GetAllVoertuigen();
            ontleningen.Sort((o1, o2) => o2.Vanaf.CompareTo(o1.Vanaf)); // sort gevonden via chatgpt

            foreach (MyClassLibrary.Ontlening ontlening in ontleningen)
            {
                Voertuig voertuig = null;

                foreach (Voertuig v in voertuigen)
                {
                    if (v.Id == ontlening.VoertuigId && v.EigenaarId != currentUser.Id)
                    {
                        voertuig = v;
                        break;
                    }
                }

                if (voertuig != null && ontlening.AanvragerId == currentUser.Id)
                {
                    string itemText = $"{voertuig.Naam} - {ontlening.Vanaf} tot {ontlening.Tot} door {currentUser.GetEigenaarNaam(voertuig.EigenaarId)}";
                    
                    ListBoxItem item = new ListBoxItem();
                    item.Content = itemText;
                    item.Tag = ontlening;

                    switch (ontlening.Status)
                    {
                        case MyClassLibrary.Status.InBehandeling:
                            item.Background = Brushes.Yellow;
                            break;
                        case MyClassLibrary.Status.Goedgekeurd:
                            item.Background = Brushes.LightSeaGreen;
                            break;
                        case MyClassLibrary.Status.Verworpen:
                            item.Background = Brushes.IndianRed;
                            break;
                        default:
                            item.Background = Brushes.Transparent;
                            break;
                    }

                    lbxAanvragen.Items.Add(item);
                }
            }
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)lbxOntleend.SelectedItem;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Ben je zeker dat je deze ontlening wil verwijderen uit de Database", "Ontlening verwijderen", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MyClassLibrary.Ontlening selectedOntlening = (MyClassLibrary.Ontlening)selectedItem.Tag;
                    if (selectedOntlening != null)
                    {
                        selectedOntlening.DeleteFromDB();
                        UpdateMijnOntleningDisplay();
                    }
                }
            }
        }

        private void LbxAanvragen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedItem = lbxAanvragen.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                MyClassLibrary.Ontlening ontlening = selectedItem.Tag as MyClassLibrary.Ontlening;
                if (ontlening != null)
                {
                    Voertuig voertuig = voertuigen.FirstOrDefault(v => v.Id == ontlening.VoertuigId);
                    
                    // bron https://www.c-sharpcorner.com/article/datetime-in-c-sharp/
                    if (ontlening.Vanaf > DateTime.Today) 
                    {
                        BtnAccepteren.IsEnabled = true;
                    }
                    else
                    {
                        BtnAccepteren.IsEnabled = false;
                    }

                    lblVoertuig.Content = $"Voertuig: {voertuig.Naam}";
                    lblPeriode.Content = $"Periode: van {ontlening.Vanaf} tot {ontlening.Tot}";
                    lblAanvrager.Content = $"Aanvrager: {currentUser.GetEigenaarNaam(voertuig.EigenaarId)}";
                    lblBericht.Content = $"Bericht: {ontlening.Bericht}";
                }
            }
        }

        private void BtnAccepteren_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = lbxAanvragen.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Ben je zeker dat je deze ontlening wil goedkeuren?", "Ontlening goedkeuren", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MyClassLibrary.Ontlening selectedOntlening = selectedItem.Tag as MyClassLibrary.Ontlening;
                    if (selectedOntlening != null)
                    {
                        selectedOntlening.Status = MyClassLibrary.Status.Goedgekeurd;
                        selectedOntlening.SaveToDB();

                        UpdateMijnOntleningDisplay();
                        UpdateMijnAanvragenDisplay();
                    }
                }
            }
        }

        private void BtnAfwijzen_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = lbxAanvragen.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Ben je zeker dat je deze ontlening wil Afwijzen?", "Ontlening afwijzen", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MyClassLibrary.Ontlening selectedOntlening = selectedItem.Tag as MyClassLibrary.Ontlening;
                    if (selectedOntlening != null)
                    {
                        selectedOntlening.Status = MyClassLibrary.Status.Verworpen;
                        selectedOntlening.SaveToDB();

                        UpdateMijnOntleningDisplay();
                        UpdateMijnAanvragenDisplay();
                    }
                }
            }
        }
    }
}

