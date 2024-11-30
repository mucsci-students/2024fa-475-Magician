using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static bool GameIsPaused = true;

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject settingMenuCanvas;
    [SerializeField] private GameObject pauseMenuUI;    // Reference to the Pause Menu UI
    [SerializeField] private GameObject otherCanvas;  // Reference to the previous UI Canvas (e.g., HUD)

    private void Start()
    {
        mainMenuCanvas.SetActive(true);
        otherCanvas.SetActive(false);
        settingMenuCanvas.SetActive(false);
    }

    void Update()
    {
        // Check if the player presses the Escape key to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (!GameIsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
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
        mainMenuCanvas.SetActive(false);
        settingMenuCanvas.SetActive(false);
        otherCanvas.SetActive(true);
    }

    // Method to quit the game
    public void QuitGame()
    {
        // This will only work in a built version of the game, not in the editor
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void SettingMenu()
    {
        mainMenuCanvas.SetActive(false);
        otherCanvas.SetActive(false);
        pauseMenuUI.SetActive(false);
        settingMenuCanvas.SetActive(true);
    }

    // Reset stats
    public void ResetStats()
    {
        PlayerCollision.healthPackAmount = 0;
        PlayerCollision.shotgunAmmo = 0;
        PlayerCollision.rocketAmmo = 0;
        PlayerCollision.rifleAmmo = 0;
        GameObject _player = GameObject.FindGameObjectWithTag("Player");

       if (_player != null)
        {
            PlayerStats _playerStat = _player.GetComponent<PlayerStats>();
            _playerStat.SetPlayerHealth(200f);

            PlayerActions _playerAction = _player.GetComponent<PlayerActions>();
            _playerAction.SetRifleAqquire(false);
            _playerAction.SetRocketAqquire(false);
            _playerAction.SetShotgunAqquire(false);

            PlayerCollision _playerCollision = _player.GetComponent<PlayerCollision>();
            _playerCollision.ResetInventory();
        }
    }

    // Method to return to the main menu
    public void MainMenu()
    {
        Time.timeScale = 0f;                // Reset time scale to ensure the game isn't paused
        GameIsPaused = true;               // Reset the game paused state (fixed here)
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        mainMenuCanvas.SetActive(true);
        otherCanvas.SetActive(false);
        pauseMenuUI.SetActive(false);
        settingMenuCanvas.SetActive(false);
    }

    // Method to resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);       // Hide the pause menu
        otherCanvas.SetActive(true);     // Show the previous canvas (e.g., HUD)
        Time.timeScale = 1f;                // Resume the game
        GameIsPaused = false;
        mainMenuCanvas.SetActive(false);
        settingMenuCanvas.SetActive(false);
    }

    // Method to pause the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);        // Show the pause menu
        Time.timeScale = 0f;                // Pause the game
        GameIsPaused = true;
        mainMenuCanvas.SetActive(false);
        settingMenuCanvas.SetActive(false);
    }
}
