using UnityEngine;
using System;
using System.Collections;
using SpaceInvaders.Score;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    private enum GameState
    {
        Starting,
        Playing,
        Paused,
        Ended
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

    [Tooltip("The amount of extra lives the player should start with.")]
    [SerializeField] private int m_StartingExtraPlayerLives = 3;

    [Tooltip("Interval between player death and respawn.")]
    [SerializeField] private float m_RespawnDelaySeconds = 2.0f;

    private int m_CurrentPlayerLives;
    private GameState m_GameState;

    private void Awake()
    {
        if(Instance == null)
        {
            //Set this as the instance.
            Instance = this;
            Init();
        }
        else if (Instance != null && Instance != this)
        {
            //If another instance already exists.
            Destroy(gameObject);
        }
    }

    private void Init ()
    {
        m_CurrentPlayerLives = m_StartingExtraPlayerLives;
        ScoreManager.Instance.ShowGameScoreView();
        m_GameState = GameState.Playing;
    }

    /// <summary>
    /// Pause the game session.
    /// </summary>
    public void PauseGame ()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke();
        m_GameState = GameState.Paused;
    }

    /// <summary>
    /// Resume the game session.
    /// </summary>
    public void ResumeGame ()
    {
        Time.timeScale = 1.0f;
        OnGameResumed?.Invoke();
        m_GameState = GameState.Playing;
    }

    /// <summary>
    /// Terminate the game and return to the main menu
    /// </summary>
    public void ReturnToMenu ()
    {
        Time.timeScale = 1.0f;
        ScoreManager.Instance.HideAllViews();
        AppHelper.GoToMenuScene();
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
        if(m_GameState == GameState.Ended)
        {
            //We already ended.
            return;
        }

        m_GameState = GameState.Ended;
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
