using UnityEngine;
using System;
using System.Collections;
using SpaceInvaders.Score;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager m_Instance;

    public static GameplayManager Instance
    {
        get
        {
            //First search for GameplayManager in the scene.
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<GameplayManager>();
            }

            //If still none available, create a new one.
            if (m_Instance == null)
            {
                m_Instance = new GameObject(nameof(GameplayManager)).AddComponent<GameplayManager>();
            }

            //Initialize the object if not already initialized
            if(!m_Instance.m_IsInitialized)
            {
                m_Instance.Init();
            }

            return m_Instance;
        }
    }

    /// <summary>
    /// Event fired when the game session is over.
    /// </summary>
    public event Action OnGameSessionEnded;

    /// <summary>
    /// Event fired when the game gets paused.
    /// </summary>
    public event Action OnGamePaused;

    /// <summary>
    /// Event fired when the game gets resumed.
    /// </summary>
    public event Action OnGameResumed;

    [Tooltip("The amount of lives the player should start with.")]
    [SerializeField] private int m_StartingPlayerLives = 3;

    [Tooltip("Interval between player death and respawn.")]
    [SerializeField] private float m_RespawnDelaySeconds = 2.0f;

    private bool m_IsInitialized = false;
    private int m_CurrentPlayerLives;

    private void Init ()
    {
        m_CurrentPlayerLives = m_StartingPlayerLives;
        m_IsInitialized = true;
    }

    /// <summary>
    /// Pause the game session.
    /// </summary>
    public void PauseGame ()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke();
    }

    /// <summary>
    /// Resume the game session.
    /// </summary>
    public void ResumeGame ()
    {
        Time.timeScale = 1.0f;
        OnGameResumed?.Invoke();
    }

    /// <summary>
    /// Terminate the game and return to the main menu
    /// </summary>
    public void ReturnToMenu ()
    {
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Decide on whether the player should respawn based on how many lives left.
    /// </summary>
    /// <param name="respawnAction">Action to execute for respawn</param>
    public void RespawnOrEndGame (Action respawnAction)
    {
        if(m_CurrentPlayerLives > 0)
        {
            m_CurrentPlayerLives--;
            StartCoroutine(WaitAndRunRespawn(respawnAction));
            
            return;
        }
        
        EndGame();
    }

    private void EndGame ()
    {
        Time.timeScale = 0f;
        //Save the accummulated score points
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SaveUserScore();
        }

        OnGameSessionEnded?.Invoke();
    }

    private IEnumerator WaitAndRunRespawn (Action respawnAction)
    {
        yield return new WaitForSeconds(m_RespawnDelaySeconds);

        respawnAction?.Invoke();
    }
}
