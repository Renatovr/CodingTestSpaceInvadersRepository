using System;
using UnityEngine;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Handles the setup for each row of invaders.
    /// </summary>
    [Serializable]
    public class RowConfigModel
    {
        /// <summary>
        /// The number of columns contained within the row.
        /// </summary>
        [Tooltip("The number of columns the row should contain.")]
        public int ColumnCount;

        /// <summary>
        /// The prefab for invaders on this row.
        /// </summary>
        [Tooltip("Reference to a prefab to instantiate across the row.")]
        public Invader InvaderPrefab;
    }
}
