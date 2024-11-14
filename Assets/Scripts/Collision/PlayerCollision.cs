using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this to use UI components

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject rocket;
    PlayerActions playerAction;

    [SerializeField] GameObject popUpPanel; // Reference to the Pop-Up Panel
    [SerializeField] TMP_Text popUpText;        // Reference to the Text component of the Pop-Up Panel

    void Start()
    {
        playerAction = GetComponentInChildren<PlayerActions>();
        popUpPanel.SetActive(false); // Ensure the Pop-Up is hidden at the start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin")
            || collision.gameObject.CompareTag("HealthPack")
            || collision.gameObject.CompareTag("PistolAmmo")
            || collision.gameObject.CompareTag("RifleAmmo")
            || collision.gameObject.CompareTag("ShotgunAmmo")
            || collision.gameObject.CompareTag("SniperAmmo")
            || collision.gameObject.CompareTag("RocketAmmo"))
        {
            Destroy(collision.gameObject);
            Debug.Log($"Pick up {collision.gameObject.tag}!");
        }

        if (collision.gameObject.CompareTag("Level1"))
        {
            SceneManager.LoadScene("Map1");
        }

        if (collision.gameObject.CompareTag("Level2") && playerAction.hasShotgun())
        {
            SceneManager.LoadScene("Map2");
        }
        else if (collision.gameObject.CompareTag("Level2"))
        {
            ShowPopUp("You must complete Level 1 in the North Gate first.");
        }

        if (collision.gameObject.CompareTag("Level3") && playerAction.hasRocket())
        {
            SceneManager.LoadScene("Map3");
        }
        else if (collision.gameObject.CompareTag("Level3"))
        {
            ShowPopUp("You must complete Level 2 in the West Gate first.");
        }

        if (collision.gameObject.CompareTag("Shotgun"))
        {
            // Enable the Shotgun object in the Canvas
            shotgun.SetActive(true);
            playerAction.SetShotgunAqquire(true);
            Destroy(collision.gameObject);
            Debug.Log($"Picked up {collision.gameObject.tag}!");
        }
        else if (collision.gameObject.CompareTag("Rifle"))
        {
            // Enable the Rifle object in the Canvas
            rifle.SetActive(true);
            playerAction.SetRifleAqquire(true);
            Destroy(collision.gameObject);
            Debug.Log($"Picked up {collision.gameObject.tag}!");
        }
        else if (collision.gameObject.CompareTag("Rocket"))
        {
            // Enable the Rocket object in the Canvas
            rocket.SetActive(true);
            playerAction.SetRocketAqquire(true);
            Destroy(collision.gameObject);
            Debug.Log($"Picked up {collision.gameObject.tag}!");
        }
    }

    // Method to show the pop-up message
    private void ShowPopUp(string message)
    {
        popUpText.text = message;  // Set the warning message
        popUpPanel.SetActive(true); // Show the Pop-Up Panel
        Invoke("HidePopUp", 3f);   // Automatically hide the Pop-Up after 3 seconds
    }

    // Method to hide the pop-up message
    private void HidePopUp()
    {
        popUpPanel.SetActive(false); // Hide the Pop-Up Panel
    }
}
