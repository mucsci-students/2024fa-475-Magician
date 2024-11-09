using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] float _playerHealth = 100f;
    [SerializeField] float _meleeDamage = 10f;
    [SerializeField] float _playerMoveSpeed = 1f;
    bool _isDead = false;

    public void TakeDamage(float damageAmount)
    {
        _playerHealth -= damageAmount;  // Reduce the enemy's health by the damage amount

        if (_playerHealth <= 0)
        {
            Die();  // Call the Die function if health is 0 or less
        }
    }

    private void Die()
    {
        // Add death animations, sounds, or other effects here
        _playerAnimator.SetTrigger("playerDead");
        Collider2D _playerCollider = gameObject.GetComponent<Collider2D>();
        _playerCollider.isTrigger = true;

        Rigidbody2D _playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _playerRigidbody.velocity = Vector2.zero;       // Stop linear movement
        _playerRigidbody.isKinematic = true;            // Disable physics simulation

        _isDead = true;
    }

    public bool GetIsDead()
    {
        return _isDead;
    }

    public void SetIsDead(bool isDead)
    {
        _isDead = isDead;
    }

    public float GetPlayerHealth()
    {
        return _playerHealth;
    }

    public void SetPlayerHealth(float health)
    {
        _playerHealth = health;
    }

    public float GetPlayerMeleeDamage()
    {
        return _meleeDamage;
    }

    public void SetPlayerMeleeDamage(float damage)
    {
        _meleeDamage = damage;
    }

    public float GetPlayerMoveSpeed()
    {
        return _playerMoveSpeed;
    }

    public void SetPlayerMoveSpeed(float speed)
    {
        _playerMoveSpeed = speed;
    }
}
