using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoFiremark
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App CurrentApp => (App)Current;

        public static MainWindow CastedMainWindow() => CurrentApp.MainWindow as MainWindow;

        public Configuration Configuration { get; set; }

        public Image<Rgb, byte> ContainerImage { get; set; }

        public Image<Rgb, byte> FiremarkImage { get; set; }

        public Image<Rgb, byte> ResultImage { get; set; }

    }
}
