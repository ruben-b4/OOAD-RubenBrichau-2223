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
        static bool istoevoegengeklikt = false;
        static Stack<ListBoxItem> verwijderdeitems = new Stack<ListBoxItem>();

        public MainWindow()
        {
            InitializeComponent();
            CbbPrioriteit.SelectedIndex = 0;
            tbkFoutmeldingen.Text = string.Empty;
        }

        private List<string> CheckForm()
        {
            List<string> errors = new List<string>();
            if (!istoevoegengeklikt) return errors;

            // reset
            txtTaak.Text = string.Empty;
            CbbPrioriteit.SelectedIndex = 0;
            DateDeadline.SelectedDate = null;
            RbdAdam.IsChecked = false;
            RbdBilal.IsChecked = false;
            RbnChelsey.IsChecked = false;
            tbkFoutmeldingen.Text = string.Empty;
            txtTaak.BorderBrush = Brushes.LightGray;
            CbbPrioriteit.BorderBrush = Brushes.LightGray;
            DateDeadline.BorderBrush = Brushes.LightGray;

            // form controleren
            if (txtTaak.Text == string.Empty)
            {
                errors.Add($"Gelieve een taak toe te voegen {Environment.NewLine}");
                txtTaak.BorderBrush = Brushes.Red;
            }

            if (CbbPrioriteit.SelectedIndex == 0)
            {
                errors.Add(tbkFoutmeldingen.Text += $"Gelieve een prioriteit toe te voegen {Environment.NewLine}");
                CbbPrioriteit.BorderBrush = Brushes.Red;
            }

            if (DateDeadline.SelectedDate == null)
            {
                errors.Add($"gelieve een deadline te kiezen {Environment.NewLine}");
                DateDeadline.BorderBrush = Brushes.Red;
            }

            if (RbdAdam.IsChecked == false && RbdBilal.IsChecked == false && RbnChelsey.IsChecked == false)
            {
                errors.Add($"Gelieve een uitvoerder te kiezen{Environment.NewLine}");
                RbdAdam.BorderBrush = Brushes.Red;
                RbnChelsey.BorderBrush = Brushes.Red;
                RbdBilal.BorderBrush = Brushes.Red;
            }

            return errors;
        }

        private void BtnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            istoevoegengeklikt = true;
            tbkFoutmeldingen.Text = string.Empty;
            List<string> errors = CheckForm();
            tbkFoutmeldingen.Text = string.Join(Environment.NewLine, errors);

            if (errors.Count > 0) return;

            // Voeg taak toe
            ListBoxItem taak = new ListBoxItem();
            string taakuitvoerder = string.Empty;
            List<RadioButton> radioButtons = new List<RadioButton>() { RbdAdam, RbdBilal, RbnChelsey };
            taakuitvoerder = GetCheckedButton(radioButtons).Content.ToString();
            taak.Content = $"{txtTaak.Text} (Deadline: {DateDeadline.SelectedDate.Value.ToShortDateString()}; door: {taakuitvoerder})";
            List<SolidColorBrush> colors = new List<SolidColorBrush>() { Brushes.IndianRed, Brushes.LightGreen, Brushes.Beige };
            taak.Background = colors[CbbPrioriteit.SelectedIndex - 1];

            lsbTaken.Items.Add(taak);
        }

        private RadioButton GetCheckedButton(List<RadioButton> radioButtons)
        {
            foreach (RadioButton radioButton in radioButtons)
            {
                if (radioButton.IsChecked == true) return radioButton;
            }
            return null;
        }

        private void SetButtonStates()
        {
            BtnTerugzetten.IsEnabled = verwijderdeitems.Count == 0 ? false : true;
            BtnVerwijderen.IsEnabled = lsbTaken.SelectedIndex >= 0;
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            CheckForm();
        }

        private void LsbTaken_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetButtonStates();
        }

        private void BtnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            verwijderdeitems.Push((ListBoxItem)lsbTaken.SelectedItem);
            lsbTaken.Items.Remove(lsbTaken.SelectedItem);
            SetButtonStates();
        }

        private void BtnTerugzetten_Click(object sender, RoutedEventArgs e)
        {
            lsbTaken.Items.Add(verwijderdeitems.Pop());
            SetButtonStates();
        }
    }
}
