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
    /// Interaction logic for EmbedPage.xaml
    /// </summary>
    public partial class EmbedPage : Page
    {
        public EmbedPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var source = App.CurrentApp.ContainerImage;
                var destination = App.CurrentApp.FiremarkImage;

                App.CurrentApp.ResultImage = source.Embed(destination);
            });

            App.CastedMainWindow().MainFrame.Navigate(new ResultViewPage());
        }
    }
}
