using UnityEngine;
using SpaceInvaders.Pooling;

namespace SpaceInvaders.Projectiles
{
    /// <summary>
    /// Handles the behaviour of projectiles fired by both the player and the invaders.
    /// </summary>
    public class Projectile : PooledBehaviour
    {
        [Tooltip("Velocity with which the projectile should move.")]
        [SerializeField] private Vector3 m_MovementVelocity;

        // Update is called once per frame
        void Update()
        {
            transform.position += m_MovementVelocity * Time.deltaTime;
        }

        /// <summary>
        /// Activate the projectile for action.
        /// </summary>
        public void Activate ()
        {
            gameObject.SetActive(true);
        }

        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
        }

        protected override void OnPoolBehaviourDestroy()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var bulletTaker = collision.transform.GetComponent<ICanTakeABullet>();

            if(bulletTaker != null)
            {
                bulletTaker.TakeTheBullet();
                gameObject.SetActive(false);
            }
        }
    }
}
