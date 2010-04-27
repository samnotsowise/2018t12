using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace AirHockeyGame
{
    class Profile
    {
        #region fields
        String name;
        int win, lost, draw;
        #endregion

        #region Properties

        String Name
        {
            get { return name;  }
            set { name = value; }
        }

        int Win
        { get { return win; } }

        int Lost
        { get { return lost; } }           

        int Draw
        { get { return draw; } }

        int TotalGamesPlayed
        { get { return win + lost + draw; } }
#endregion

        #region Methods

        /// <summary>
        /// Constructor to create a "blank" profile.
        /// </summary>
        public Profile() : this("")
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="name"></param>
        public Profile(String name)
        {
            this.name = name;
            win = lost = draw = 0;
        }

        /// <summary>
        /// Increments games won counter
        /// </summary>
        public void GameWon()
        {
            win++;
        }

        /// <summary>
        /// Increments games lost counter
        /// </summary>
        public void GameLost()
        {
            lost++;
        }

        /// <summary>
        /// Increments games drawn counter
        /// </summary>
        public void GameDrawn()
        {
            draw++;
        }

        /// <summary>
        /// Reads profile in from specified path.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Profile ReadProfile(String fileName)
        {
            //To read file
            if (!File.Exists(fileName))
                return null;

            //Open file
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

            //Deserialse data into new instance
            XmlSerializer serializer = new XmlSerializer(typeof(Profile));

            Profile settingsFromFile = (Profile)serializer.Deserialize(stream);

            //Close stream
            stream.Close();

            //Return the found file
            return settingsFromFile;
        }

        /// <summary>
        /// Writes profile to path with specified filename.
        /// </summary>
        /// <param name="fileName"></param>
        public void WriteProfile(String fileName)
        {
            //Open filestream
            File.Delete(fileName);
            FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);

            //Serialise the data
            XmlSerializer serializer = new XmlSerializer(typeof(Profile));
            serializer.Serialize(stream, this);

            //Close stream
            stream.Close();
        }

        #endregion
    }
}
