using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] Animator _enemyAnimator;
    [SerializeField] float _enemyHealth = 50f;
    [SerializeField] float _enemyDamage = 10f;
    [SerializeField] float _enemyMoveSpeed = 1.5f;
    [SerializeField] private GameObject[] _itemList;  // List of item prefabs
    [SerializeField] private float[] _dropRates;      // Corresponding drop rates for each item
    [SerializeField] private float _dropRadius = 2f;  // Radius within which items will be scattered

    private Dictionary<GameObject, float> itemDropRateMap;
    bool _isDead = false;
    float _destroyTime = 10f;

    void Start()
    {
        InitializeDropRateMap();  // Initialize drop rates
    }

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
        // Trigger death animation
        _enemyAnimator.SetTrigger("enemyDead");

        // Disable enemy collider
        Collider2D _enemyCollider = gameObject.GetComponent<Collider2D>();
        _enemyCollider.enabled = false;

        // Stop enemy movement and disable physics
        Rigidbody2D _enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _enemyRigidbody.velocity = Vector2.zero;
        _enemyRigidbody.isKinematic = true;

        if (!_isDead)
        {
            TryDropItems(transform.position);  // Drop items at the enemy's position
        }

        _isDead = true;
        Invoke("DestroyWrapper", _destroyTime);  // Destroy the enemy after a delay
    }

    /**
    * Initializes the drop rate map using the item list and drop rates.
    */
    private void InitializeDropRateMap()
    {
        itemDropRateMap = new Dictionary<GameObject, float>();

        if (_itemList.Length != _dropRates.Length)
        {
            Debug.LogError("Item list and drop rates list must have the same length.");
            return;
        }

        // Populate the dictionary
        for (int i = 0; i < _itemList.Length; i++)
        {
            itemDropRateMap[_itemList[i]] = _dropRates[i];
        }
    }

    /**
    * Attempts to drop items based on their drop rates and scatter them around the drop position.
    */
    private void TryDropItems(Vector3 dropPosition)
    {
        List<GameObject> droppedItems = new List<GameObject>();

        // Loop through all items and apply drop logic
        for (int i = 0; i < _itemList.Length; i++)
        {
            GameObject item = _itemList[i];
            float dropRate = _dropRates[i];

            // Ensure items with a 100% drop rate always drop
            if (dropRate >= 100f)
            {
                droppedItems.Add(item);
            }
            else
            {
                // Attempt to drop items with a lower drop rate
                float randomValue = Random.Range(0f, 100f);
                if (randomValue <= dropRate)
                {
                    droppedItems.Add(item);
                }
            }
        }

        // Instantiate all dropped items with random scatter around the drop position
        foreach (GameObject itemPrefab in droppedItems)
        {
            // Generate a random point within a circle, using Random.insideUnitCircle
            Vector2 randomOffset = Random.insideUnitCircle * _dropRadius;

            // Convert the 2D random offset to a 3D position by adding it to the drop position
            Vector3 randomDropPosition = dropPosition + new Vector3(randomOffset.x, randomOffset.y, 0f); // Assuming your game uses the XY plane

            // Instantiate the item at the random position
            GameObject item = Instantiate(itemPrefab, randomDropPosition, Quaternion.identity);

            // Get the Animator component from the item and trigger the corresponding animation
            Animator itemAnimator = item.GetComponent<Animator>();
            if (itemAnimator != null)
            {
                if (item.CompareTag("Coin")) { itemAnimator.SetTrigger("isCoinDropped"); }
                else if (item.CompareTag("HealthPack")) { itemAnimator.SetTrigger("isHealthPackDropped"); }
                else if (item.CompareTag("PistolAmmo")) { itemAnimator.SetTrigger("isPistolAmmoDropped"); }
                else if (item.CompareTag("RifleAmmo")) { itemAnimator.SetTrigger("isRifleAmmoDropped"); }
                else if (item.CompareTag("ShotgunAmmo")) { itemAnimator.SetTrigger("isShotgunAmmoDropped"); }
                else if (item.CompareTag("SniperAmmo")) { itemAnimator.SetTrigger("isSniperAmmoDropped"); }
            }
        }

    }

    /**
    * Destroys the enemy game object.
    */
    private void DestroyWrapper()
    {
        Destroy(gameObject);
    }

    // Getter and setter methods for enemy properties
    public bool GetIsDead() => _isDead;
    public void SetIsDead(bool isDead) => _isDead = isDead;
    public float GetEnemySpeed() => _enemyMoveSpeed;
    public void SetEnemySpeed(float speed) => _enemyMoveSpeed = speed;
    public float GetEnemyHealth() => _enemyHealth;
    public void SetEnemyHealth(float health) => _enemyHealth = health;
    public float GetEnemyDamage() => _enemyDamage;
    public void SetEnemyDamage(float damage) => _enemyDamage = damage;
}
