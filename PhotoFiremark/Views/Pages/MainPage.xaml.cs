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
using PhotoFiremark.Utilities;

namespace PhotoFiremark.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

        }

        private void GoToEmbed_Click(object sender, RoutedEventArgs e)
        {
            App.CastedMainWindow().MainFrame.Navigate(new SelectPhotoPage());
        }

        private void GoToReveal_Click(object sender, RoutedEventArgs e)
        {
            App.CastedMainWindow().MainFrame.Navigate(new RevealPhoto());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MethodSelect_Combo.SelectedIndex == 0)
            {
                WaveletAlgorithmSelect_Region.HideUsingLinearAnimation(100);
            }
            else
            {
                WaveletAlgorithmSelect_Region.ShowUsingLinearAnimation(100);
            }
        }
    }
}
