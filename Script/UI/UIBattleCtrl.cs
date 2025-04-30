using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleCtrl : MonoBehaviour
{
    public TextMeshProUGUI textGold;
    public TextMeshProUGUI textGoldTotal;
    public TextMeshProUGUI textRewardExp;
    public TextMeshProUGUI textLV;
    public TextMeshProUGUI textSpeed;
    public GameObject rectExp;
    public CanvasGroup skillCG;
    public CanvasGroup gameoverCG;
    public List<UIChooseSkillCtrl> skillBtns;
    


    private bool isPause;
    private bool isSpeedUp;

    void Start()
    {
        //註冊更新功能 & 初始化
        GameData.GoldRest(GoldUpdate);
        GameData.RewardExpRest(RewardExpUpdate);
        GameData.LevelRest(LevelUpdate);
        GameData.CombatExpRest(CombatExpUpdate);
        GameData.GameOver(GameOverPanelSwitch);
        
        
    }
    #region 按鈕功能
    /// <summary>
    /// 遊戲暫停(遊戲選單)
    /// </summary>
    public void GamePause()
    {
        isPause = !isPause;
        if (isPause) GameMaster.GamePause();
        else if (isSpeedUp) GameMaster.GameSpeedCtrl(2);
        else GameMaster.DefautGameSpeed();
    }
    public void GameResume()
    {
        isPause = false;
        if (isPause) GameMaster.GamePause();
        else if (isSpeedUp) GameMaster.GameSpeedCtrl(2);
        else GameMaster.DefautGameSpeed();
        SkillPanelSwitch(false);

    }
    /// <summary>
    /// 遊戲速度調整(1~2倍)
    /// </summary>
    public void SpeedUp()
    {
        if (isPause) return;//暫停狀態下禁止變更速度
        isSpeedUp = !isSpeedUp;
        textSpeed.text = isSpeedUp ? "x2" : "x1";
        if (isSpeedUp) GameMaster.GameSpeedCtrl(2);
        else GameMaster.DefautGameSpeed();
    }
    /// <summary>
    /// 關閉關卡
    /// </summary>
    public void CloseStage()
    {
        GameData.ClearReward();
        GameMaster.UnloadGameUI();
        GameMaster.GoBackMainMenu();
    }
    #endregion 按鈕功能
    #region 託管更新功能
    /// <summary>
    /// 戰利金UI更新功能(託管)
    /// </summary>
    /// <param name="str">文字內容</param>
    void GoldUpdate(string str)
    {
        textGold.text = str;
      textGoldTotal.text = str;
    }
    /// <summary>
    /// 戰鬥獎勵經驗UI更新功能(託管)
    /// </summary>
    /// <param name="str">文字內容</param>
    void RewardExpUpdate(string str)
    {
       textRewardExp.text = str;
    }
    /// <summary>
    /// 戰鬥等級UI更新功能(託管)
    /// </summary>
    /// <param name="str">文字內容</param>
    void LevelUpdate(string str)
    {
        textLV.text = str;
        SkillPanelSwitch(true);
        GamePause();
    }
    /// <summary>
    /// 戰鬥經驗UI更新功能(託管)
    /// </summary>
    /// <param name="val">百分比</param>
    void CombatExpUpdate(float val)
    {
        rectExp.GetComponent<Image>().fillAmount = val;
    }
    #endregion 託管更新功能
    #region 技能面板功能
    /// <summary>
    /// 選擇技能面板開關
    /// </summary>
    void SkillPanelSwitch()
    {
        skillCG.blocksRaycasts = !skillCG.blocksRaycasts;
        skillCG.alpha = skillCG.blocksRaycasts ? 1 : 0;
        if (skillCG.blocksRaycasts) SkillButtonShowUp();
    }

    void SkillPanelSwitch(bool B)
    {
        skillCG.blocksRaycasts = B;
        skillCG.alpha = B ? 1 : 0;
        if (B) SkillButtonShowUp();
    }
    void GameOverPanelSwitch(bool B)
    {
        gameoverCG.blocksRaycasts = B;
        gameoverCG.alpha = B ? 1 : 0;

    }

   public GameMaster.SoltsCount solts;
   void SkillButtonShowUp()
    {
        UIChooseSkillCtrl.ClearGotSkillNames();
        for (int i = 0; i < 3; i++)
        {
            skillBtns[i].SetActive(i < solts.GetCount());
        }
    }
    #endregion 技能面板功能
}
