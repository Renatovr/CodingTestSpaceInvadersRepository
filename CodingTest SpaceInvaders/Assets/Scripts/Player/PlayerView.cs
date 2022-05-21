using UnityEngine;
using SpaceInvaders.Projectiles;

namespace SpaceInvaders.Player
{
    /// <summary>
    /// The class to hold and handle the general behaviour of the player ship.
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        [Tooltip("The speed with which the player will move horizontally, while being controlled.\n" +
            "This value will be applied on initialization, and will not be updated in real-time.")]
        [SerializeField] private float m_MovementSpeed = 5f;

        [Tooltip("Prefab of the projectile to be shot by the player.")]
        [SerializeField] private Projectile m_ProjectilePrefab;

        [Tooltip("Minimum interval between each shot.")]
        [SerializeField] private float m_ShoorInterval = 1.0f;

        [Tooltip("The point the projectiles will originate from.")]
        [SerializeField] private Transform m_ShootPoint;

        [Tooltip("The key with which we will shoot a projectile.")]
        [SerializeField] private KeyCode m_ShootKey = KeyCode.S;

        private PlayerMovementController m_MovementController;
        private ShootController m_ShootController;

        // Start is called before the first frame update
        private void Start()
        {
            Init();
        }

        private void Init ()
        {
            m_MovementController = new PlayerMovementController(m_MovementSpeed);
            m_ShootController = new ShootController(m_ProjectilePrefab, m_ShoorInterval);
        }

        // Update is called once per frame
        private void Update()
        {
            if(Time.timeScale <= 0f)
            {
                return;
            }

            transform.position = m_MovementController.GetUpdatedPosition(transform.position);
            UpdateShoot();
        }

        private void UpdateShoot ()
        {
            if(Input.GetKey(m_ShootKey))
            {
                _ = m_ShootController.TryShoot(m_ShootPoint.position);
            }
        }
    }
}
