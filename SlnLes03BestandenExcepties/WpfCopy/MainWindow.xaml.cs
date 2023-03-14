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

namespace WpfCopy
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
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                TbxLocatie.Text = folderPath;
            }
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            dialog.FileName = "savedfile.txt";
            if (dialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(dialog.FileName))
                {
                    writer.Write(TbxLocatie.Text);
                    writer.Write(TbxLocatie.Text);
                    BtnGo.IsEnabled = false;
                    lblovergezet.Visibility = Visibility.Visible;
                }
            }
          
        }
    }
}
