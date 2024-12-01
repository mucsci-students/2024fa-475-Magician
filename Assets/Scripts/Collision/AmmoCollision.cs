using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || 
            collision.gameObject.CompareTag("SmallEnemy") ||
            collision.gameObject.CompareTag("MediumEnemy") ||
            collision.gameObject.CompareTag("BigEnemy") ||
            collision.gameObject.CompareTag("AnimalEnemy") ||
            collision.gameObject.CompareTag("Boss1") ||
            collision.gameObject.CompareTag("Boss2") ||
            collision.gameObject.CompareTag("Boss3"))
        {
            if (gameObject.CompareTag("RocketBullet"))
            {
                Animator bulletAnimation = GetComponent<Animator>();
                bulletAnimation.SetTrigger("rocketCollision");
                Invoke("DestroyObjectWrapper", 0.3f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


    void DestroyObjectWrapper()
    {
        Destroy(gameObject);
    }
}
