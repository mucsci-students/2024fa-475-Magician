using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene current = SceneManager.GetActiveScene();
        string sceneName = current.name;
        if (sceneName == "Level1" || sceneName == "Level2")
        {
            gameObject.transform.localScale = new Vector3(2, 2, 2);
        }
        else if (sceneName == "MainGame")
        {
            gameObject.transform.localScale = new Vector3(7, 7, 7);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
