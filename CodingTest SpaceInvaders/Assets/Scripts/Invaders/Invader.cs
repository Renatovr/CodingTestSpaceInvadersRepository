using System;
using UnityEngine;
using SpaceInvaders.Projectiles;
using SpaceInvaders.Score;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Handles the general behaviour of each individual invader.
    /// </summary>
    public class Invader : MonoBehaviour, ICanTakeABullet
    {
        private const int SCORE_POINTS_FROM_DEFEAT = 100;
        /// <summary>
        /// Event fired when the invader takes a hit and dies.
        /// </summary>
        public event Action<Invader> OnInvaderKilled;

        [Tooltip("Reference to a projectile prefab to be shot.")]
        [SerializeField] private Projectile m_ProjectilePrefab;

        [Tooltip("SpriteRenderer component on this invader's gameObject.")]
        [SerializeField] private SpriteRenderer m_SpriteRenderer;

        private ShootController m_ShootController;

        /// <summary>
        /// The color of this invader.
        /// </summary>
        public Color Color => m_SpriteRenderer != null ? m_SpriteRenderer.color : Color.white;

        /// <summary>
        /// Initialize the invader
        /// </summary>
        public void Init ()
        {
            m_ShootController = new ShootController(m_ProjectilePrefab, 0f);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Update the behaviour of the invader.
        /// </summary>
        public void Shoot()
        {
            m_ShootController.ShootImmediately(transform.position);
        }

        public void TakeTheBullet()
        {
            Die();
        }

        private void Die()
        {
            //Add points to score.
            if(ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPointsToScore(SCORE_POINTS_FROM_DEFEAT);
            }

            gameObject.SetActive(false);
            OnInvaderKilled?.Invoke(this);
        }
    }
}
