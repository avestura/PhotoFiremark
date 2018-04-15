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

        /// <summary>
        /// If it is the first time loading image
        /// </summary>
        private bool InitialImageLoad { get; set; } = true;

        /// <summary>
        /// Checks wheter app should reveal photo or show the original image
        /// </summary>
        bool IsRevealState { get; set; } = true;

        /// <summary>
        /// Image loaded in UI
        /// </summary>
        private Image<Rgb, byte> Image { get; set; } = null;

        /// <summary>
        /// Revealed image holder
        /// </summary>
        private Image<Rgb, byte> RevealedImage { get; set; } = null;

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

                if (InitialImageLoad)
                    ThumbText.Visibility = Visibility.Collapsed;
                else
                    await ImagePreview.HideUsingLinearAnimationAsync();

                await ThumbLoading.ShowUsingLinearAnimationAsync();

                var availableToProcess = await ProcessImageAsync(urlString);

                await ThumbLoading.HideUsingLinearAnimationAsync();

                if (!availableToProcess)
                {
                    await ImagePreview.ShowUsingLinearAnimationAsync();
                    RevealButton.IsEnabled = true;
                }

                SelectPhotoButton.IsEnabled = true;

                if (InitialImageLoad) InitialImageLoad = false;
                if(!RevealButton.IsVisible && !availableToProcess)
                    RevealButton.ShowUsingLinearAnimation();

            }

        }

        /// <summary>
        /// Asynchronously process image and shows it in the UI
        /// </summary>
        /// <param name="url">Url of image</param>
        /// <returns>Returns true if any errors occurs</returns>
        public async Task<bool> ProcessImageAsync(string url)
        {
            return await Task.Run<bool>(() =>
            {
                try
                {
                    var newImage = new Image<Rgb, byte>(url);
                    if (newImage.Rows == 1000 && newImage.Cols == 1000)
                    {
                        Image = newImage.Copy();
                        ImagePreview.Dispatcher.Invoke(() => { ImagePreview.Source = Image.ToBitmapSource(); });
                        return false;
                    }
                    else {
                        MessageBox.Show("Photo is not a secret one!");
                    }
                }
                catch
                {
                    MessageBox.Show("Image could not be revealed!");
                }

                return true;

            });
        }

        private async void RevealButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRevealState)
            {
                if(RevealedImage == null)
                {
                    await ConstructText.MarginFadeInAnimationAsync(new Thickness(0, 10, 0, 0), new Thickness(0, 0, 0, 0));
                    RevealedImage = Image.ExtractSecretPhoto();
                    await ConstructText.MarginFadeOutAnimationAsync(new Thickness(0, 0, 0, 0), new Thickness(0, 10, 0, 0));
                }
                ImagePreview.Source = RevealedImage.ToBitmapSource();
                RevealButtonText.Text = "Original Image";
                RevealButtonIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Image;

                IsRevealState = false;
            }
            else
            {
                ImagePreview.Source = Image.ToBitmapSource();
                RevealButtonText.Text = "Reveal";
                RevealButtonIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Eye;
                IsRevealState = true;
            }
        }
    }
}
