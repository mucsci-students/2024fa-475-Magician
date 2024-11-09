using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy4Scale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene current = SceneManager.GetActiveScene();
        string sceneName = current.name;
        if (sceneName == "Level1" || sceneName == "Level2")
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (sceneName == "MainGame")
        {
            gameObject.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
