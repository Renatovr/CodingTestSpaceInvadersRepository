using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Handles the spawning and behaviour of a whole invader herd.
    /// </summary>
    public class InvadersGroup : MonoBehaviour
    {
        [Header("Spawning")]
        [Tooltip("Configurations with which the invaders will be spawned.")]
        [SerializeField] private RowConfigModel[] m_RowConfigurations;

        [Tooltip("A transform to serve as parent for all the invaders.")]
        [SerializeField] private Transform m_InvadersHolder;

        [Tooltip("Vertical and horizontal spacing for the invaders.")]
        [SerializeField] private float m_InvaderSpacing = 1.0f;

        [Space]
        [Header("Movement")]
        [Tooltip("The horizontal movement speed of the herd of invaders.")]
        [SerializeField] private float m_MovementSpeed = 1.0f;

        [Tooltip("How far the group will move downwards each time it hits the edge of the screen.")]
        [SerializeField] private float m_DownwardStep = 1.0f;

        [Tooltip("Padding from the actual left/right screen edge. Used in determining when to move the group down.")]
        [SerializeField] private float m_ScreenEdgePadding = 0.5f;

        private List<InvadersRow> m_InvadersRows;
        private Vector2 movementDirection;

        // Start is called before the first frame update.
        private void Start()
        {
            Init();
        }

        //Initialize the component.
        private void Init ()
        {
            m_InvadersRows = new List<InvadersRow>();
            movementDirection = Vector2.right;

            if (m_InvadersHolder == null)
            {
                m_InvadersHolder = transform;
            }

            SpawnInvaders();
        }

        private void SpawnInvaders ()
        {
            float groupHeight = m_InvaderSpacing * (m_RowConfigurations.Length - 1);

            //Loop through row configs.
            for (int rowIndex = 0; rowIndex < m_RowConfigurations.Length; rowIndex++)
            {
                var rowConfig = m_RowConfigurations[rowIndex];

                if(rowConfig.ColumnCount <= 0)
                {
                    //The row config is nor valid.
                    continue;
                }

                //Calculate the width of the current row.
                float groupWidth = m_InvaderSpacing * (rowConfig.ColumnCount - 1);

                var center = new Vector2(groupWidth/2, groupHeight/2);
                var rowPosition = new Vector3(-center.x, -center.y + (rowIndex * m_InvaderSpacing), 0f);

                //Create new invader row to manage all invaders in this row.
                var row = new InvadersRow();
                m_InvadersRows.Add(row);

                var prefab = rowConfig.InvaderPrefab;

                //Spawn columns of invaders (left to right)
                for(int columnIndex = 0; columnIndex < rowConfig.ColumnCount; columnIndex++)
                {
                    var invader = Instantiate(prefab, m_InvadersHolder);
                    var invaderPosition = rowPosition;
                    invaderPosition.x += columnIndex * m_InvaderSpacing;
                    invader.transform.localPosition = invaderPosition;

                    row.RegisterInvader(invader);
                }
            }
        }

        // Update is called once per frame.
        private void Update()
        {
            if(Time.timeScale == 0)
            {
                //Don't update the component while the game is paused.
                return;
            }

            UpdateGroupPosition();
        }

        private void UpdateGroupPosition ()
        {
            Vector3 movement = movementDirection * m_MovementSpeed * Time.deltaTime;
            m_InvadersHolder.position += movement;

            var cam = Camera.main;
            var leftEdge = cam.ViewportToWorldPoint(Vector3.zero);
            var rightEdge = cam.ViewportToWorldPoint(Vector3.right);

            foreach(var row in m_InvadersRows)
            {
                if(!row.HasAvailableInvader)
                {
                    continue;
                }

                if((movementDirection == Vector2.right && row.HasInvaderBeyondCameraRightBoundary((rightEdge.x - m_ScreenEdgePadding))) ||
                    (movementDirection == Vector2.left && row.HasInvaderBeyondCameraLeftBoundary((leftEdge.x + m_ScreenEdgePadding))))
                {
                    MoveGroupDown();
                    break;
                }
            }
        }

        private void MoveGroupDown ()
        {
            var position = m_InvadersHolder.position;
            position.y -= m_DownwardStep;
            m_InvadersHolder.position = position;

            //Change movement direction
            movementDirection.x *= -1.0f;
        }
    }
}
