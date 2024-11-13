using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScaleScript : MonoBehaviour
{
    PlayerStats stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        Scene current = SceneManager.GetActiveScene();
        string sceneName = current.name;
        if (sceneName == "Level1" || sceneName == "Level2")
        {
            gameObject.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
            stats.SetPlayerMoveSpeed(3);
        } else if (sceneName == "MainGame")
        {
            gameObject.transform.localScale = new Vector3(7, 7, 7);
            stats.SetPlayerMoveSpeed(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
