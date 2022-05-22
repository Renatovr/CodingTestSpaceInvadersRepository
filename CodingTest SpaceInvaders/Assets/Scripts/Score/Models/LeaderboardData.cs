using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SpaceInvaders.Score
{
    /// <summary>
    /// Class to load, save and sort leaderboard entries.
    /// </summary>
    public static class Leaderboard
    {
        private const string SAVE_FILE_NAME = "Leaderboard.spc";
        private const int LEADERBOARD_ENTRIES_LIMIT = 3;

        /// <summary>
        /// Static instance of a Leaderboard
        /// </summary>
        public static LeaderboardData Data { get; private set; }
        
        /// <summary>
        /// Load leaderboard data from a system file, if available.
        /// </summary>
        public static void LoadFromFile ()
        {
            var path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

            if(File.Exists(path))
            {
                var file = File.Open(path, FileMode.Open);
                var bf = new BinaryFormatter();

                Data = (LeaderboardData) bf.Deserialize(file);
                file.Close();
            }
            else
            {
                Data = new LeaderboardData();
            }

            Debug.Log("Leaderboard loaded with entry count: " + Data.LeaderboardEntries.Count);
        }

        /// <summary>
        /// Save leaderboard data to a system file.
        /// </summary>
        public static void SaveToFile ()
        {
            if(Data == null)
            {
                Data = new LeaderboardData();
            }

            var path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

            var file = File.Open(path, FileMode.OpenOrCreate);
            var bf = new BinaryFormatter();
            bf.Serialize(file, Data);
            file.Close();
            Debug.Log("Leaderboard saved with entry count: " + Data.LeaderboardEntries.Count);
        }

        /// <summary>
        /// Add a new entry to the leaderboard.
        /// This will trigger the sorting of all leaderboard entries in descending order.
        /// The entries will be limited to a specific count;
        /// </summary>
        /// <param name="entry"></param>
        public static void AddLeaderboardEntry (LeaderboardEntry entry)
        {
            Data.LeaderboardEntries.Add(entry);

            var sortedData = Data.LeaderboardEntries.OrderBy(entry => entry.ScorePoints).Reverse().Take(LEADERBOARD_ENTRIES_LIMIT);
            Data.LeaderboardEntries = sortedData.ToList();
            SaveToFile();
        }
    }

    /// <summary>
    /// Simple data model for a list of leaderboard entries.
    /// </summary>
    [Serializable]
    public class LeaderboardData
    {
        /// <summary>
        /// The list of leaderboard entries
        /// </summary>
        public List<LeaderboardEntry> LeaderboardEntries = new List<LeaderboardEntry>();
    }
}
