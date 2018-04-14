using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.WPF;
using Microsoft.Win32;
using PhotoFiremark.Utilities;
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

namespace PhotoFiremark.Views.Pages
{
    /// <summary>
    /// Interaction logic for RevealPhoto.xaml
    /// </summary>
    public partial class RevealPhoto : Page
    {
        public RevealPhoto()
        {
            InitializeComponent();
        }

        bool initialImageLoad = true;
        bool reveal = true;
        Image<Rgb, byte> Image = null;

        Image<Rgb, byte> RevealedImage = null;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PNG|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Multiselect = false,
                Title = "Open Image",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var urlString = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                RevealedImage = null;
                urlString = openFileDialog.FileName;

                SelectPhotoButton.IsEnabled = false;
                RevealButton.IsEnabled = false;

                if (initialImageLoad)
                    ThumbText.Visibility = Visibility.Collapsed;
                else
                    await ImagePreview.HideUsingLinearAnimationAsync();

                await ThumbLoading.ShowUsingLinearAnimationAsync();

                await ProcessImageAsync(urlString);

                await ThumbLoading.HideUsingLinearAnimationAsync();
                await ImagePreview.ShowUsingLinearAnimationAsync();

                SelectPhotoButton.IsEnabled = true;
                RevealButton.IsEnabled = true;

                if (initialImageLoad)
                {
                    initialImageLoad = false;
                    RevealButton.ShowUsingLinearAnimation();
                }

            }

        }

        public async Task ProcessImageAsync(string url)
        {
            await Task.Run(() =>
            {
                var newImage = new Image<Rgb, byte>(url);
                if (Image.Rows == 1000 && Image.Cols == 1000)
                {
                    Image = newImage;
                    ImagePreview.Dispatcher.Invoke(() => { ImagePreview.Source = Image.ToBitmapSource(); });
                }
                else
                    MessageBox.Show("Photo is not a secret one!");

            });
        }

        private void RevealButton_Click(object sender, RoutedEventArgs e)
        {
            if (reveal)
            {
                if(RevealedImage == null)
                {
                    RevealedImage = Image.ExtractSecretPhoto();
                }
                ImagePreview.Source = RevealedImage.ToBitmapSource();
                RevealButtonText.Text = "Original Image";
                RevealButtonIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Image;

                reveal = false;
            }
            else
            {
                ImagePreview.Source = Image.ToBitmapSource();
                RevealButtonText.Text = "Reveal";
                RevealButtonIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Eye;
                reveal = true;
            }
        }
    }
}
