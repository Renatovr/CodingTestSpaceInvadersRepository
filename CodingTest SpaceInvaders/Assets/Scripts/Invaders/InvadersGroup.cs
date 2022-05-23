using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace SpaceInvaders.Invaders
{
    /// <summary>
    /// Handles the spawning and behaviour of a whole invader herd.
    /// </summary>
    public class InvadersGroup : MonoBehaviour
    {
        private const float DELAY_BETWEEN_WAVE_SECONDS = 2.0f;

        [Header("Spawning")]
        [Tooltip("Configurations with which the invaders will be spawned.")]
        [SerializeField] private RowConfigModel[] m_RowConfigurations;

        [Tooltip("A transform to serve as parent for all the invaders.")]
        [SerializeField] private Transform m_InvadersHolder;

        [Tooltip("Vertical and horizontal spacing for the invaders.")]
        [SerializeField] private float m_InvaderSpacing = 1.0f;

        [Space]
        [Header("Movement, Effects And Shooting")]
        [Tooltip("The horizontal movement speed of the herd of invaders.")]
        [SerializeField] private float m_MovementSpeed = 1.0f;

        [Tooltip("How far the group will move downwards each time it hits the edge of the screen.")]
        [SerializeField] private float m_DownwardStep = 1.0f;

        [Tooltip("Padding from the actual left/right screen edge. Used in determining when to move the group down.")]
        [SerializeField] private float m_ScreenEdgePadding = 0.5f;

        [Tooltip("The position on the y-axis the invader group needs to reach for the player to lose.")]
        [SerializeField] private float m_MaximumInvasionPositionY = -6.0f;

        [Space]

        [Tooltip("Minimum interval between projectileShots.")]
        [SerializeField] private float m_MinimumShootInterval = 1f;

        [Tooltip("Maximum interval between projectileShots.")]
        [SerializeField] private float m_MaximumShootInteral = 5f;

        [Space]

        [Tooltip("Particle Effect to play on Invader death.")]
        [SerializeField] private ParticleSystem m_DeathParticle;

        [Tooltip("How many particles to emit when playing the death particle")]
        [SerializeField] private int m_DeathParticleCount = 30;

        [Space]
        [Header("Progression")]
        [Tooltip("Reference to a difficulty configuration asset.")]
        [SerializeField] private InvadersDifficultyConfiguration m_DifficultyConfig;

        private List<InvadersRow> m_InvadersRows;
        private Vector2 m_MovementDirection;
        private Vector3 m_InvadersHolderStartPosition;

        private float m_WaveSpeed = 1.0f;
        private float m_ProgressionSpeed = 0.0f;
        private float m_DifficultySpeedMultiplier => (m_WaveSpeed + m_ProgressionSpeed);

        private float m_ShootWaitTime = 0f;

        // Start is called before the first frame update.
        private void Start()
        {
            Init();
        }

        //Initialize the component.
        private void Init ()
        {
            m_InvadersRows = new List<InvadersRow>();
            m_MovementDirection = Vector2.right;
            m_InvadersHolderStartPosition = m_InvadersHolder.position;
            ResetShootWaitTime();

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
                row.OnRowCleared += OnRowCleared;
                row.OnInvaderKilledEvent += OnInvaderKilled;
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
            UpdateShooting();
        }

        private void UpdateGroupPosition ()
        {
            Vector3 movement = m_MovementSpeed * Time.deltaTime * m_MovementDirection * m_DifficultySpeedMultiplier;
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

                if((m_MovementDirection == Vector2.right && row.HasInvaderBeyondCameraRightBoundary((rightEdge.x - m_ScreenEdgePadding))) ||
                    (m_MovementDirection == Vector2.left && row.HasInvaderBeyondCameraLeftBoundary((leftEdge.x + m_ScreenEdgePadding))))
                {
                    MoveGroupDown();
                    break;
                }
            }
        }

        private void UpdateShooting ()
        {
            if(m_ShootWaitTime > 0f)
            {
                m_ShootWaitTime -= Time.deltaTime * m_DifficultySpeedMultiplier;
            }
            else
            {
                SignalShoot();
                ResetShootWaitTime();
            }
        }

        private void MoveGroupDown ()
        {
            var position = m_InvadersHolder.position;
            position.y -= m_DownwardStep;
            m_InvadersHolder.position = position;

            //Change movement direction
            m_MovementDirection.x *= -1.0f;

            //Check for invasion.
            foreach (var row in m_InvadersRows)
            {
                if (!row.HasAvailableInvader)
                {
                    continue;
                }

                if (row.HasInvaderReachedInvasionHeight(m_MaximumInvasionPositionY))
                {
                    Debug.Log("Invaders are here");
                    GameplayManager.Instance.InvadersAreHere();
                    enabled = false;
                }
            }
        }

        private void SignalShoot ()
        {
            var activeRows = m_InvadersRows.Where(row => row.HasAvailableInvader);

            if(activeRows.Count() > 0)
            {
                var row = activeRows.ElementAt(Random.Range(0, activeRows.Count()));

                row.SignalShoot();
            }
        }

        private void OnInvaderKilled (Invader invader)
        {
            if(m_DeathParticle != null && m_DeathParticleCount > 0)
            {
                var emitParams = new ParticleSystem.EmitParams();
                emitParams.position = invader.transform.position;
                emitParams.startColor = invader.Color;
                m_DeathParticle.Emit(emitParams, m_DeathParticleCount);
            }

            IncreaseDifficultyPerInvaderKilled();
        }

        private void OnRowCleared ()
        {
            bool hasInvaderLeft = false;

            foreach(var row in m_InvadersRows)
            {
                if(row.HasAvailableInvader)
                {
                    hasInvaderLeft = true;
                    break;
                }
            }

            if(!hasInvaderLeft)
            {
                EndWave();
            }
        }

        private void EndWave ()
        {
            IncreaseDifficultyPerWaveCleared();
            StartCoroutine(WaitAndResetInvaders());
        }

        private IEnumerator WaitAndResetInvaders ()
        {
            yield return new WaitForSeconds(DELAY_BETWEEN_WAVE_SECONDS);

            m_InvadersHolder.position = m_InvadersHolderStartPosition;

            foreach (var row in m_InvadersRows)
            {
                row.ResetRow();
            }
        }

        private void IncreaseDifficultyPerInvaderKilled ()
        {
            if(m_DifficultyConfig != null)
            {
                m_ProgressionSpeed += m_DifficultyConfig.SpeedIncreasePerInvaderDeath;
            }
        }

        private void IncreaseDifficultyPerWaveCleared()
        {
            if (m_DifficultyConfig != null)
            {
                m_WaveSpeed += m_DifficultyConfig.SpeedIncreasePerWaveCleared;
            }

            m_ProgressionSpeed = 0f;
        }

        private void ResetShootWaitTime ()
        {
            m_ShootWaitTime = Random.Range(m_MinimumShootInterval, m_MaximumShootInteral);
        }
    }
}
