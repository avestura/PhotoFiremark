using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static PhotoFiremark.App;

namespace PhotoFiremark
{
    public class Configuration
    {
        #region Config :: Fields :: Internal

        #endregion

        #region Config :: Fields :: Public Properties
        [XmlIgnore]
        public string IgnoredProperty { get; set; }
        public string StoredProperty { get; set; }

        public FiremarkDomain Domain { get; set; }
        #endregion

        #region Config :: Constants
        private const string fileName = "App.Config.Xml";
        private static string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//Aryan Software//PhotoFiremark//";
        private static string path = directory + fileName;
        #endregion

        #region Config :: Constructor
        public Configuration()
        {

        }
        #endregion

        #region Config :: Disk Operations

        public static void InitializeLocalFolder()
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void SaveSettingsToFile()
        {

            XmlSerializer xsSubmit = new XmlSerializer(typeof(Configuration));
            var xml = string.Empty;

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, this);
                    xml = sww.ToString();
                }
            }

            File.WriteAllText(path, xml);

        }
        public static void LoadSettingsFromFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    var stream = new StreamReader(fileStream, Encoding.UTF8);
                    App.CurrentApp.Configuration = (Configuration)serializer.Deserialize(stream);
                }
            }
            catch
            {

                App.CurrentApp.Configuration = new Configuration();
                App.CurrentApp.Configuration.SaveSettingsToFile();
            }

        }

        #endregion

    }
}
