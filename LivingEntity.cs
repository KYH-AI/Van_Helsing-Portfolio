using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LivingEntity: MonoBehaviour, IDamageable
{
    protected int currentHp;
    protected bool dead = false;
    public event Action onDead; // Enemy 전용 onDead 이벤트
   [SerializeField] private GameObject experienceObject;
    public void TakeDamage(int damage) // IDamageable 함수
    {
        currentHp -= damage;
        UiManager.GetInstacne().CreateDamageText(gameObject.transform.position , damage);
        if (currentHp <= 0 && !dead) Dead(); // 현재 체력이 0 이하면 사망처리
    }
    private void Dead()
    {
        StopAllCoroutines();
        dead = true;
        if (onDead != null) // Enemy 전용 onDead 이벤트  (GameManager에서 등록)
        {
            onDead();
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }

    private void EnemyDeadEvent() // 애니메이션에서 호출
    {
        Instantiate(experienceObject, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
