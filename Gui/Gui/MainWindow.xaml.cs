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
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using Core;

namespace Gui
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

        private void OpenCsv_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //TODO: GUI: Prompt for Delmiter? Or put something in Settings?
                Controller.LoadCsv(openFileDialog.FileName, ',');
                Csv_Preview csvPreview = new Csv_Preview();
                csvPreview.ShowDialog();
            }
            else //User hit cancel
            {
                return;
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            // This doesn't work
            Debug.Write("Settings clicked"); 
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // This DOES work - The only difference is I used the auto complete so it generated the function automatically.
        }
    }
}
