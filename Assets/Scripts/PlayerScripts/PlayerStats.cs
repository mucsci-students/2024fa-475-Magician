using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] float _playerHealth = 100f;
    [SerializeField] float _meleeDamage = 10f;
    [SerializeField] float _playerMoveSpeed = 1f;

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

        // Destroy the enemy object
        //Destroy(gameObject);
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
