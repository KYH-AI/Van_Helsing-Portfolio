using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LivingEntity: MonoBehaviour, IDamageable
{
    protected int currentHp;
    protected bool dead = false;
    public event Action onDead; // Enemy ���� onDead �̺�Ʈ
   [SerializeField] private GameObject experienceObject;
    public void TakeDamage(int damage) // IDamageable �Լ�
    {
        currentHp -= damage;
        UiManager.GetInstacne().CreateDamageText(gameObject.transform.position , damage);
        if (currentHp <= 0 && !dead) Dead(); // ���� ü���� 0 ���ϸ� ���ó��
    }
    private void Dead()
    {
        StopAllCoroutines();
        dead = true;
        if (onDead != null) // Enemy ���� onDead �̺�Ʈ  (GameManager���� ���)
        {
            onDead();
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }

    private void EnemyDeadEvent() // �ִϸ��̼ǿ��� ȣ��
    {
        Instantiate(experienceObject, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
