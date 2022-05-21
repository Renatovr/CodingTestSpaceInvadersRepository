using System.Collections.Generic;
using System.Linq;
using System;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Handles each row of Invaders.
    /// </summary>
    public class InvadersRow
    {
        public event Action OnRowCleared;

        private List<Invader> m_Invaders;

        /// <summary>
        /// Whether the row has at least one available invader.
        /// </summary>
        public bool HasAvailableInvader
        {
            get
            {
                var activeInvaders = m_Invaders.Where(inv => inv.gameObject.activeSelf);
                return activeInvaders.Count() > 0;
            }
        }

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
            invader.Init();

            if (!m_Invaders.Contains(invader))
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
                var leftInvader = m_Invaders.Where(inv => inv.gameObject.activeSelf).FirstOrDefault();

                if(leftInvader != null)
                {
                    return leftInvader.transform.position.x <= leftBoundary;
                }
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
                var rightInvader = m_Invaders.Where(inv => inv.gameObject.activeSelf).LastOrDefault();

                if(rightInvader != null)
                {
                    return rightInvader.transform.position.x >= rightBoundary;
                }
            }
            return false;
        }

        /// <summary>
        /// Update all available invaders.
        /// </summary>
        public void UpdateInvaders ()
        {
            var availableInvaders = m_Invaders.Where(inv => inv.gameObject.activeSelf);
            foreach (var invader in availableInvaders)
            {
                invader.UpdateInvader();
            }
        }

        /// <summary>
        /// Reset all invaders the row
        /// </summary>
        public void ResetRow()
        {
            foreach(var invader in m_Invaders)
            {
                RegisterInvader(invader);
            }
        }

        private void OnInvaderKilled (Invader invader)
        {
            invader.OnInvaderKilled -= OnInvaderKilled;

            if(!HasAvailableInvader)
            {
                OnRowCleared?.Invoke();
            }
        }
    }
}
