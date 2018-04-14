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
    /// Interaction logic for ResultViewPage.xaml
    /// </summary>
    public partial class ResultViewPage : Page
    {
        public ResultViewPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {

            await Task.Run(() =>
            {
                RealImage.Dispatcher.Invoke(() =>
                {
                    RealImage.Source = App.CurrentApp.ContainerImage.ToBitmapSource();
                });
                SecretImage.Dispatcher.Invoke(() =>
                {
                    SecretImage.Source = App.CurrentApp.ResultImage.ToBitmapSource();
                });
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Title = "Save Image"
            };

            if(saveFileDialog.ShowDialog() == true)
                App.CurrentApp.ResultImage.Save(saveFileDialog.FileName);
        }
    }
}
