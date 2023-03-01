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

namespace WpfFileInfo
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

        private void BtnKies_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            if (dialog.ShowDialog() == true)
            {
                FileInfo fi = new FileInfo(dialog.FileName);
                string text = File.ReadAllText(dialog.FileName);

                string[] lines = text.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                int woordaantal = lines.Length;
                SortedDictionary<string, int> woordenNummers = new SortedDictionary<string, int>();
                foreach (string woorden in lines)
                {
                    if (woordenNummers.ContainsKey(woorden))
                    {
                        woordenNummers[woorden]++;
                    }

                    else
                    {
                        woordenNummers[woorden] = 1;
                    }
                            
                }

                lblInfo.Content = ($@"
                bestandsnaam: {fi.Name}
                extensie: {fi.Extension}
                gemaakt op: {fi.CreationTime.ToString()}
                mapnaam: {fi.Directory.Name}
                aantal woorden: {woordaantal}
                meest voorkomend: 
                ");
            }
        }
    }
}
