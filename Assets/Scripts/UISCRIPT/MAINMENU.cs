using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Method to start the game (load the next scene)
    public void StartGame()
    {
        // Assuming your game scene is at index 1 in the Build Settings
        SceneManager.LoadScene("MainHub");
    }

    // Method to quit the game
    public void QuitGame()
    {
        // This will only work in a built version of the game, not in the editor
        Debug.Log("Quit Game");
        Application.Quit();
    }
}