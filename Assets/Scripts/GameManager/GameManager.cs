using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Example variable to persist between scenes
    public int playerScore = 0;

    private void Awake()
    {
        // Check if an instance of GameManager already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager
        }
    }

    // Method to increase player score (example functionality)
    public void AddScore(int points)
    {
        playerScore += points;
        Debug.Log("Player Score: " + playerScore);
    }
}
