using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [Header("플레이어 경험치")]
    #region 플레이어 경험치 관련
    private int playerMaxExp = 70;
    public int GetPlayerMaxExp { get { return playerMaxExp; } }

    private int playerCurrentExp = 0;

    private int playerCurrentLevel = 1;
    public int GetPlayerCurrentLevel { get { return playerCurrentLevel; } }
    #endregion
    
    [Header("적 Wave")]
    #region 적 Wave 관련
    [SerializeField] private List<EnemyWaveData> enemyWaves;
    private EnemyWaveData currentWave;
    [SerializeField] private Player playerObject;
    [SerializeField] private Vector3 spawnArea;
    [SerializeField] private int currentWaveNumber;
    private float spawnTime = 0;
    private const float enemyWaveSpawnTime = 30f;
    #endregion

    [Header("플레이어 스킬")]
    #region 플레이어 스킬 관련
    [SerializeField] List<GameObject> playerSkillList;
    /* 0 = Whip, 1 = MagicArrow, 2 = MagicGrenade, 3 = IceLaser, 4 = RoyalBarrier */
    #endregion

    [System.Serializable]
    private class EnemyWaveData
    {
        public List<EnemyWave> enemyWaveList;

        [System.Serializable]
        public class EnemyWave
        {
            public int enemyCount;
            public float spawnDelayTime;
            public GameObject enemyPrefab;
        }
    }

    private void Awake()
    {
       SetInstacne();
       StopAllCoroutines();
       SoundManager.GetInstacne().StopAllAudioSound();
       SoundManager.GetInstacne().PlayBGMSound("InGameBGM");
    }
    private void SetInstacne()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static GameManager GetInstacne()
    {
        return instance;
    }

    private void Start()
    {
        spawnTime = enemyWaveSpawnTime;
        SpawnWave();
    }

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        if(spawnTime < 0)
        {
            spawnTime = enemyWaveSpawnTime;
            currentWaveNumber++;
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        currentWave = enemyWaves[currentWaveNumber];
        StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        WaitForSeconds spawnDelay;
        for (int wave = 0; wave < currentWave.enemyWaveList.Count; wave++)  // EnemyWave List 길이 만큼 반복
        {
            spawnDelay = new WaitForSeconds(currentWave.enemyWaveList[wave].spawnDelayTime); // 각 웨이브에 정해진 스폰딜레이 값 지정
               
            for (int spawnEnemyCount = 0; spawnEnemyCount < currentWave.enemyWaveList[wave].enemyCount; spawnEnemyCount++) // EnemyWave 구조체에 들어있는 EnemyCount 만큼 반복 생성
            {
                Vector3 enemyRespawnPos = new Vector3(Random.Range(-spawnArea.x, spawnArea.x),
                                                      Random.Range(-spawnArea.y, spawnArea.y),
                                                      0f);
                enemyRespawnPos += playerObject.GetPlayerObjectPosition().position;
                yield return spawnDelay;

                Enemy enemy = Instantiate(currentWave.enemyWaveList[wave].enemyPrefab, enemyRespawnPos, Quaternion.identity).GetComponent<Enemy>();
                enemy.onDead += () => UiManager.GetInstacne().UpdateKillCountText();
                enemy.onDead += () => enemy.DeadAnimation();
                enemy.SetTargetObject = playerObject.GetPlayerObjectPosition();
            }
        }
    }

    public void UpgradeSkill(string skillName)
    {
        switch (skillName)
        {
            case "WhipActivate": playerSkillList[0].gameObject.SetActive(true); break;
            case "MagicArrowActivate": playerSkillList[1].gameObject.SetActive(true); break;
            case "MagicGrenadeActivate": playerSkillList[2].gameObject.SetActive(true); break;
            case "IceLaserActivate": playerSkillList[3].gameObject.SetActive(true); break;
            case "RoyalBarrierActivate": playerSkillList[4].gameObject.SetActive(true); break;

            case "WhipDamageUpgrade": playerSkillList[0].gameObject.GetComponent<WhipAttackSkill>().SetDamage = 7; break;
            case "MagicArrowDamageUpgrade": playerSkillList[1].gameObject.GetComponent<MagicArrowSkill>().SetDamage = 5; break;
            case "MagicGrenadeDamageUpgrade": playerSkillList[2].gameObject.GetComponent<MagicGrenadeSkill>().SetDamage = 5; break;
            case "IceLaserDamageUpgrade": playerSkillList[3].gameObject.GetComponent<IceLaserSkill>().SetDamage = 10; break;
            case "RoyalBarrierDamageUpgrade": playerSkillList[4].gameObject.GetComponent<RoyalBarrierSkill>().SetDamage = 2; break;

            case "WhipCoolTimeUpgrade": playerSkillList[0].gameObject.GetComponent<WhipAttackSkill>().SetSkillDelayTime = 10; break;
            case "MagicArrowCoolTimeUpgrade": playerSkillList[1].gameObject.GetComponent<MagicArrowSkill>().SetSkillDelayTime = 10; break;  
            case "MagicGrenadeCoolTimeUpgrade": playerSkillList[2].gameObject.GetComponent<MagicGrenadeSkill>().SetSkillDelayTime= 10; break;
            case "IceLaserCoolTimeUpgrade": playerSkillList[3].gameObject.GetComponent<IceLaserSkill>().SetSkillDelayTime = 10; break;
            case "RoyalBarrierCoolTimeUpgrade": playerSkillList[4].gameObject.GetComponent<RoyalBarrierSkill>().SetSkillDelayTime = 3; break;

            case "MagicArrowProjectileUpgrade": playerSkillList[1].gameObject.GetComponent<MagicArrowSkill>().SetProjectileCount = 1; break;
            case "MagicGrenadeProjectileUpgrade": playerSkillList[2].gameObject.GetComponent<MagicGrenadeSkill>().SetProjectileCount = 1; break;
            case "IceLaserProjectileUpgrade": playerSkillList[3].gameObject.GetComponent<IceLaserSkill>().SetProjectileCount = 1; break;

                /* 0 = Whip, 1 = MagicArrow, 2 = MagicGrenade, 3 = IceLaser, 4 = RoyalBarrier */
        }
    }

    public void PlayerTakeExp(int expValue)
    {
        SoundManager.GetInstacne().PlaySFXSound("TakeExp" , 0.15f);
        playerCurrentExp += expValue;
        UiManager.GetInstacne().UpdateExpBar(expValue);

        if (playerCurrentExp >= playerMaxExp)
        {
            playerCurrentLevel++;
            playerCurrentExp = 0;
            playerMaxExp += 30;
            UiManager.GetInstacne().ShowLevelupPanel();
        }
    }


}
