using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
}