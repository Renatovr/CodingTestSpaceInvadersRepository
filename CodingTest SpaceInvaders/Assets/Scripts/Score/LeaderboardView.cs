using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Score
{
    public class LeaderboardView : MonoBehaviour
    {
        [Tooltip("An instance of LeaderboardRow to serve as a template.")]
        [SerializeField] private LeaderboardRow m_RowTemplate;

        [Tooltip("Transform to serve as parent for the rows.")]
        [SerializeField] private Transform m_RowHolder;

        private List<LeaderboardRow> m_Rows = new List<LeaderboardRow>();

        /// <summary>
        /// Refresh the data displayed by the leaderboard.
        /// </summary>
        public void RefreshLeaderboard ()
        {
            ClearRows();
            Leaderboard.LoadFromFile();

            var entries = Leaderboard.Data.LeaderboardEntries;
            CreateRows(entries.ToArray());
        }

        private void ClearRows ()
        {
            foreach(var row in m_Rows)
            {
                Destroy(row.gameObject);
            }

            m_Rows.Clear();
        }

        private void CreateRows (LeaderboardEntry[] entries)
        {
            if(entries == null || entries.Length == 0)
            {
                Debug.Log("No entries in the leaderboard.");
                return;
            }

            for(int i = 0; i < entries.Length; i++)
            {
                var newRow = Instantiate(m_RowTemplate, m_RowHolder);
                newRow.Init(entries[i], i+1);

                m_Rows.Add(newRow);
            }
        }
    }
}
