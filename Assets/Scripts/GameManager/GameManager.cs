using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Reference to the initial location in the main hub
    private Transform _initialLocation;
    private Transform _respawnPosition;

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

        // Find the initial location and respawn position objects in the scene
        GameObject initialLocationObj = GameObject.Find("InitialPosition");
        GameObject respawnPositionObj = GameObject.Find("RespawnPosition");

        if (initialLocationObj != null)
        {
            _initialLocation = initialLocationObj.transform;
        }
        else
        {
            Debug.LogWarning("InitialLocation object not found in the scene.");
        }

        if (respawnPositionObj != null)
        {
            _respawnPosition = respawnPositionObj.transform;
        }
        else
        {
            Debug.LogWarning("RespawnPosition object not found in the scene.");
        }
    }

    public void MovePlayerToRespawnPosition()
    {
        if (_player != null && _respawnPosition != null)
        {
            _player.transform.position = _respawnPosition.position;
            Debug.Log("Player moved to the respawn position.");
        }
        else
        {
            Debug.LogWarning("Player or respawn position not set.");
        }
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
