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
            || collision.gameObject.CompareTag("RocketAmmo")
            || collision.gameObject.CompareTag("Shotgun")
            || collision.gameObject.CompareTag("Rifle")
            || collision.gameObject.CompareTag("Rocket"))
        {
            Destroy(collision.gameObject);
            Debug.Log($"Pick up {collision.gameObject.tag}!");
        }
    }
}
