/*
 * Holds settings for the game including difficulty.
 * 
 * Author(s):
 *  David Valente
 */

using System.IO;
using System.Xml.Serialization;

namespace GameScreenManager.ScreenSystem {
    public class Settings {
        static string fileName = "settings.xml";

        public enum Difficulty {
            easy,
            medium,
            hard
        }
        public Difficulty difficulty;

        public enum ScreenSize {
            windowed,
            fullscreen
        }
        public ScreenSize screenSize;

        public string serverAddress;
        public string serverPort;
        //Creates an object to hold the settings with default values
        public Settings() {
            difficulty = Difficulty.medium;
            screenSize = ScreenSize.windowed;
            serverAddress = "localhost";
            serverPort = "2000";
        }

        /// <summary>
        /// Checks two settings instances are the same.
        /// </summary>
        /// <returns></returns>
        public bool IsEqual(Settings set2) {
            if(this.difficulty == set2.difficulty &&
                this.screenSize == set2.screenSize && 
                this.serverAddress == set2.serverAddress && 
                this.serverPort == set2.serverPort)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Writes the settings to an XML file.
        /// </summary>
        public void WriteSettingsFile() {
            //Open filestream
            File.Delete(fileName);
            FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);

            //Serialise the data
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            serializer.Serialize(stream, this);

            //Close stream
            stream.Close();
        }

        /// <summary>
        /// Copies one settings instance into this one.
        /// </summary>
        /// <param name="set"></param>
        public void Copy(Settings set) {
            this.difficulty = set.difficulty;
            this.screenSize = set.screenSize;
            this.serverPort = set.serverPort;
            this.serverAddress = set.serverAddress;
        }

        /// <summary>
        /// Reads the settings from an XML file.
        /// </summary>
        public static Settings ReadSettingsFile() {
            //To read file
            if(!File.Exists(fileName))
                return null;

            //Open file
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

            //Deserialse data into new instance
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            Settings settingsFromFile = (Settings)serializer.Deserialize(stream);

            //Close stream
            stream.Close();

            //Return the found file
            return settingsFromFile;

        }
    }
}