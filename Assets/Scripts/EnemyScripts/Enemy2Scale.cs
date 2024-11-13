using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy2Scale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene current = SceneManager.GetActiveScene();
        string sceneName = current.name;
        if (sceneName == "Level1" || sceneName == "Level2")
        {
            gameObject.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        }
        else if (sceneName == "MainGame")
        {
            gameObject.transform.localScale = new Vector3(6, 6, 6);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
