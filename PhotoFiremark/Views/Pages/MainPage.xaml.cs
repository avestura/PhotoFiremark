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

        private bool firstTimeInit = true;

        private void GoToEmbed_Click(object sender, RoutedEventArgs e)
        {
            App.CastedMainWindow().MainFrame.Navigate(new SelectPhotoPage());
        }

        private void GoToReveal_Click(object sender, RoutedEventArgs e)
        {
            App.CastedMainWindow().MainFrame.Navigate(new RevealPhoto());
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MethodSelect_Combo.SelectedIndex == 0)
            {
                App.CurrentApp.Configuration.Domain = App.FiremarkDomain.Time;

                if (!firstTimeInit)
                {
                    await WaveletAlgorithmSelect_Region.HideUsingLinearAnimationAsync(500);
                }

                await TimeDomainSignal_Alert.ShowUsingLinearAnimationAsync(500);
            }
            else
            {
                App.CurrentApp.Configuration.Domain = App.FiremarkDomain.Frequesny;

                if (!firstTimeInit)
                {
                    await TimeDomainSignal_Alert.HideUsingLinearAnimationAsync(500);
                }
                await WaveletAlgorithmSelect_Region.ShowUsingLinearAnimationAsync(500);
            }

            App.CurrentApp.Configuration.SaveSettingsToFile();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.CurrentApp.Configuration.Domain == App.FiremarkDomain.Time)
                MethodSelect_Combo.SelectedIndex = 0;
            else
                MethodSelect_Combo.SelectedIndex = 1;

            firstTimeInit = false;
        }
    }
}
