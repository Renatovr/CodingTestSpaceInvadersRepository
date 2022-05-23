using UnityEngine;

namespace SpaceInvaders.Player
{
    /// <summary>
    /// Class which will handle the logical part of the player's movement
    /// </summary>
    public class PlayerMovementController
    {
        private const string MOVEMENT_AXIS_NAME = "Horizontal";

        private float m_MovementSpeed;

        /// <summary>
        /// Construct a new instance of PlayerMovement controller, providing the relevant data.
        /// </summary>
        /// <param name="movementSpeed">Speed with which the object will calculate movement.</param>
        public PlayerMovementController (float movementSpeed)
        {
            m_MovementSpeed = movementSpeed;
        }

        /// <summary>
        /// Calculate value for a new position after movement is done.
        /// </summary>
        /// <param name="currentPosition">The position before movement calculation.</param>
        /// <returns>Returns an updated position after movement is calculated from currentPosition.</returns>
        public Vector3 GetUpdatedPosition (Vector3 currentPosition)
        {
            var movementInput = Input.GetAxisRaw(MOVEMENT_AXIS_NAME);

            var newPosition = currentPosition;
            newPosition.x += movementInput * m_MovementSpeed * Time.deltaTime;
            newPosition.x = GetClampedToCameraHorizontalView(newPosition.x);

            return newPosition;
        }

        private float GetClampedToCameraHorizontalView (float currentPosition)
        {
            var clampedPosition = currentPosition;
            var camera = Camera.main;

            if(camera != null)
            {
                //Get world positions for the left and right borders of the camera viewport
                var leftBorder = camera.ViewportToWorldPoint(Vector3.zero);
                var rightBorder = camera.ViewportToWorldPoint(Vector3.right);
                clampedPosition = Mathf.Clamp(clampedPosition, leftBorder.x, rightBorder.x);
            }

            return clampedPosition;
        }
    }
}
