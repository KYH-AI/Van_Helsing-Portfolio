using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipAttackSkill : MonoBehaviour
{
    [SerializeField] GameObject whipWeapon;
    private int damage = 10;
    public int SetDamage { set { damage += value; } }

    LayerMask enemyLayerMask = 1 << 10;

    private const float WHIP_X_HITBOX_SIZE = 2f, WHIP_Y_HITBOX_SIZE = 1f;
    private Vector2 whipWeaponHitBoxSize = new Vector2(WHIP_X_HITBOX_SIZE, WHIP_Y_HITBOX_SIZE);

    private static float skillDelayTime = 2.5f;
    public float SetSkillDelayTime { set { skillDelayTime = skillDelayTime * (1 - ((float)value / 100)); } } 
 //   private WaitForSeconds skillCoolTime = new WaitForSeconds(skillDelayTime);
    private bool readyToFire = true;

    private void Awake()
    {
        skillDelayTime = 2.5f;
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if(readyToFire)
        {
            whipWeapon.SetActive(false);
            StartCoroutine(WhipAttack());
        }
    }


    private IEnumerator WhipAttack()
    {
        readyToFire = false;
        whipWeapon.SetActive(true);

        SoundManager.GetInstacne().PlaySFXSound("Whip");
        Collider2D[] whipWeaponHitBox = Physics2D.OverlapBoxAll(whipWeapon.transform.position, whipWeaponHitBoxSize, 0f, enemyLayerMask);
        
        if(whipWeaponHitBox.Length > 0)
        {     
            for(int i = 0; i < whipWeaponHitBox.Length; i++)
            {
                Enemy enemy = whipWeaponHitBox[i].gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
                enemy.SlowDownCoroutine();
            }
        }

        yield return WaitForSeconds(skillDelayTime);

        readyToFire = true;
         
    }

    /// <summary>
    /// Caching WaitForSeconds In Unity Coroutines
    /// </summary>
    #region WaitForSeconds Caching
    private readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();
    private  WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
    #endregion
}
