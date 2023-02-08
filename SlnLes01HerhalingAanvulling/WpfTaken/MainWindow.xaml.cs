using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
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

namespace WpfTaken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnToevoegen_Click(object sender, RoutedEventArgs e)
        {
           

            if (txtTaak.Text == "")
            {
                lblTaakfout.Text = "Gelieve een taak in te vullen";
            }

            if (CBoxPrio.SelectedIndex == -1)
            {
                lblPrioriteit.Text = "Gelieve een prioriteit te kiezen";
            }

            if (DateDeadline.SelectedDate == null)
            {
                lblDeadlinefout.Text = "Gelieve een deadline te kiezen";
            }

            if (rbtn1.IsChecked != true && rbtn2.IsChecked != true && rbtn3.IsChecked != true)
            {
               lblUitvoerderfout.Text = "Gelieve een uitvoerder te kiezen";
            }

        }

        private void btnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            
            lstItems.Items.RemoveAt
            (lstItems.Items.IndexOf(lstItems.SelectedItem));
        }
    }
}
