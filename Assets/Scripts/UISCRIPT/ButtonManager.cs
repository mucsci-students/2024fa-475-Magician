using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static bool GameIsPaused = true;
    Scene _currentScene;

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

    // Method to start the game (load the next scene)
    public void StartGame()
    {
        GameObject manager = GameObject.Find("GameManager");
        if (manager)
        {
            ResetStats();
            // Move the player to the hub location
            GameManager.Instance.MovePlayerToHub();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainHub");
        GameIsPaused = false;
    }

    // Method to quit the game
    public void QuitGame()
    {
        // This will only work in a built version of the game, not in the editor
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    // Reset stats
    public void ResetStats()
    {
        PlayerCollision.healthPackAmount = 0;
        PlayerCollision.shotgunAmmo = 0;
        PlayerCollision.rocketAmmo = 0;
        PlayerCollision.rifleAmmo = 0;
        GameObject _player = GameObject.FindGameObjectWithTag("Player");

        PlayerStats _playerStat = _player.GetComponent<PlayerStats>();
        _playerStat.SetPlayerHealth(200f);

        PlayerActions _playerAction = _player.GetComponent<PlayerActions>();
        _playerAction.SetRifleAqquire(false);
        _playerAction.SetRocketAqquire(false);
        _playerAction.SetShotgunAqquire(false);

        PlayerCollision _playerCollision = _player.GetComponent<PlayerCollision>();
        _playerCollision.ResetInventory();
    }

    // Method to return to the main menu
    public void MainMenu()
    {
        Time.timeScale = 0f;                // Reset time scale to ensure the game isn't paused
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
}
