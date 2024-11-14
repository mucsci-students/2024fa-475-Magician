using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Reference to the initial location in the main hub
    [SerializeField] Transform _initialLocation;

    // Reference to the player object
    private GameObject _player;

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

    private void Start()
    {
        // Find the player object in the scene
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void MovePlayerToHub()
    {
        if (_player != null && _initialLocation != null)
        {
            _player.transform.position = _initialLocation.position;
            Debug.Log("Player moved to the main hub location.");
        }
        else
        {
            Debug.LogWarning("Player or initial location not set.");
        }
    }
}
