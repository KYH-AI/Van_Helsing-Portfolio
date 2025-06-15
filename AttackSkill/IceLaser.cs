using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLaser : MonoBehaviour
{
    private int damage;
    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    public void SetLaserData(int dmg)
    {
        damage = dmg;
    }


    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Enemy"))
        {
            Enemy enemy = target.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.SlowDownCoroutine();
        }
    }
}
