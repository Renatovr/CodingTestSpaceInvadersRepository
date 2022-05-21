using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Handles each row of Invaders.
    /// </summary>
    public class InvadersRow
    {
        private List<Invader> m_Invaders;

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
