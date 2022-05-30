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
            Controller.OnNewPicturesAvailable += RefreshPictures;
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

        private void RefreshPictures(object sender, EventArgs e)
        {
            //TODO: Refresh Pictures
            PicOne.Source = convertToImage(Controller.PictureData[0]);
            PicTwo.Source = convertToImage(Controller.PictureData[1]);
            PicThree.Source = convertToImage(Controller.PictureData[2]);
            PicFour.Source = convertToImage(Controller.PictureData[3]);
            PicFive.Source = convertToImage(Controller.PictureData[4]);
            PicSix.Source = convertToImage(Controller.PictureData[5]);
        }

        private ImageSource convertToImage(byte[] picture)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(picture);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }
    }
}
