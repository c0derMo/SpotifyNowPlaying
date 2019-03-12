using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpotifyNowPlaying
{
    class OptionManager
    {
        public static decimal checkInterval { get; private set; }
        public static decimal scrollSpeed { get; private set; }
        public static decimal waitDelay { get; private set; }

        public static bool outputToTextFile { get; private set; }

        public static string nameFile { get; private set; }
        public static string artistsFile { get; private set; }
        public static string imageFile { get; private set; }

        public static void createFile()
        {
            XElement elem = new XElement("settings",
                new XElement("checkInterval", 3000),
                new XElement("scrollSpeed", 1000),
                new XElement("waitDelay", 5),
                new XElement("outputToTextFile", false),
                new XElement("nameFile", ""),
                new XElement("artistFile", ""),
                new XElement("imageFile", ""));
            XDocument doc = new XDocument(elem);
            doc.Save("settings.config");
            checkInterval = 3000;
            scrollSpeed = 1000;
            waitDelay = 5;
            outputToTextFile = false;
            nameFile = "";
            artistsFile = "";
            imageFile = "";
        }

        public static void readFile()
        {
            XDocument doc = XDocument.Load("settings.config");
            checkInterval = int.Parse(doc.Element("settings").Element("checkInterval").Value);
            scrollSpeed = int.Parse(doc.Element("settings").Element("scrollSpeed").Value);
            waitDelay = int.Parse(doc.Element("settings").Element("waitDelay").Value);
            outputToTextFile = bool.Parse(doc.Element("settings").Element("outputToTextFile").Value);
            nameFile = doc.Element("settings").Element("nameFile").Value;
            artistsFile = doc.Element("settings").Element("artistFile").Value;
            imageFile = doc.Element("settings").Element("imageFile").Value;
        }

        private static void writeFile()
        {
            Program.npi.updateValues(scrollSpeed, waitDelay);
            XElement elem = new XElement("settings",
                new XElement("checkInterval", checkInterval),
                new XElement("scrollSpeed", scrollSpeed),
                new XElement("waitDelay", waitDelay),
                new XElement("outputToTextFile", outputToTextFile),
                new XElement("nameFile", nameFile),
                new XElement("artistFile", artistsFile),
                new XElement("imageFile", imageFile));
            XDocument doc = new XDocument(elem);
            doc.Save("settings.config");
        }

        public static void setCheckInterval(decimal interval)
        {
            checkInterval = interval;
            writeFile();
        }

        public static void setScrollSpeed(decimal speed)
        {
            scrollSpeed = speed;
            writeFile();
        }
        
        public static void setWaitDelay(decimal delay)
        {
            waitDelay = delay;
            writeFile();
        }

        public static void setOutput(bool isActive)
        {
            outputToTextFile = isActive;
            writeFile();
        }
    
        public static void setNameFile(String file)
        {
            nameFile = file;
            writeFile();
        }

        public static void setArtistFile(String file)
        {
            artistsFile = file;
            writeFile();
        }

        public static void setImageFile(String file)
        {
            imageFile = file;
            writeFile();
        }
    }
}
