using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameButtons : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private GameObject pauseMenuUI;    // Reference to the Pause Menu UI
    [SerializeField] private GameObject previousCanvas;  // Reference to the previous UI Canvas (e.g., HUD)

    void Update()
    {
        // Check if the player presses the Escape key to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Method to return to the main menu
    public void MainMenu()
    {
        Time.timeScale = 1f;                // Reset time scale to ensure the game isn't paused
        GameIsPaused = false;               // Reset the game paused state (fixed here)
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    // Method to resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);       // Hide the pause menu
        previousCanvas.SetActive(true);     // Show the previous canvas (e.g., HUD)
        Time.timeScale = 1f;                // Resume the game
        GameIsPaused = false;
    }

    // Method to pause the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);        // Show the pause menu
        previousCanvas.SetActive(false);    // Hide the previous canvas (e.g., HUD)
        Time.timeScale = 0f;                // Pause the game
        GameIsPaused = true;
    }

    // Method to quit the game application
    public void QuitGame()
    {
        Application.Quit();
    }
}
