using Emgu.CV.WPF;
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
    /// Interaction logic for WaveletLab.xaml
    /// </summary>
    public partial class WaveletLab : Page
    {
        public WaveletLab()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            LoadFiremarkImage();

            LoadContainerImage();

        }

        public void LoadFiremarkImage()
        {
            Task.Run(() =>
            {
                Firemark_ImagePreview.Dispatcher.Invoke(() =>
                {
                    Firemark_ImagePreview.Source = App.CurrentApp.FiremarkImage.ToBitmapSource();
                });
                Firemark_ThumbLoading.Dispatcher.Invoke(() =>
                {
                    Firemark_ThumbLoading.HideUsingLinearAnimation(milliSeconds: 250);
                });
                Firemark_ImagePreview.Dispatcher.Invoke(() =>
                {
                    Firemark_ImagePreview.ShowUsingLinearAnimation(milliSeconds: 250);
                });
            });
        }

        public void LoadContainerImage()
        {
            Task.Run(() =>
            {
                Container_ImagePreview.Dispatcher.Invoke(() =>
                {
                    Container_ImagePreview.Source = App.CurrentApp.ContainerImage.ToBitmapSource();
                });
                Container_ThumbLoading.Dispatcher.Invoke(() =>
                {
                    Container_ThumbLoading.HideUsingLinearAnimation(milliSeconds: 250);
                });
                Container_ThumbLoading.Dispatcher.Invoke(() =>
                {
                    Container_ImagePreview.ShowUsingLinearAnimation(milliSeconds: 250);
                });
            });
        }
    }

}
