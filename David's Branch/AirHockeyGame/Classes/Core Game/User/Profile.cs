/*
 * Holds information on the user and can be written to and read from file.
 * 
 * Author(s):
 *  David Valente
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace AirHockeyGame
{
    public class Profile
    {
        #region fields
        String name;
        int win, lost, draw;
        #endregion

        #region Properties

        public String Name
        {
            get { return name;  }
            set { name = value; }
        }

        public int Win
        { get { return win; } }

        public int Lost
        { get { return lost; } }           

        public int Draw
        { get { return draw; } }

        public string WonLostDrawn
        { get { return ""+win+"/"+lost+"/"+draw; } }
        
        public int TotalGamesPlayed
        { get { return win + lost + draw; } }
#endregion

        #region Methods

        /// <summary>
        /// Constructor to create a "blank" profile.
        /// </summary>
        public Profile() : this("Player Name")
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
        public static Profile ReadProfile(String fileName)
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

        /// <summary>
        /// Compares this profile with another to check for a match.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Matches(Profile p)
        {
            if (p.name.Equals(this.name) && p.WonLostDrawn.Equals(this.WonLostDrawn))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies another profile's variables to create a copy.
        /// </summary>
        /// <param name="p"></param>
        public void Copy(Profile p)
        {
            this.win = p.win;
            this.lost = p.lost;
            this.draw = p.draw;
            this.name = p.name;
        }

        #endregion
    }
}
