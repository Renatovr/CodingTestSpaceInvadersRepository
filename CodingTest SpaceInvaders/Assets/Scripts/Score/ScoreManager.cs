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

        [Tooltip("Reference to a LeaderboardView component. This component should handle displaying the leaderboard.")]
        [SerializeField] private LeaderboardView m_LeaderboardView;

        private int m_CurrentScore = 0;
        private int m_HighScore = 0;

        private bool m_CurrentScoreIsHigh;

        private string m_CurrentName => AppHelper.PlayerName;

        /// <summary>
        /// Score achieved from the current play session.
        /// </summary>
        public int SessionScore => m_CurrentScore;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Refresh();
        }

        /// <summary>
        /// Refresh all values of the score view.
        /// </summary>
        public void Refresh ()
        {
            //This will also lead to the loading of the leaderboard data.
            m_LeaderboardView.RefreshLeaderboard();

            var highestPlayer = Leaderboard.GetHighestUser();

            var highScorerName = string.Empty;

            if(highestPlayer != null)
            {
                m_HighScore = highestPlayer.ScorePoints;
                highScorerName = highestPlayer.PlayerName;
            }

            m_GameplayScoreView.UpdateNames(m_CurrentName, highScorerName);
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

                if(!m_CurrentScoreIsHigh)
                {
                    m_GameplayScoreView.UpdateNames(m_CurrentName, m_CurrentName);
                    m_CurrentScoreIsHigh = true;
                }
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
                PlayerName = AppHelper.PlayerName,
                ScorePoints = m_CurrentScore
            };

            Leaderboard.AddLeaderboardEntry(leaderboardEntry);
            m_LeaderboardView.RefreshLeaderboard();
        }

        /// <summary>
        /// Show the score view for the play session.
        /// </summary>
        public void ShowGameScoreView ()
        {
            m_GameplayScoreView.gameObject.SetActive(true);
        }

        /// <summary>
        /// Show the leaderboard view.
        /// </summary>
        public void ShowLeaderboardView ()
        {
            m_LeaderboardView.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hide all available score views.
        /// </summary>
        public void HideAllViews ()
        {
            m_GameplayScoreView.gameObject.SetActive(false);
            m_LeaderboardView.gameObject.SetActive(false);
        }
    }
}
