using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] GameObject _gun;
    [SerializeField] Animator _enemyAnimator;

    WeaponStat _weaponStat;
    EnemyStats _enemyStat;
    GroundEnemyActions _enemyActions;
    float _gunDamage;
    bool _isEnemyShot = false;

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
