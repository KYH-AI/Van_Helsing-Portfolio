using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicGrenadeSkill : MonoBehaviour
{
    private static float skillDelayTime = 3f;
    public float SetSkillDelayTime { set { skillDelayTime = skillDelayTime * (1 - ((float)value / 100)); } }

  //  private WaitForSeconds skillCoolTime = new WaitForSeconds(skillDelayTime);
    [SerializeField] private GameObject magicGrenadeObject;
    private LayerMask enemyLayerMask = 1 << 10;
    private bool readyToFire = true;


    private int damage = 10;
    public int SetDamage { set { damage += value; } }

    private int projectileCount = 1;
    public int SetProjectileCount { set { projectileCount += value; } }

    private void Awake()
    {
        skillDelayTime = 3f;
        StopAllCoroutines();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (readyToFire)
        {
            MagicGrenadeAttackRange();
        }
    }

    private void MagicGrenadeAttackRange()
    {
        Collider2D[] enemyArray = Physics2D.OverlapCircleAll(this.transform.position, 5f, enemyLayerMask);
        if (enemyArray.Length > 0)
        {
            StartCoroutine(CreateMagicGrenade(enemyArray));
        }
    }

    private IEnumerator CreateMagicGrenade(Collider2D[] enemyArray)
    {
        readyToFire = false;
        Vector2 dir;
        Vector3 maigcGrandeSpawnOffset = new Vector3(-3.5f, 7.4f, 0f) + transform.position;
        SoundManager.GetInstacne().PlaySFXSound("MagicGrenade", 0.15f);

        for (int i = 0; i < projectileCount; i++)
        {
            try
            {
                dir = enemyArray[i].transform.position - maigcGrandeSpawnOffset;
            }
            catch
            {
                break;
            }
          
            GameObject magicGrandeObj = Instantiate(magicGrenadeObject, maigcGrandeSpawnOffset, Quaternion.identity);
            magicGrandeObj.GetComponent<MagicGrenade>().SetMagicGrenadeData(damage, dir.normalized * 5f, enemyArray[i].transform);

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
