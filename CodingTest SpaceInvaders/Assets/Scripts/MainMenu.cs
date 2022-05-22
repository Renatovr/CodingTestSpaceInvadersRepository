using SpaceInvaders.Score;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles processes of the main menu view
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private InputField m_PlayerNameInput;

    private void Start()
    {
        m_PlayerNameInput.text = AppHelper.PlayerName;
    }

    /// <summary>
    /// Called when we press the play button in the main menu.
    /// </summary>
    public void OnPlayButtonPressed ()
    {
        var inputName = m_PlayerNameInput.text;
        AppHelper.PlayerName = string.IsNullOrEmpty(inputName) ? "Player" : inputName;

        AppHelper.GoToGameScene();
    }

    /// <summary>
    /// Called when we press the leaderboard button in the main menu.
    /// </summary>
    public void OnLeaderboardButtonPressed ()
    {
        ScoreManager.Instance.ShowLeaderboardView();
    }

    /// <summary>
    /// Called when we press the quit button in the main menu.
    /// </summary>
    public void OnQuitButtonPressed ()
    {
        AppHelper.QuitGame();
    }
}
