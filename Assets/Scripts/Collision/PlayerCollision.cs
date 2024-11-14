using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
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
            playerAction.SetShotgunAqquire(true);
            Destroy(collision.gameObject);
            Debug.Log($"Pick up {collision.gameObject.tag}!");
        }
        else if (collision.gameObject.CompareTag("Rifle"))
        {
            playerAction.SetRifleAqquire(true);
            Destroy(collision.gameObject);
            Debug.Log($"Pick up {collision.gameObject.tag}!");
        }
        else if (collision.gameObject.CompareTag("Rocket"))
        {
            playerAction.SetRocketAqquire(true);
            Destroy(collision.gameObject);
            Debug.Log($"Pick up {collision.gameObject.tag}!");
        }
    }
}
