using UnityEngine;

namespace SpaceInvaders.Blocks
{
    /// <summary>
    /// Handles the behaviour of the gameplay shield blocks.
    /// </summary>
    public class Block : MonoBehaviour, ICanTakeABullet
    {
        [Tooltip("Number of shots it takes to destroy the block.")]
        [SerializeField] private int m_ShotsToDestroy = 10;

        private float m_ShrinkLerpValue = 0f;
        private Vector3 m_StartScale;

        private void Awake()
        {
            m_StartScale = transform.localScale;
        }

        public void TakeTheBullet()
        {
            Shrink();
        }

        private void Shrink ()
        {
            float lerpAmount = 1 / (float)m_ShotsToDestroy;
            m_ShrinkLerpValue += lerpAmount;

            transform.localScale = Vector3.Lerp(m_StartScale, Vector3.zero, m_ShrinkLerpValue);

            if(m_ShrinkLerpValue >= 1f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
