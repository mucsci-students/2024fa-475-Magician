using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private Animator _enemyAnimator;

    private WeaponStat _weaponStat;
    private EnemyStats _enemyStat;
    private float _gunDamage;
    private bool _isEnemyShot = false;

    void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Find the WeaponStat component in the player's child objects
            _weaponStat = player.GetComponentInChildren<WeaponStat>();
            if (_weaponStat != null)
            {
                _gunDamage = _weaponStat.GetWeaponDamage();
            }
            else
            {
                Debug.LogError("WeaponStat component not found in children of Player");
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }

        // Get the EnemyStats component attached to this enemy
        _enemyStat = GetComponent<EnemyStats>();
        if (_enemyStat == null)
        {
            Debug.LogError("EnemyStats component not found on enemy!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ammo"))
        {
            Destroy(collision.gameObject);
            _enemyStat.TakeDamage(_gunDamage);
            _isEnemyShot = true;
        }
    }

    public bool GetEnemyShot()
    {
        return _isEnemyShot;
    }

    public void SetEnemyShot(bool isEnemyShot)
    {
        _isEnemyShot = isEnemyShot;
    }
}
