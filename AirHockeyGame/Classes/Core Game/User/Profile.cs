/*
 *      Profile Class
 *      
 * Description:
 *      Holds information on the user
 *      and can be written to and read
 *      from file.
 * 
 * Author(s):
 *      David Valente
 */

using System;
using System.IO;
using System.Xml.Serialization;

namespace AirHockeyGame {
    public class Profile {
        #region fields
        String name;
        int win, lost, draw;
        public const int MaxNameLength = 15;
        int picture;
        #endregion

        #region Properties

        public String Name {
            get { return name; }
            set { name = value; }
        }

        public int Win {
            get { return win; }
            set { win = value; }
        }

        public int Lost {
            get { return lost; }
            set { lost = value; }
        }

        public int Draw {
            get { return draw; }
            set { draw = value; }
        }

        public string WonLostDrawn { get { return "" + win + "/" + lost + "/" + draw; } }

        public int PictureIndex {
            get { return picture; }
            set { picture = value; }
        }

        public int TotalGamesPlayed { get { return win + lost + draw; } }
        #endregion

        #region Methods

        /// <summary>
        /// Constructor to create a "blank" profile.
        /// </summary>
        public Profile()
            : this("Player Name") {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="name"></param>
        public Profile(String name) {
            this.name = name;
            win = lost = draw = 0;
            this.PictureIndex = 0;
        }

        /// <summary>
        /// Increments games won counter
        /// </summary>
        public void GameWon() {
            win++;
            GameState.playerProfile.WriteProfile("profile.dat");
        }

        /// <summary>
        /// Increments games lost counter
        /// </summary>
        public void GameLost() {
            lost++;
            GameState.playerProfile.WriteProfile("profile.dat");
        }

        /// <summary>
        /// Increments games drawn counter
        /// </summary>
        public void GameDrawn() {
            draw++;
            GameState.playerProfile.WriteProfile("profile.dat");
        }

        /// <summary>
        /// Reads profile in from specified path.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Profile ReadProfile(String fileName) {
            //To read file
            if(!File.Exists(fileName))
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
        public void WriteProfile(String fileName) {
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
        public bool Matches(Profile p) {
            if(p.name.Equals(this.name) && p.WonLostDrawn.Equals(this.WonLostDrawn) && p.PictureIndex == this.PictureIndex)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies another profile's variables to create a copy.
        /// </summary>
        /// <param name="p"></param>
        public void Copy(Profile p) {
            this.win = p.win;
            this.lost = p.lost;
            this.draw = p.draw;
            this.name = p.name;
        }

        #endregion
    }
}