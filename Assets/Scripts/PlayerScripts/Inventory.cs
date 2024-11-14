using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    PlayerActions actions;
    private GameObject temp;
    private Canvas rifle;
    private Canvas shotgun;
    private Canvas rocket;
    // Start is called before the first frame update
    void Start()
    {
        actions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
        temp = GameObject.Find("RifleSprite");
        rifle = temp.GetComponent<Canvas>();
        temp = GameObject.Find("ShotgunSprite");
        shotgun = temp.GetComponent<Canvas>();
        temp = GameObject.Find("RocketSprite");
        rocket = temp.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (actions.hasRifle() == true)
        {
            rifle.enabled = true;
        }
        if (actions.hasShotgun() == true)
        {
            shotgun.enabled = true;
        }
        if (actions.hasRocket() == true)
        {
            rocket.enabled = true;
        }
    }
}
