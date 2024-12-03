using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] float _playerHealth = 100f;
    [SerializeField] float _meleeDamage = 10f;
    [SerializeField] float _playerMoveSpeed = 1f;

    [Header("Slider")]
    [SerializeField] private Slider _healthBar;

    public static bool _isDead = false;

    private void Start()
    {
        _healthBar.maxValue = _playerHealth;
        _healthBar.value = _playerHealth;
    }

    private void Update()
    {
        _healthBar.value = _playerHealth;
    }

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
        Collider2D _playerCollider = gameObject.GetComponent<Collider2D>();
        _playerCollider.isTrigger = true;

        Rigidbody2D _playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _playerRigidbody.velocity = Vector2.zero;       // Stop linear movement
        _playerRigidbody.isKinematic = true;            // Disable physics simulation

        _isDead = true;
        GetComponent<PlayerInput>().enabled = false;
        // Add death animations, sounds, or other effects here
        _playerAnimator.SetBool("isDead", true);
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
