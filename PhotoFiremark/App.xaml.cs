using Emgu.CV;
using Emgu.CV.Structure;
using PhotoFiremark.Views.Pages;
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

        /// <summary>
        /// Container (Large) image that gets value in <see cref="SelectPhotoPage"/>
        /// </summary>
        public Image<Rgb, byte> ContainerImage { get; set; }

        /// <summary>
        /// Secret image that gets value in <see cref="SelectFiremarkPage"/>
        /// </summary>
        public Image<Rgb, byte> FiremarkImage { get; set; }

        /// <summary>
        /// Result value of embeding secret photo in container photo
        /// </summary>
        public Image<Rgb, byte> ResultImage { get; set; }

    }
}
