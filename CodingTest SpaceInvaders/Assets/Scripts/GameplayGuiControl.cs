using UnityEngine;
using UnityEngine.UI;
using SpaceInvaders.Score;

/// <summary>
/// Controls the HUD UI for gameplay.
/// </summary>
public class GameplayGuiControl : MonoBehaviour
{
    [Tooltip("Reference to the HUD's pause button.")]
    [SerializeField] private Button m_PauseButton;

    [Tooltip("An object to be displayed when the pause button is pressed.")]
    [SerializeField] private GameObject m_PauseMenu;

    [Tooltip("An object to be displayed when the play session is over.")]
    [SerializeField] private GameObject m_GameOverMenu;

    private void Start()
    {
        GameplayManager.Instance.OnGameSessionEnded += OnGameSessionEnded;
    }
    /// <summary>
    /// Method to call when the pause button is pressed.
    /// </summary>
    public void OnPauseButtonPressed ()
    {
        m_PauseButton.gameObject.SetActive(false);
        m_PauseMenu.SetActive(true);
        GameplayManager.Instance.PauseGame();
    }

    /// <summary>
    /// Method to call when the resume button is pressed.
    /// </summary>
    public void OnResumeButtonPressed()
    {
        m_PauseButton.gameObject.SetActive(true);
        m_PauseMenu.SetActive(false);
        GameplayManager.Instance.ResumeGame();
    }

    /// <summary>
    /// Method to call when the exit button is pressed.
    /// </summary>
    public void OnExitButtonPressed()
    {
        m_PauseButton.gameObject.SetActive(false);
        m_PauseMenu.SetActive(false);
        m_GameOverMenu.SetActive(false);

        GameplayManager.Instance.ReturnToMenu();
    }

    /// <summary>
    /// Called when the quit button is pressed.
    /// </summary>
    public void OnQuitButtonPressed ()
    {
        AppHelper.QuitGame();
    }

    /// <summary>
    /// Called when leaderboard button on the exit menu is pressed.
    /// </summary>
    public void OnLeaderboardButtonPressed ()
    {
        ScoreManager.Instance.ShowLeaderboardView();
    }

    private void OnGameSessionEnded ()
    {
        GameplayManager.Instance.OnGameSessionEnded -= OnGameSessionEnded;
        m_PauseButton.gameObject.SetActive(false);
        m_PauseMenu.SetActive(false);
        m_GameOverMenu.SetActive(true);
    }
}
