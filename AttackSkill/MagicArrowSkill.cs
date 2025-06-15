using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrowSkill : MonoBehaviour
{
    private static float skillDelayTime = 1.5f;
    public float SetSkillDelayTime { set { skillDelayTime = skillDelayTime * (1 - ((float)value / 100)); } }

    //private WaitForSeconds skillCoolTime = new WaitForSeconds(skillDelayTime);

    [SerializeField] private GameObject magicArrowObject;
    private LayerMask enemyLayerMask = 1 << 10;
    private bool readyToFire = true;

    private int damage = 10;
    public int SetDamage { set { damage += value; } }

    private int projectileCount = 1;
    public int SetProjectileCount {  set {  projectileCount += value; } }

    private void Awake()
    {
        skillDelayTime = 1.5f;
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (readyToFire)
        {
            MagicArrowAttackRange();
        }
    }

    private void MagicArrowAttackRange()
    {
        List<Transform> enemyList = new List<Transform>();
        Collider2D[] enemyArray = Physics2D.OverlapCircleAll(this.transform.position, 5f, enemyLayerMask);
        if (enemyArray.Length > 0)
        {
            float minDistance = 100;
            for (int i = 0; i < enemyArray.Length; i++)
            {
                float distance = Vector2.Distance(transform.position, enemyArray[i].transform.position);
                if (distance < minDistance)
                {
                    enemyList.Add(enemyArray[i].transform);
                    minDistance = distance;
                }

            }
            if (enemyList != null)
            {
                StartCoroutine(CreateMagicArrow(enemyList));
            }
        }
    }


    private IEnumerator CreateMagicArrow(List<Transform> enemyPositionList)
    {

        readyToFire = false;
        Vector2 dir;
        float angle;
        Quaternion angleAxis;
        SoundManager.GetInstacne().PlaySFXSound("MagicArrow");
        for (int i=0; i< projectileCount; i++)
        {
            try
            {
                dir = enemyPositionList[i].position - transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            catch
            {
                break;
            }

            GameObject magicArrow = Instantiate(magicArrowObject, transform.position, angleAxis);
            magicArrow.GetComponent<MagicArrow>().SetMagicArrowData(damage, dir.normalized * 5f);
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
