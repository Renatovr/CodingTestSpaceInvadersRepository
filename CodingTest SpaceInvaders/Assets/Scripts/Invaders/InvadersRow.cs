using System.Collections.Generic;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Handles each row of Invaders.
    /// </summary>
    public class InvadersRow
    {
        private List<Invader> m_Invaders;

        /// <summary>
        /// Whether the row has at least one available invader.
        /// </summary>
        public bool HasAvailableInvader => m_Invaders.Count > 0;

        /// <summary>
        /// Construct a new instance of InvadersRow with all necessary internal initialization.
        /// </summary>
        public InvadersRow ()
        {
            m_Invaders = new List<Invader>();
        }

        /// <summary>
        /// Register a new invader to be observed and managed.
        /// </summary>
        /// <param name="invader"></param>
        public void RegisterInvader (Invader invader)
        {
            invader.OnInvaderKilled += OnInvaderKilled;

            if(!m_Invaders.Contains(invader))
            {
                m_Invaders.Add(invader);
            }
        }

        /// <summary>
        /// Check from the left-most available invader, to see whether it has gone beyond the provided left boundary.
        /// </summary>
        /// <param name="leftBoundary">The left boundary position.</param>
        /// <returns>Returns true if the row has an invader beyond the left boundary.</returns>
        public bool HasInvaderBeyondCameraLeftBoundary (float leftBoundary)
        {
            if(m_Invaders.Count > 0)
            {
                var leftInvader = m_Invaders[0];

                return leftInvader.transform.position.x <= leftBoundary;
            }
            return false;
        }

        /// <summary>
        /// Check from the right-most available invader, to see whether it has gone beyond the provided right boundary.
        /// </summary>
        /// <param name="rightBoundary">The right boundary position.</param>
        /// <returns>Returns true if the row has an invader beyond the right boundary.</returns>
        public bool HasInvaderBeyondCameraRightBoundary (float rightBoundary)
        {
            if (m_Invaders.Count > 0)
            {
                var rightInvader = m_Invaders[(m_Invaders.Count - 1)];

                return rightInvader.transform.position.x >= rightBoundary;
            }
            return false;
        }

        /// <summary>
        /// Update all available invaders.
        /// </summary>
        public void UpdateInvaders ()
        {
            foreach(var invader in m_Invaders)
            {
                invader.UpdateInvader();
            }
        }

        private void OnInvaderKilled (Invader invader)
        {
            invader.OnInvaderKilled -= OnInvaderKilled;

            if (m_Invaders.Contains(invader))
            {
                m_Invaders.Remove(invader);
            }
        }
    }
}
