using UnityEngine;

namespace SpaceInvaders.Score
{
    /// <summary>
    /// Manages receiving and processing of player scores.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        /// <summary>
        /// Static reference to an instance of the ScoreManager class.
        /// </summary>
        public static ScoreManager Instance { get; private set; }

        [Tooltip("Reference to a GameplayScoreView component. This component should handle displaying scores during a session.")]
        [SerializeField] private GameplayScoreView m_GameplayScoreView;

        private int m_CurrentScore = 0;
        private int m_HighScore = 0;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }

        private void Init ()
        {
            Leaderboard.LoadFromFile();
            m_GameplayScoreView.UpdateScores(m_CurrentScore, m_HighScore);
        }

        /// <summary>
        /// Add some points to the score of the current play session.
        /// </summary>
        /// <param name="points">Number of points to be added to the score</param>
        public void AddPointsToScore (int points)
        {
            m_CurrentScore += points;

            if(m_CurrentScore > m_HighScore)
            {
                m_HighScore = m_CurrentScore;
            }

            m_GameplayScoreView.UpdateScores(m_CurrentScore, m_HighScore);
        }

        /// <summary>
        /// Save the scores which have been accumulated by the user
        /// </summary>
        public void SaveUserScore ()
        {
            //Create new leaderboard entry for the user
            var leaderboardEntry = new LeaderboardEntry
            {
                PlayerName = "TestPlayer",
                ScorePoints = m_CurrentScore
            };

            Leaderboard.AddLeaderboardEntry(leaderboardEntry);
        }
    }
}
