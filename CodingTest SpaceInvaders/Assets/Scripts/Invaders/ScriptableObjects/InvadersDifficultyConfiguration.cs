using UnityEngine;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Configuration holding values for difficulty progression.
    /// </summary>
    [CreateAssetMenu(fileName = "InvadersDifficultyConfiguration", menuName = "Space Invaders/Create New Difficulty Config Asset", order = 0)]
    public class InvadersDifficultyConfiguration : ScriptableObject
    {
        /// <summary>
        /// Amount to add to invaders speed multiplier for each time an invader is killed.
        /// </summary>
        public float SpeedIncreasePerInvaderDeath = 0.01f;

        /// <summary>
        /// Amount to add to invaders speed each time we clear a whole wave of invaders.
        /// </summary>
        public float SpeedIncreasePerWaveCleared = 0.1f;
    }
}
