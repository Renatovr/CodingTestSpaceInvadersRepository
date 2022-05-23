using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.Score
{
    /// <summary>
    /// Handles each row in the leaderboard view.
    /// </summary>
    public class LeaderboardRow : MonoBehaviour
    {
        [Tooltip("text to indicate position on the leaderboard")]
        [SerializeField] private Text m_PositionText;

        [Tooltip("text to indicate the player's name on the leaderboard")]
        [SerializeField] private Text m_PlayerNameText;

        [Tooltip("text to indicate score on the leaderboard")]
        [SerializeField] private Text m_ScoreText;

        /// <summary>
        /// Display a leaderboard entry on the UI.
        /// </summary>
        /// <param name="entry">LeaderboardEntry to be displayed.</param>
        /// <param name="position">The position of this row on the leaderboard</param>
        public void Init (LeaderboardEntry entry, int position)
        {
            m_PositionText.text = position.ToString();
            m_PlayerNameText.text = entry.PlayerName;
            m_ScoreText.text = entry.ScorePoints.ToString();
            gameObject.name = $"Row ({entry.PlayerName})";
            gameObject.SetActive(true);
        }
    }
}
