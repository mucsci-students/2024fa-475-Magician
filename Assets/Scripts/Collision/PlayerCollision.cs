using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject rocket;

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

        PlayerActions playerAction = GetComponentInChildren<PlayerActions>();

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
}
