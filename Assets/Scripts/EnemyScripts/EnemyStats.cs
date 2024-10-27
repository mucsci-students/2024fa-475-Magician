using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] Animator _enemyAnimator;
    [SerializeField] float _enemyHealth = 50f;
    [SerializeField] float _enemyDamage = 10f;
    [SerializeField] float _enemyMoveSpeed = 1.5f;
    [SerializeField] GameObject[] _itemList; 
    bool _isDead = false;
    bool _isDropItem = false;
    float _destroyTime = 10f;

    /**
    * Called when the enemy takes damage. Reduces health and destroys the enemy if health falls below 0.
    */
    public void TakeDamage(float damageAmount)
    {
        _enemyHealth -= damageAmount;  // Reduce the enemy's health by the damage amount

        if (_enemyHealth <= 0)
        {
            Die();  // Call the Die function if health is 0 or less
        }
    }

    /**
    * Called when the enemy's health reaches 0. Handles enemy death.
    */
    private void Die()
    {
        // Add death animations, sounds, or other effects here
        _enemyAnimator.SetTrigger("enemyDead");
        Collider2D _enemyCollider = gameObject.GetComponent<Collider2D>();
        _enemyCollider.enabled = false;

        Rigidbody2D _enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _enemyRigidbody.velocity = Vector2.zero;       // Stop linear movement
        _enemyRigidbody.isKinematic = true;            // Disable physics simulation
        if (!_isDropItem)
        {
            GameObject coin = Instantiate(_itemList[0], gameObject.transform.position, Quaternion.identity);
            _isDropItem = true;
        }

        _isDead = true;
        Invoke("DestroyWrapper", _destroyTime);
    }

    public void DestroyWrapper()
    {
        Destroy(gameObject);
    }

    public bool GetIsDead()
    {
        return _isDead; 
    }

    public void SetIsDead(bool isDead)
    {
        _isDead = isDead;
    }

    public float GetEnemySpeed()
    {
        return _enemyMoveSpeed;
    }

    public void SetEnemySpeed(float speed)
    {
        _enemyMoveSpeed = speed;
    }

    public float GetEnemyHealth()
    {
        return _enemyHealth;
    }

    public void SetEnemyHealth(float health)
    {
        _enemyHealth = health;
    }

    public float GetEnemyDamage()
    {
        return _enemyDamage;
    }

    public void SetEnemyDamage(float damage)
    {
        _enemyDamage = damage;
    }
}
