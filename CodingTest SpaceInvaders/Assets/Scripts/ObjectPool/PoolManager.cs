using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Pooling
{
    /// <summary>
    /// Manages the pooling of multiple types objects.
    /// It only works for PooledBehaviours.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager m_Instance;

        public static PoolManager Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new GameObject("PoolManager").AddComponent<PoolManager>();
                    m_Instance.Init();
                }

                return m_Instance;
            }
        }

        private Dictionary<PooledBehaviour, Pool> m_Pools;

        private void Init ()
        {
            m_Pools = new Dictionary<PooledBehaviour, Pool>();
        }

        /// <summary>
        /// Tries to get a dormant pooled instance object for the provided object prefab.
        /// If not is available, it will create a new one.
        /// </summary>
        /// <typeparam name="T">A type which inherits from PooledBehaviour.</typeparam>
        /// <param name="objectPrefab">An object prefab which matches the provided type.</param>
        /// <returns>Returns an instance of the prefab.</returns>
        public T GetInstanceOfObject<T> (T objectPrefab) where T : PooledBehaviour
        {
            //Create a new pool for the object, if not available
            if (!m_Pools.TryGetValue(objectPrefab, out var pool))
            {
                pool = new GameObject($"Pool ({objectPrefab.name})").AddComponent<Pool>();
                pool.transform.parent = transform;
                m_Pools[objectPrefab] = pool;
            }

            return pool.GetDormantPooledObject(objectPrefab) as T;
        }
    }
}
