using UnityEngine;

namespace SpaceInvaders.Projectiles
{
    /// <summary>
    /// Handles the shooting of a projectile.
    /// </summary>
    public class ShootController
    {
        private Projectile m_ProjectilePrefab;
        private float m_ShootInterval;

        private float m_NextShootTime = 0f;

        /// <summary>
        /// Construct a new ShootController with the necessary data.
        /// </summary>
        /// <param name="projectilePrefab"></param>
        /// <param name="shootInterval"></param>
        /// <param name="firstShootTime">The time the first shot should be taken</param>
        public ShootController (Projectile projectilePrefab, float shootInterval, float firstShootTime = 0f)
        {
            m_ProjectilePrefab = projectilePrefab;
            m_ShootInterval = shootInterval;
            m_NextShootTime = firstShootTime;
        }

        /// <summary>
        /// Assign a new shoot rate to the object.
        /// </summary>
        /// <param name="newShootInterval">The new shoot rate to be assigned to the object.</param>
        public void UpdateShootInterval (float newShootInterval)
        {
            m_ShootInterval = newShootInterval;
        }

        /// <summary>
        /// Will shoot if the right conditions are met.
        /// <param name="projectileOrigin">The position at which the an instance of the projectile will be placed as origin.</param>
        /// </summary>
        /// <returns>Returns true if the right shooting conditions are met</returns>
        public bool TryShoot (Vector3 projectileOrigin)
        {
            if(m_ProjectilePrefab == null)
            {
                return false;
            }

            if(Time.time > m_NextShootTime)
            {
                Shoot(projectileOrigin);
                m_NextShootTime = Time.time + m_ShootInterval;
                return true;
            }

            return false;
        }

        private void Shoot (Vector3 projectileOrigin)
        {
            var projectile = Pooling.PoolManager.Instance.GetInstanceOfObject<Projectile>(m_ProjectilePrefab);

            if(projectile != null)
            {
                projectile.transform.position = projectileOrigin;
                projectile.Activate();
            }
        }
    }
}
