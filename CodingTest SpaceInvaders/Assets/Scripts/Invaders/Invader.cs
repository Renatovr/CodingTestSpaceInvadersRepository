using System.Collections;
using System;
using UnityEngine;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Handles the general behaviour of each individual invader.
    /// </summary>
    public class Invader : MonoBehaviour
    {
        /// <summary>
        /// Event fired when the invader takes a hit and dies.
        /// </summary>
        public event Action<Invader> OnInvaderKilled;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Die ()
        {
            OnInvaderKilled?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
