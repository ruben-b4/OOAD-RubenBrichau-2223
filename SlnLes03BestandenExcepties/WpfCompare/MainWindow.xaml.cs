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
using Path = System.IO.Path;

namespace WpfCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string selectedFilePathlings;
        string selectedFilePathRechts;

        public MainWindow()
        {
            InitializeComponent();
            LoadFiles();
        }

        private void LoadFiles()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string startfolder = Path.Combine(folderPath, "WpfCompare");

            string[] files = Directory.GetFiles(startfolder, "*.txt");
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                lbxFilesLinks.Items.Add(fi.Name);
                lbxFilesRechts.Items.Add(fi.Name);
            }
        }

        private void LoadFileContent(string filePath, ListBox listBox)
        {
            listBox.Items.Clear();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox.Items.Add(line);
                }
            }
        }

        private void LbxFilesLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedFilePathlings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WpfCompare", lbxFilesLinks.SelectedItem.ToString());
            LoadFileContent(selectedFilePathlings, lbxContentLinks);
        }
        private void LbxFilesRechts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
               selectedFilePathRechts = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WpfCompare", lbxFilesRechts.SelectedItem.ToString());
               LoadFileContent(selectedFilePathRechts, lbxContentRechts);
        }
        private void BtnCompare_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lbxContentLinks.Items.Count; i++)
            {
                string leftLine = lbxContentLinks.Items[i].ToString();
                string rightLine = lbxContentRechts.Items[i].ToString();

                if (leftLine != rightLine)
                {
                    lbxContentRechts.Items[i] = new TextBlock() { Text = rightLine, Background = Brushes.IndianRed };
                }
            }
        }
    }
}
