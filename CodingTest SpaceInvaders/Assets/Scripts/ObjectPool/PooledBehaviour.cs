using System;
using UnityEngine;

namespace SpaceInvaders.Pooling
{
    /// <summary>
    /// A child of MonoBehaviour which works with our pooling system.
    /// </summary>
    public abstract class PooledBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Event fired whent the object is being destroyed.
        /// </summary>
        public event Action<PooledBehaviour> OnObjectDestroyed;

        private void OnDestroy()
        {
            OnPoolBehaviourDestroy();
            OnObjectDestroyed?.Invoke(this);
        }

        protected abstract void OnPoolBehaviourDestroy();
    }
}
