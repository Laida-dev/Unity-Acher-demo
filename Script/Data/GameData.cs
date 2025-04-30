using System;
using UnityEngine;

/// <summary>
/// 遊戲資訊中心
/// [靜態]遊戲資料暫存(全局靜態資料)
/// </summary>
public static class GameData
{
    #region GameInfo資料區
    /// <summary>
    /// 遊戲能量資訊[公開讀取｜內部設定]
    /// </summary>
    public static int energy { get; private set; }
    /// <summary>
    /// 遊戲能量最大值資訊
    /// </summary>
    public const int energyMax = 30;
    /// <summary>
    /// 遊戲能量百分比
    /// </summary>
    public static float energyPercent
    {
        get
        {
            return (float)energy / energyMax;
        }
    }
    
    /// <summary>
    /// 能量是否滿載
    /// </summary>
    public static bool energyFull
    {
        get
        {
            return energy >= energyMax;
        }
    }
    private static float energyTimer;
    private static float energyTimeFreq =1f;


    /// <summary>
    /// 遊戲能量資訊UI更新託管
    /// </summary>
    public static Action<string> energyUpdateEvent;
    public static void EnergyCtrl(int val)
    {
        //先判斷 增/減
        if (val > 0)
        {
            energy += val;
        }
        else if (val < 0)
        {
            if (energy >= Mathf.Abs(val)) energy += val;
            else Debug.LogWarning("沒能量");
        }
        //更新UI($格式化string)
        energyUpdateEvent?.Invoke($"{energy}/{energyMax}");
    }
    /// <summary>
    /// 能量回復的功能
    /// </summary>
    /// <param name="time"></param>
    public static void EnergyRecovery(float time)
    {
        if (energyFull) return;//能量回滿阻擋以下程式
        energyTimer += time;
        if (energyTimer >= energyTimeFreq)
        {//計時循環
            energyTimer = 0;
            EnergyCtrl(1);
        }
    }

    /// <summary>
    /// 遊戲金錢資訊[公開讀取｜內部設定]
    /// </summary>
    public static int money { get; private set; }
    /// <summary>
    /// 遊戲金錢資訊UI更新託管
    /// </summary>
    public static Action<string> moneyUpdateEvent;
    /// <summary>
    /// 遊戲金錢控制功能
    /// </summary>
    /// <param name="val">增/減金額</param>
    public static void MoneyCtrl(int val)
    {
        //先判斷 增/減
        if(val > 0)
        {
            money += val;
        }
        else if (val < 0)
        {
            //先檢查是否夠扣
            //if((money + val) >= 0) money += val;
            if (money >= Mathf.Abs(val)) money += val;
            else Debug.LogWarning("金幣不夠了");
        }
        //更新UI
        moneyUpdateEvent?.Invoke(money.ToString());
    }

