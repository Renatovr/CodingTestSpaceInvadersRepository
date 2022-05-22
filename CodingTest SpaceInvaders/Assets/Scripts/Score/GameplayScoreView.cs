using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.Score
{
    /// <summary>
    /// Handles displaying scores for during a gameplay session.
    /// </summary>
    public class GameplayScoreView : MonoBehaviour
    {
        [Tooltip("Reference to the text which displays the player's current score.")]
        [SerializeField] private Text m_CurrentScoreText;

        [Tooltip("Reference to the text which displays the current high score.")]
        [SerializeField] private Text m_HighScoreText;

        /// <summary>
        /// Update the scores shown on the UI.
        /// </summary>
        /// <param name="currentScore">The new current score value.</param>
        /// <param name="highScore">The new high score value.</param>
        public void UpdateScores (int currentScore, int highScore)
        {
            m_CurrentScoreText.text = currentScore.ToString();
            m_HighScoreText.text = highScore.ToString();
        }
    }
}
