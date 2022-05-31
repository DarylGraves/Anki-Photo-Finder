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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            var availableApis = Enum.GetNames(typeof(Controller.apiTypes));

            if (Controller.ApiKey != null)
            {
                TxtApiKey.Text = Controller.ApiKey;
                ApiChoices.ItemsSource = availableApis;
                ApiChoices.SelectedItem = Controller.SelectedApiType.ToString();

            }
            else
            {
                ApiChoices.ItemsSource = availableApis;
                ApiChoices.SelectedItem = availableApis[0];
            }
        }

        private void Button_Click_Ok(object sender, RoutedEventArgs e)
        {
            // Add the Api Key and Enum choice to Controller
            // Save this to a file in AppData
            Controller.ApiKey = TxtApiKey.Text;
            Controller.UpdateApiType(TxtApiType.Text);
            Controller.SaveSettings();
            this.Close();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
