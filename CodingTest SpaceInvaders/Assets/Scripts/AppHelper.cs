using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Helps with some of the core processes and actions of the app.
/// </summary>
public static class AppHelper
{
    private const string GAMEPLAY_SCENE_NAME = "Game";
    private const string MENU_SCENE_NAME = "Menu";

    /// <summary>
    /// Load the gameplay scene.
    /// </summary>
    public static void GoToGameScene ()
    {
        SceneManager.LoadScene(GAMEPLAY_SCENE_NAME);
    }

    /// <summary>
    /// Load the main menu scene.
    /// </summary>
    public static void GoToMenuScene ()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }

    /// <summary>
    /// Terminate the game.
    /// </summary>
    public static void QuitGame ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
        return;
#endif
        Application.Quit();
    }
}
