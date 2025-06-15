using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : MonoBehaviour
{
    private static  UiManager instance = null;

    [Header ("Level UI �� ������ Panel")] 
    #region Level UI �� Panel
    [SerializeField] GameObject levelupPanel;
    [SerializeField] Animator expBarAnimation;
    [SerializeField] Slider currentExpBar;
    [SerializeField] Text currentLevelText;
    [SerializeField] TextMeshProUGUI levelupPanel_playerLevelText;
    readonly private Vector2[] SKILL_BUTTON_POSITION = new Vector2[3] { new Vector2(-7.096107f, 176f), new Vector2(-7.096027f, -34f), new Vector2(-7.096027f, -243f) };
    [SerializeField] private List<Button> skillButtonsList = new List<Button>();
    #endregion

    [Header("ų UI")]
    #region ų UI
    [SerializeField] Text killCountText;
    private int enemyKillCount;
    #endregion

    [Header("�ð� UI")]
    #region �ð� UI
    [SerializeField] Text currentTimeText;
    private int min;
    private float sec;
    #endregion

    [Header("������ Floating Text UI")]
    #region ������ TextUI
    [SerializeField] Camera mainCamera;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] GameObject floatingDamageText;
    #endregion

    [Header("�÷��̾� ü�� UI")]
    #region �÷��̾� ü�� UI
    [SerializeField] Slider playerHPbar;
    #endregion

    [Header("���Ӱ�� UI")]
    #region ���Ӱ�� UI
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text totalSurvivalTimeText;
    [SerializeField] Text totalEnemyKillText;
    [SerializeField] Text lastLeveltext;
    #endregion



    private void Awake()
    {
        SetInstacne();
        expBarAnimation.enabled = false;
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
   

    public static UiManager GetInstacne()
    {
        return instance;
    }

    private void Update()
    {
        Timer();
    }


    private void Timer() // ���ӽð� �ִ� 5������ ����
    {
        sec += Time.deltaTime;
        currentTimeText.text = string.Format("{0:D2}:{1:D2}",min, (int)sec);

        if (sec >= 59)
        {
            sec = 0f;
            min++;
        }
    }

    public void UpdateKillCountText()
    {
        enemyKillCount++;
        killCountText.text = enemyKillCount.ToString();
    }

    public void UpdateHpBar(int dmgValue)
    {
        SoundManager.GetInstacne().PlaySFXSound("HitPlayer", 0.25f);
        playerHPbar.value = (float)dmgValue / 100;
    }

    public void UpdateExpBar(int expValue)
    {
        currentExpBar.value += (float)expValue / GameManager.GetInstacne().GetPlayerMaxExp;
    }

    public void ShowLevelupPanel()
    {
        SoundManager.GetInstacne().PlaySFXSound("LevelUp");
        List<int> tmpOverlapIndex = new List<int>();
        int randomIndex;
        for (int i = 0; i < 3; i++)
        {
            do
            {
               randomIndex = Random.Range(0, skillButtonsList.Count);
            } while(tmpOverlapIndex.Contains(randomIndex));
            tmpOverlapIndex.Add(randomIndex);

            skillButtonsList[randomIndex].GetComponent<RectTransform>().anchoredPosition = SKILL_BUTTON_POSITION[i];
            skillButtonsList[randomIndex].gameObject.SetActive(true);
        }
        currentLevelText.text = GameManager.GetInstacne().GetPlayerCurrentLevel.ToString();
        levelupPanel_playerLevelText.text = currentLevelText.text;
        expBarAnimation.enabled = true;
        levelupPanel.SetActive(true);

        Time.timeScale = 0;
    }

    public void ClickAbilityButton(Button selectSkillButton)
    {
        if (selectSkillButton.gameObject.name.Contains("Activate"))
        {
            skillButtonsList.Remove(selectSkillButton);
            selectSkillButton.gameObject.SetActive(false);
        }
        for (int i = 0; i < skillButtonsList.Count; i++) skillButtonsList[i].gameObject.SetActive(false);

        expBarAnimation.enabled = false;
        levelupPanel.SetActive(false);
        GameManager.GetInstacne().UpgradeSkill(selectSkillButton.name);
        currentExpBar.value = 0;
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        Time.timeScale = 1;
        SoundManager.GetInstacne().PlaySFXSound("MainLobbySFX", 0.35f);
        SceneManager.LoadScene("MainLobbyScene");
    }

    public void CreateDamageText(Vector2 objcetPosition, int damageValue)
    {
        SoundManager.GetInstacne().PlaySFXSound("HitSound", 0.5f);
        Vector2 position = mainCamera.WorldToScreenPoint(objcetPosition);
        GameObject floatingTextObjcet =  Instantiate(floatingDamageText, position, Quaternion.identity, uiCanvas.transform);
        floatingTextObjcet.transform.SetAsFirstSibling();
        floatingTextObjcet.GetComponentInChildren<Text>().text = damageValue.ToString();
    }

    public void GameOverPanel()
    {
        SoundManager.GetInstacne().StopAllAudioSound();
        SoundManager.GetInstacne().PlaySFXSound("GameOver");
        gameOverPanel.SetActive(true);
        totalSurvivalTimeText.text = currentTimeText.text;
        totalEnemyKillText.text = killCountText.text;
        lastLeveltext.text = GameManager.GetInstacne().GetPlayerCurrentLevel.ToString();
        Time.timeScale = 0;
    }
}
