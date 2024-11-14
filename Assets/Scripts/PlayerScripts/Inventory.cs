using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    PlayerActions actions;
    PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        actions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
        playerStats = actions.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        GunSwitch();
    }

    void GunSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Number 1 key pressed!");
            if(actions.hasPistol() == true)
            {
                actions.setPistol(true);
                actions.setRifle(false);
                actions.setShotgun(false);
                actions.setRocket(false);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Number 2 key pressed!");
            if (actions.hasShotgun() == true)
            {
                actions.setShotgun(true);
                actions.setRifle(false);
                actions.setRocket(false);
                actions.setPistol(false);
            }
        }

        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Number 3 key pressed!");
            if (actions.hasRocket() == true)
            {
                actions.setRocket(true);
                actions.setRifle(false);
                actions.setShotgun(false);
                actions.setPistol(false);
            }
        }

        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Number 4 key pressed!");
            if (actions.hasRifle() == true)
            {
                actions.setRifle(true);
                actions.setShotgun(false);
                actions.setPistol(false);
                actions.setRocket(false);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Number 5 key pressed!");
            if(PlayerCollision.healthPackAmount > 0)
            {
                float currentHealth = playerStats.GetPlayerHealth();
                playerStats.SetPlayerHealth(currentHealth + 15f);
                --PlayerCollision.healthPackAmount;
            }
        }
    }
}
