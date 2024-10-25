using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] GameObject _gun;
    WeaponStat _weaponStat;
    EnemyStats _enemyStat;
    
    float _gunDamage;

    void Start()
    {
        _weaponStat = _gun.GetComponent<WeaponStat>();
        _enemyStat = gameObject.GetComponent<EnemyStats>();
        _gunDamage = _weaponStat.GetWeaponDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ammo"))
        {
            Destroy(collision.gameObject);
            _enemyStat.TakeDamage(_gunDamage);
        }
    }
}
