using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Pooling
{
    /// <summary>
    /// Manages the pooling of a specific object
    /// </summary>
    public class Pool : MonoBehaviour
    {
        private List<PooledBehaviour> m_PooledObjects = new List<PooledBehaviour>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public PooledBehaviour GetDormantPooledObject (PooledBehaviour prefab)
        {
            PooledBehaviour chosen = null;

            foreach (var pooledObject in m_PooledObjects)
            {
                if(!pooledObject.gameObject.activeSelf)
                {
                    chosen = pooledObject;
                    break;
                }
            }

            if(chosen == null)
            {
                chosen = Instantiate(prefab, transform);
                chosen.OnObjectDestroyed += OnPooledObjectDestroyed;
                m_PooledObjects.Add(chosen);
            }

            return chosen;
        }

        private void OnPooledObjectDestroyed (PooledBehaviour pooled)
        {
            pooled.OnObjectDestroyed += OnPooledObjectDestroyed;

            if(m_PooledObjects.Contains(pooled))
            {
                m_PooledObjects.Remove(pooled);
            }
        }
    }
}
