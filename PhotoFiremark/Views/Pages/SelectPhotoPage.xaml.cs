using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
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
using Emgu.CV.WPF;

namespace PhotoFiremark.Views.Pages
{
    /// <summary>
    /// Interaction logic for SelectPhotoPage.xaml
    /// </summary>
    public partial class SelectPhotoPage : Page
    {
        public SelectPhotoPage()
        {
            InitializeComponent();
        }

        bool initialImageLoad = true;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Jpeg Photos|*.jpg;*.jpeg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Multiselect = false,
                Title = "Open Image",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var urlString = string.Empty;
            if(openFileDialog.ShowDialog() == true)
            {
                urlString = openFileDialog.FileName;

                SelectPhotoButton.IsEnabled = false;
                NextPageButton.IsEnabled = false;

                if(initialImageLoad)
                    ThumbText.Visibility = Visibility.Collapsed;
                else
                    await ImagePreview.HideUsingLinearAnimationAsync(milliSeconds: 250);

                await ThumbLoading.ShowUsingLinearAnimationAsync(milliSeconds: 250);

                await ProcessImageAsync(urlString);

                await ThumbLoading.HideUsingLinearAnimationAsync(milliSeconds: 250);
                await ImagePreview.ShowUsingLinearAnimationAsync(milliSeconds: 250);

                SelectPhotoButton.IsEnabled = true;
                NextPageButton.IsEnabled = true;

                if (initialImageLoad)
                {
                    initialImageLoad = false;
                    NextPageButton.ShowUsingLinearAnimation(milliSeconds: 250);
                }

            }

        }

        public async Task ProcessImageAsync(string url)
        {
            await Task.Run(() =>
            {
                var myImage = new Image<Rgb, byte>(url);
                myImage = myImage.Resize(1024, 1024, Emgu.CV.CvEnum.Inter.Nearest);

                App.CurrentApp.ContainerImage = myImage.Copy();

                ImagePreview.Dispatcher.Invoke(() => { ImagePreview.Source = myImage.ToBitmapSource(); });
            });
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            App.CastedMainWindow().MainFrame.Navigate(new SelectFiremarkPage());
        }
    }
}
