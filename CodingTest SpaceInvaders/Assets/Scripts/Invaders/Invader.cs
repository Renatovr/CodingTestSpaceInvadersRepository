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

        [Tooltip("Minimum interval between projectileShots.")]
        [SerializeField] private float m_MinimumShootInterval = 5f;

        [Tooltip("Maximum interval between projectileShots.")]
        [SerializeField] private float m_MaximumShootInteral = 20f;

        private ShootController m_ShootController;

        /// <summary>
        /// Initialize the invader
        /// </summary>
        public void Init ()
        {
            var shootInterval = UnityEngine.Random.Range(m_MinimumShootInterval, m_MaximumShootInteral);
            var firstShootTime = Time.time + shootInterval;
            m_ShootController = new ShootController(m_ProjectilePrefab, shootInterval, firstShootTime);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Update the behaviour of the invader.
        /// </summary>
        public void UpdateInvader()
        {
            if(m_ShootController.TryShoot(transform.position))
            {
                //Update the next shoot interval
                var shootInterval = UnityEngine.Random.Range(m_MinimumShootInterval, m_MaximumShootInteral);
                m_ShootController.UpdateShootInterval(shootInterval);
            }
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
