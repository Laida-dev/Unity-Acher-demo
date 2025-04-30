using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ArcherGame;

/// <summary>
/// [系統序列化]遊戲用資訊資料結構
/// </summary>
[Serializable]
public struct GameInfo
{
    public TextMeshProUGUI uiText;
    public Action updateEvent;

    /// <summary>
    /// 接收信息的方法接口
    /// </summary>
    /// <param name="strMsg">信息</param>
    public void Update(string strMsg)
    {
        uiText.text = strMsg;
        //附帶事件不為空就呼叫
        updateEvent?.Invoke();
        //if(updateEvent != null) updateEvent();
    }
}

public class UIPlayerInfoCtrl : MonoBehaviour
{
    [Header("遊戲資料結構")]
    public Image imgExp;
    public TextMeshProUGUI textLV;
    public GameInfo energyInfo;
    public Image EnergyBar;    
    [SerializeField]
    private float energyRecoveryRate;
    public GameInfo moneyInfo;
    public GameInfo diamondInfo;

    //[Header("玩家資訊")]

    /// <summary>
    /// 腳本啟動時會執行：一般用於註冊事件
    /// </summary>
    private void OnEnable()
    {
        GameData.SetExpUI(UpdateExpBarLV);
        //能量BAR的更新功能綁定於資訊更新事件
        energyInfo.updateEvent = UpdateEnergyBar;
        GameData.energyUpdateEvent += energyInfo.Update;
        //訂閱金錢參數資料
        GameData.moneyUpdateEvent += moneyInfo.Update;
        GameData.diamondUpdateEvent += diamondInfo.Update;
    }

    /// <summary>
    /// 腳本關閉時會執行：一般用於取消註冊事件
    /// </summary>
    private void OnDisable()
    {
        GameData.SetExpUI(null);
        GameData.energyUpdateEvent -= energyInfo.Update;
        //取消訂閱金錢參數資料
        GameData.moneyUpdateEvent -= moneyInfo.Update;
        GameData.diamondUpdateEvent -= diamondInfo.Update;
    }

    //初始化
    void Start()
    {
        GameData.ExpCtrl(0);
        //更新能量相關UI內容
        GameData.EnergyCtrl(0);
        //更新金錢相關UI內容
        GameData.MoneyCtrl(0);//資料變化
        //更新鑽石相關UI內容
        GameData.DiamondCtrl(0);//資料變化
    }

    private void Update()
    {
        GameData.EnergyRecovery(Time.deltaTime* energyRecoveryRate);
    }

    public void StartGame()
    {
        if (GameData.energy >= 5)
        {
            GameData.EnergyCtrl(-5);
            LoadScenes.Loading();
        }       
    }

    /// <summary>
    /// 更新能量BAR長度
    /// </summary>
    void UpdateEnergyBar()
    {
        if (EnergyBar != null)
        {
            EnergyBar.fillAmount = GameData.energyPercent;
        }
    }
    /// <summary>
    /// 更新Exp長度
    /// </summary>
    void UpdateExpBarLV(float expPercent,string accLV)
    {
        if (imgExp != null)
        {
            imgExp.fillAmount = expPercent;
            textLV.text = accLV;
        }
    }
}
