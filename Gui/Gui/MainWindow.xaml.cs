using Core;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            if (Controller.NewInstall)
            {
                Settings_Click(this, null);
            }

            Controller.OnNewPicturesAvailable += RefreshPictures;
            Controller.CallingApi += HideButtons;
        }

        private void HideButtons(object? sender, EventArgs e)
        {
            btnOne.Visibility = Visibility.Hidden;
            btnTwo.Visibility = Visibility.Hidden;
            btnThree.Visibility = Visibility.Hidden;
            btnFour.Visibility = Visibility.Hidden;
            btnFive.Visibility = Visibility.Hidden;
            btnSix.Visibility = Visibility.Hidden;
        }

        private void ShowButtons()
        {
            btnOne.Visibility = Visibility.Visible;
            btnTwo.Visibility = Visibility.Visible;
            btnThree.Visibility = Visibility.Visible;
            btnFour.Visibility = Visibility.Visible;
            btnFive.Visibility = Visibility.Visible;
            btnSix.Visibility = Visibility.Visible;
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
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // This DOES work - The only difference is I used the auto complete so it generated the function automatically.
        }

        private void RefreshPictures(object sender, EventArgs e)
        {
            //TODO: RefreshPictures(): What do we do if there isn't 6 pictures?
            PicOne.Source = convertToImage(Controller.PictureData[0]);
            PicTwo.Source = convertToImage(Controller.PictureData[1]);
            PicThree.Source = convertToImage(Controller.PictureData[2]);
            PicFour.Source = convertToImage(Controller.PictureData[3]);
            PicFive.Source = convertToImage(Controller.PictureData[4]);
            PicSix.Source = convertToImage(Controller.PictureData[5]);

            ShowButtons();

            if (textBlockKeyWord.Visibility != Visibility.Visible)
            {
                textBlockKeyWord.Visibility = Visibility.Visible;
            }
            textBlockKeyWord.Text = Controller.Data.KeywordsToDo.Peek();
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

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            System.Diagnostics.Debug.WriteLine($"Button {button.Name} was clicked!");

            byte[] bytes = null;
            string fileLocation = null;

            switch (button.Name)
            {
                case "btnOne":
                    bytes = ImageSourceToBytes(PicOne.Source);
                    fileLocation = Controller.SavePic(textBlockKeyWord.Text, bytes);
                    break;
                case "btnTwo":
                    bytes = ImageSourceToBytes(PicTwo.Source);
                    fileLocation = Controller.SavePic(textBlockKeyWord.Text, bytes);
                    break;
                case "btnThree":
                    bytes = ImageSourceToBytes(PicThree.Source);
                    fileLocation = Controller.SavePic(textBlockKeyWord.Text, bytes);
                    break;
                case "btnFour":
                    bytes = ImageSourceToBytes(PicFour.Source);
                    fileLocation = Controller.SavePic(textBlockKeyWord.Text, bytes);
                    break;
                case "btnFive":
                    bytes = ImageSourceToBytes(PicFive.Source);
                    fileLocation = Controller.SavePic(textBlockKeyWord.Text, bytes);
                    break;
                case "btnSix":
                    bytes = ImageSourceToBytes(PicSix.Source);
                    fileLocation = Controller.SavePic(textBlockKeyWord.Text, bytes);
                    break;
                default:
                    break;
            }

            Controller.Data.AddColumnValue("Picture", textBlockKeyWord.Text, fileLocation);

            var finishedKeyword = Controller.Data.KeywordsToDo.Pop();
            Controller.Data.KeywordsCompleted.Push(finishedKeyword);

            if (Controller.Data.KeywordsToDo.Count != 0)
            {
                Controller.NextPics(this, EventArgs.Empty);
            }
            else
            {
                var SaveLocation = (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                Controller.Data.Save(SaveLocation + "\\Anki.csv");
                ShowComplete(SaveLocation);
            }

            //TODO: START HERE
            //We need to:
            //Move to the next picture
            //Once this is working we should hide the buttons at load up and hide them if a picture doesn't load
        }

        private void ShowComplete(string saveLocation)
        {
            PicOne.Visibility = Visibility.Hidden;
            PicTwo.Visibility = Visibility.Hidden;
            PicThree.Visibility = Visibility.Hidden;
            PicFour.Visibility = Visibility.Hidden;
            PicFive.Visibility = Visibility.Hidden;
            PicSix.Visibility = Visibility.Hidden;

            btnOne.Visibility = Visibility.Hidden;
            btnTwo.Visibility = Visibility.Hidden;
            btnThree.Visibility = Visibility.Hidden;
            btnFour.Visibility = Visibility.Hidden;
            btnFive.Visibility = Visibility.Hidden;
            btnSix.Visibility = Visibility.Hidden;

            textBlockKeyWord.Visibility = Visibility.Hidden;

            CompletedText.Visibility = Visibility.Visible;
            CompletedText.Text = $"Complete! File saved to {saveLocation}";
        }

        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }
        private void ErrorMessage(object obj, EventArgs e)
        {
            var message = (Exception)obj;
            MessageBox.Show(message.Message);
        }
    }
}
