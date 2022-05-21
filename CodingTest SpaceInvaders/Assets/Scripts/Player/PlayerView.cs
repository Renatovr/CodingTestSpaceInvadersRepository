using UnityEngine;

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

        private PlayerMovementController m_MovementController;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        private void Init ()
        {
            m_MovementController = new PlayerMovementController(m_MovementSpeed);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = m_MovementController.GetUpdatedPosition(transform.position);
        }
    }
}
