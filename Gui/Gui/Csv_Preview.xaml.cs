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
using System.Windows.Shapes;
using Core;

namespace Gui
{
    /// <summary>
    /// Interaction logic for Csv_Preview.xaml
    /// </summary>
    public partial class Csv_Preview : Window
    {
        public Csv_Preview()
        {
            InitializeComponent();
            wpfComboBox.ItemsSource = Controller.Data.GetHeaders();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            wpfButton.Visibility = Visibility.Visible;
        }

        private void wpfButton_Click(object sender, RoutedEventArgs e)
        {
            Controller.Data.FindColumnNo(wpfComboBox.Text);
            Controller.Data.CreateCollection();
            this.Close();
        }
    }
}
