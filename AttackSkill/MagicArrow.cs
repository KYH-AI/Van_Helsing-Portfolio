using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrow : MonoBehaviour
{
    [SerializeField] Rigidbody2D magicArrowRigidBody;
    private int damage;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }
    public void SetMagicArrowData(int dmg, Vector2 enemyDir)
    {
        
        damage = dmg;
        magicArrowRigidBody.velocity = enemyDir;
    }


    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Enemy"))
        {
            Enemy enemy = target.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.SlowDownCoroutine();

            Destroy(gameObject);
        }
    }
}
