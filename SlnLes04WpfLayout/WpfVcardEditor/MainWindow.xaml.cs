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

namespace WpfVcardEditor
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
    }
}
