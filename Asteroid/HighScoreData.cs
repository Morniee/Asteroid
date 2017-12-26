using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Asteroid
{
    [Serializable]
    public struct HighScoreData
    {
        public string[] PlayerName;
        public int[] Score;

        public int Count;

        public HighScoreData(int count)
        {
            PlayerName = new string[count];
            Score = new int[count];

            Count = count;
        }

        public static void Initialize()
        {
            // Get the path of the save game
            string fullpath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, GameConstants.LEADERBOARDS_FILE_NAME);

            // Check to see if the save exists
            if (!File.Exists(fullpath))
            {
                //If the file doesn't exist, make a fake one...
                // Create the data to save
                HighScoreData data = new HighScoreData(5);
                data.PlayerName[0] = "CPU";
                data.Score[0] = 2000;

                data.PlayerName[1] = "CPU";
                data.Score[1] = 1000;

                data.PlayerName[2] = "CPU";
                data.Score[2] = 500;

                data.PlayerName[3] = "CPU";
                data.Score[3] = 100;

                data.PlayerName[4] = "CPU";
                data.Score[4] = 50;

                SaveHighScores(data);
            }
        }

        public static void SaveHighScores(HighScoreData data)
        {
            // Get the path of the save game
            string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GameConstants.LEADERBOARDS_FILE_NAME);

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }

        public static HighScoreData LoadHighScores()
        {
            HighScoreData data;

            // Get the path of the save game
            string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GameConstants.LEADERBOARDS_FILE_NAME);

            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
            FileAccess.Read);
            try
            {

                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                data = (HighScoreData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

            return data;
        }

        public void SaveHighScore(int score, string name)
        {
            // Create the data to save
            HighScoreData data = LoadHighScores();

            int scoreIndex = -1;
            for (int i = 0; i < data.Count; i++)
            {
                if (score > data.Score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                //New high score found ... do swaps
                for (int i = data.Count - 1; i > scoreIndex; i--)
                {
                    data.PlayerName[i] = data.PlayerName[i - 1];
                    data.Score[i] = data.Score[i - 1];
                }

                data.PlayerName[scoreIndex] = name;
                data.Score[scoreIndex] = score;

                SaveHighScores(data);
            }
        }
    }
}