    /// <summary>
    /// 遊戲鑽石資訊[公開讀取｜內部設定]
    /// </summary>
    public static int diamond { get; private set; }
    /// <summary>
    /// 遊戲鑽石資訊UI更新託管
    /// </summary>
    public static Action<string> diamondUpdateEvent;
    /// <summary>
    /// 遊戲鑽石控制功能
    /// </summary>
    /// <param name="val">增/減鑽石數量</param>
    public static void DiamondCtrl(int val)
    {
        //先判斷 增/減
        if (val > 0)
        {
            diamond += val;
        }
        else if (val < 0)
        {
            //先檢查是否夠扣
            //if((diamond + val) >= 0) diamond += val;
            if (diamond >= Mathf.Abs(val)) diamond += val;
            else Debug.LogWarning("鑽石不夠了");
        }
        //更新UI
        diamondUpdateEvent?.Invoke(diamond.ToString());
    }
    /// <summary>
    /// 帳號等級
    /// </summary>
    public static int accLV = 1;
    /// <summary>
    /// 經驗資訊[公開讀取｜內部設定]
    /// </summary>
    public static int exp { get; private set; }
    /// <summary>
    /// 當前等級經驗
    /// </summary>
    public static int currentExp
    {
        get
        {
            return exp - CompleteExpTotal();
        }
    }
    private static int CompleteExpTotal()
    {
        int total = 0;
        for (int i = 1; i < accLV; i++)
        {
            total += i * 100;
        }
        return total;
    }
    /// <summary>
    /// 當前等級所需經驗
    /// </summary>
    private static float expMax
    {
        get
        {
            return accLV * 100f;
        }
    }
    /// <summary>
    /// 當級所需經驗百分比
    /// </summary>
    private static float expPercent
    {
        get
        {
            return currentExp / expMax;
        }
    }
    private static int rewardExp;
    public static Action<string> addRewardExpEvent;
    public static void RewardExpCtrl(int val)
    {
        rewardExp += val;
        addRewardExpEvent?.Invoke(rewardExp.ToString());
    }
    /// <summary>
    /// 清除(結算)獎勵
    /// </summary>
    public static void ClearReward()
    {
        ExpCtrl(rewardExp);
        rewardExp = 0;
        MoneyCtrl(gold);
        gold = 0;
    }
    /// <summary>
    /// 經驗獎勵功能託管
    /// </summary>
    /// <param name="action">功能託管</param>
    public static void RewardExpRest(Action<string> action)
    {
        rewardExp = 0;
        addRewardExpEvent = action;
        addRewardExpEvent?.Invoke(rewardExp.ToString());
    }
    /// <summary>
    /// 經驗資訊UI更新託管
    /// </summary>
    public static Action<float, string> expUpdateEvent;
    /// <summary>
    /// 經驗控制功能
    /// </summary>
    /// <param name="val">增加經驗</param>
    public static void ExpCtrl(int val)
    {
        exp += val;
        while (expPercent >= 1)
        {//連發版的 if
            accLV++;
        }
        //更新UI
        expUpdateEvent?.Invoke(expPercent, accLV.ToString());
    }
    /// <summary>
    /// 設置經驗UI託管
    /// </summary>
    /// <param name="action">更新託管</param>
    public static void SetExpUI(Action<float, string> action)
    {
        expUpdateEvent = action;
        expUpdateEvent?.Invoke(expPercent, accLV.ToString());
    }
    #endregion GameInfo資料區
    #region Battle資料區
    /// <summary>
    /// 戰利金資訊[公開讀取｜內部設定]
    /// </summary>
    public static int gold { get; private set; }
    /// <summary>
    /// 戰利金資訊UI更新託管
    /// </summary>
    public static Action<string> goldUpdateEvent;
    /// <summary>
    /// 戰利金控制功能
    /// </summary>
    /// <param name="val">增加戰利金</param>
    public static void GoldCtrl(int val)
    {
        gold += val;
        //更新UI
        goldUpdateEvent?.Invoke(gold.ToString());
    }
    public static void GoldRest(Action<string> action)
    {
        gold = 0;
        goldUpdateEvent = action;
        goldUpdateEvent?.Invoke(gold.ToString());
    }

    /// <summary>
    /// 戰鬥等級資訊[公開讀取｜內部設定]
    /// </summary>
    public static int LV { get; private set; }
    /// <summary>
    /// 戰鬥等級資訊UI更新託管
    /// </summary>
    public static Action<string> levelUpdateEvent;
    /// <summary>
    /// 戰鬥等級控制功能
    /// </summary>
    /// <param name="val">增加等級</param>
    public static void LevelUp()
    {
        LV++;
        //更新UI
        levelUpdateEvent?.Invoke($"LV.{LV}");
    }
    public static void LevelRest(Action<string> action)
    {
        LV = 1;
        levelUpdateEvent = action;
        levelUpdateEvent?.Invoke($"LV.{LV}");
    }

    /// <summary>
    /// 戰鬥經驗資訊[公開讀取｜內部設定]
    /// </summary>
    public static int combatExp { get; private set; }
    private static float combatExpMax
    {
        get
        {
            return LV * 50f;
        }
    }
    private static float combatExpPercent
    {
        get
        {
            return combatExp / combatExpMax;
        }
    }
    /// <summary>
    /// 戰鬥經驗資訊UI更新託管
    /// </summary>
    public static Action<float> combatExpUpdateEvent;
    /// <summary>
    /// 戰鬥經驗控制功能
    /// </summary>
    /// <param name="val">增加經驗</param>
    public static void CombatExpCtrl(int val)
    {
        combatExp += val;
        if (combatExpPercent >= 1)
        {//達成升級條件
            combatExp = combatExp - (int)combatExpMax;
            LevelUp();
        }
        //更新UI
        combatExpUpdateEvent?.Invoke(combatExpPercent);
    }
    /// <summary>
    /// 戰鬥經驗重設功能
    /// </summary>
    /// <param name="action">更新託管</param>
    public static void CombatExpRest(Action<float> action)
    {
        combatExp = 0;
        combatExpUpdateEvent = action;
        combatExpUpdateEvent?.Invoke(combatExpPercent);
    }

    /// <summary>
    /// 戰鬥結束UI更新託管
    /// </summary>
    public static Action<bool> gameoverEvent;
    /// <summary>
    /// 戰鬥結束功能
    /// </summary>
    /// <param name="action">更新託管</param>
    public static void GameOver(Action<bool> action)
    {
        gameoverEvent = action;
    }
    public static void GameOverCtrl()
    {
        //不需要執行升級後的遊戲相關行為
        levelUpdateEvent = null;
        //獎勵結算
        gameoverEvent?.Invoke(true);
    }
    #endregion Battle資料區
}
