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
    [SerializeField] private GameObject healthPack;
    const int maxhealthPackAmount = 10;
    const int maxshotgunAmmo = 80;
    const int maxrocketAmmo = 12;
    const int maxrifleAmmo = 250;

    public static int healthPackAmount = 0;
    public static int shotgunAmmo = 0;
    public static int rocketAmmo = 0;
    public static int rifleAmmo = 0;
    PlayerActions playerAction;

    [SerializeField] GameObject popUpPanel; // Reference to the Pop-Up Panel
    [SerializeField] TMP_Text popUpText;        // Reference to the Text component of the Pop-Up Panel
    [SerializeField] TMP_Text shotgunAmmoText;
    [SerializeField] TMP_Text rocketAmmoText;
    [SerializeField] TMP_Text rifleAmmoText;
    [SerializeField] TMP_Text healthPackText;

    void Start()
    {
        UpdateUI();
        playerAction = GetComponentInChildren<PlayerActions>();
        popUpPanel.SetActive(false); // Ensure the Pop-Up is hidden at the start
    }

    void Update()
    {
        if (healthPackAmount == 0)
            healthPack.SetActive(false);
        UpdateUI();
    }

    // Method to update the UI text
    private void UpdateUI()
    {
        shotgunAmmoText.text = shotgunAmmo + " / " + maxshotgunAmmo;
        rocketAmmoText.text = rocketAmmo + " / " + maxrocketAmmo;
        rifleAmmoText.text = rifleAmmo + " / " + maxrifleAmmo;
        healthPackText.text = healthPackAmount + " / " + maxhealthPackAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ShotgunAmmo"))
        {
            shotgun.SetActive(true);
            Destroy(collision.gameObject);
            shotgunAmmo += 20;
            Debug.Log($"Pick up {collision.gameObject.tag}!");
        }

        if (collision.gameObject.CompareTag("RocketAmmo"))
        {
            rocket.SetActive(true);
            Destroy(collision.gameObject);
            rocketAmmo += 2;
            Debug.Log($"Pick up {collision.gameObject.tag}!");
        }

        if (collision.gameObject.CompareTag("RifleAmmo"))
        {
            rifle.SetActive(true);
            Destroy(collision.gameObject);
            rifleAmmo += 50;
            Debug.Log($"Pick up {collision.gameObject.tag}!");
        }

        if (collision.gameObject.CompareTag("HealthPack"))
        {
            healthPack.SetActive(true);
            Destroy(collision.gameObject);
            ++healthPackAmount;
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
            // Move the player to the hub location
            GameManager.Instance.MovePlayerToHub();
            SceneManager.LoadScene("MainHub");
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
            // Move the player to the hub location
            GameManager.Instance.MovePlayerToHub();
            SceneManager.LoadScene("MainHub");
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
