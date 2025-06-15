using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyalBarrierSkill : MonoBehaviour
{
    private static float skillDelayTime = 1f;
    public float SetSkillDelayTime { set { skillDelayTime = skillDelayTime * (1 - ((float)value / 100)); } }

    //private WaitForSeconds skillCoolTime = new WaitForSeconds(skillDelayTime);

    private LayerMask enemyLayerMask = 1 << 10;
    private bool readyToFire = true;

    private int damage = 10;
    public int SetDamage { set { damage += value; } }

    private void Awake()
    {
        skillDelayTime = 1f;
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, 150f * Time.deltaTime);
        if (readyToFire)
        {
            RoyalBarrierHitBoxRange();
        }
    }

    private void RoyalBarrierHitBoxRange()
    {
        Collider2D[] enemyArray = Physics2D.OverlapCircleAll(this.transform.position, 1f, enemyLayerMask);
        if (enemyArray.Length > 0)
        {
            StartCoroutine(RoyalBarrierEnable(enemyArray));
        }
    }

    private IEnumerator RoyalBarrierEnable(Collider2D[] enemyArray)
    {
        readyToFire = false;

        for(int i=0; i < enemyArray.Length; i++)
        {
            Enemy enemy = enemyArray[i].GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.SlowDownCoroutine();
        }

        yield return WaitForSeconds(skillDelayTime);
        readyToFire = true;
    }

    /// <summary>
    /// Caching WaitForSeconds In Unity Coroutines
    /// </summary>
    #region WaitForSeconds Caching
    private readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();
    private WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
    #endregion
}
