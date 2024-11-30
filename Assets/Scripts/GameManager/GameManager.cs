using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject _bulletinBoard;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _mainHubPosition;
    [SerializeField] private GameObject _firstRespawn;
    [SerializeField] private GameObject _secondRespawn;
    [SerializeField] private GameObject _thirdRespawn;

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
        AudioManager.Instance.PlayThemeMusic("ThemeAudio");
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainHub")
        {
            _bulletinBoard.SetActive(true);
            Debug.Log("In Main Hub!!!!");
        }
        else
        {
            _bulletinBoard.SetActive(false);
        }
    }

    public void MovePlayerToRespawnPosition(int level)
    {
        if (level == 2)
        {
            _player.transform.position = _firstRespawn.transform.position;
        }
        else if (level == 3) 
        {
            _player.transform.position = _secondRespawn.transform.position;
        }
        else if (level == 4)
        {
            _player.transform.position = _thirdRespawn.transform.position;
        }
    }

    public void MovePlayerToHub()
    {
        _player.transform.position = _mainHubPosition.transform.position;
    }
}
