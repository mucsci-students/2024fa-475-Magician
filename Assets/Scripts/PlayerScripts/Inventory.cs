using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Active Icon")]
    [SerializeField] private GameObject _slot1;
    [SerializeField] private GameObject _slot2;
    [SerializeField] private GameObject _slot3;
    [SerializeField] private GameObject _slot4;
    [SerializeField] private GameObject _slot5;

    PlayerActions actions;
    PlayerStats playerStats;
    GameObject healFXObj;
    Animator healFXAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _slot2.SetActive(true);
        // Find the "Player" GameObject first
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            actions = player.GetComponent<PlayerActions>();
            playerStats = actions.GetComponent<PlayerStats>();
            // Then, search for the "HealFx" child within the player's hierarchy
            healFXObj = player.transform.Find("HealFx").gameObject;

            if (healFXObj != null)
            {
                healFXAnimator = healFXObj.GetComponent<Animator>();
            }
            else
            {
                Debug.LogError("HealFx object not found as a child of Player!");
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GunSwitch();
    }

    void GunSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Number 2 key pressed!");
            if(actions.hasPistol() == true)
            {
                actions.setPistol(true);
                actions.setRifle(false);
                actions.setShotgun(false);
                actions.setRocket(false);
                SwitchActive(KeyCode.Alpha2);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Number 3 key pressed!");
            if (actions.hasShotgun() == true)
            {
                actions.setShotgun(true);
                actions.setRifle(false);
                actions.setRocket(false);
                actions.setPistol(false);
                SwitchActive(KeyCode.Alpha3);
            }
        }

        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Number 4 key pressed!");
            if (actions.hasRocket() == true)
            {
                actions.setRocket(true);
                actions.setRifle(false);
                actions.setShotgun(false);
                actions.setPistol(false);
                SwitchActive(KeyCode.Alpha4);
            }
        }

        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Number 5 key pressed!");
            if (actions.hasRifle() == true)
            {
                actions.setRifle(true);
                actions.setShotgun(false);
                actions.setPistol(false);
                actions.setRocket(false);
                SwitchActive(KeyCode.Alpha5);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Number 1 key pressed!");
            if(PlayerCollision.healthPackAmount > 0)
            {
                healFXObj.SetActive(true);
                float currentHealth = playerStats.GetPlayerHealth();
                playerStats.SetPlayerHealth(currentHealth + 15f);
                --PlayerCollision.healthPackAmount;
                healFXAnimator.SetBool("isHeal", true);
                Invoke("ResetHealAnimationWrapper", 1f);
                SwitchActive(KeyCode.Alpha1);
            }
        }
    }

    void SwitchActive(KeyCode key)
    {
        if (key == KeyCode.Alpha1)
        {
            _slot1.SetActive(true);
            Invoke("TurnOffHealthSlotActive", 0.1f);
        }
        else if (key == KeyCode.Alpha2)
        {
            _slot2.SetActive(true);
            _slot3.SetActive(false);
            _slot4.SetActive(false);
            _slot5.SetActive(false);
        }
        else if (key == KeyCode.Alpha3)
        {
            _slot2.SetActive(false);
            _slot3.SetActive(true);
            _slot4.SetActive(false);
            _slot5.SetActive(false);
        }
        else if (key == KeyCode.Alpha4)
        {
            _slot2.SetActive(false);
            _slot3.SetActive(false);
            _slot4.SetActive(true);
            _slot5.SetActive(false);
        }
        else if (key == KeyCode.Alpha5)
        {
            _slot2.SetActive(false);
            _slot3.SetActive(false);
            _slot4.SetActive(false);
            _slot5.SetActive(true);
        }
    }

    void TurnOffHealthSlotActive()
    {
        _slot1.SetActive(false);
    }

    void ResetHealAnimationWrapper()
    {
        healFXAnimator.SetBool("isHeal", false);
        healFXObj.SetActive(false);
    }
}
