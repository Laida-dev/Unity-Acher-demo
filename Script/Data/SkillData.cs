using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 技能資料控制介面
/// </summary>
public interface ISkillDataCtrl
{
    /// <summary>
    /// 設定技能等級
    /// </summary>
    /// <param name="val">數值</param>
    public void SetLV(int val);
    /// <summary>
    /// 設定技能預設等級
    /// </summary>
    public void SetDefaultLV();
    /// <summary>
    /// 設定技能最大等級
    /// </summary>
    public void SetMaxLV();
    /// <summary>
    /// 技能等級升級
    /// </summary>
    public void LevelUp();
    /// <summary>
    /// 取得當前技能等級
    /// </summary>
    /// <returns>技能等級</returns>
    public int GetLevel();
    /// <summary>
    /// 技能是否已啟用
    /// </summary>
    public bool IsActive();
    /// <summary>
    /// 技能是否滿等
    /// </summary>
    public bool IsLevelMax();
}
/// <summary>
/// 基本技能資料結構：繼承技能控制介面
/// </summary>
public class SkillBaseData : ISkillDataCtrl
{
    /// <summary>
    /// 技能類別名(字串)
    /// </summary>
    public string Name
    {
        get
        {
            return GetType().Name;
        }
    }
    protected int defaultLV;
    protected int maxLV;
    /// <summary>
    /// 當前技能等級
    /// </summary>
    public int LV { get; private set; }

    /// <summary>
    /// 初始化技能預設值/最大值
    /// </summary>
    /// <param name="defaultLV">預設值</param>
    /// <param name="maxLV">最大值</param>
    public virtual void Initial(int defaultLV, int maxLV)
    {
        this.defaultLV = defaultLV;
        this.maxLV = maxLV;
        SetDefaultLV();
        
    }

    public void SetLV(int val)
    {
        LV = val;
    }

    public void LevelUp()
    {
        LV++;
        //當LV超過max值時，回傳max值
        LV = Mathf.Min(LV, maxLV);
        //LV = Mathf.Clamp(LV, defaultLV, maxLV);
    }

    public void SetDefaultLV()
    {
        SetLV(defaultLV);
    }

    public void SetMaxLV()
    {
        SetLV(maxLV);
    }

    public bool IsLevelMax()
    {
        return LV >= maxLV;
    }

    public bool IsActive()
    {
        return LV > 0;
    }

    public int GetLevel()
    {
        return LV;
    }
}

/// <summary>
/// 技能資料
/// </summary>
public static class SkillData
{
    /// <summary>
    /// 技能資料查詢清單(字典)
    /// </summary>
    public static Dictionary<string, SkillBaseData> list { get; private set; }

    public static void Initial()
    {
        //建立查詢清單
        list = new Dictionary<string, SkillBaseData>();
        //新建技能資料暫存
        SkillBaseData data;

        data = new FrontArrow();
        list.Add(data.Name, data);

        data = new DiagonalArrow();
        list.Add(data.Name, data);

        data = new BounceArrow();
        list.Add(data.Name, data);

        data = new TrackingEye();
        list.Add(data.Name, data);


    }
    #region 查詢功能
    public static bool IsMaxLV(string skillName)
    {
        return list[skillName].IsLevelMax();
    }
    public static bool IsMaxLV(SkillBaseData baseData)
    {
        return list[baseData.Name].IsLevelMax();
    }
    public static bool IsOn(string skillName)
    {
        return list[skillName].IsActive();
    }
    public static int GetLV(string skillName)
    {
        return list[skillName].GetLevel();
    }
    #endregion 查詢功能
    #region 輔助功能
    /// <summary>
    /// 基於繼承SkillBaseData的物件反射
    /// </summary>
    public static void SkillDataReflection()
    {
        var skillTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract &&
            type.IsSubclassOf(typeof(SkillBaseData)));

        foreach (var skillType in skillTypes)
        {
            var skill = (SkillBaseData)Activator.CreateInstance(skillType);
            list.Add(skill.Name, skill);
        }
    }
    /// <summary>
    /// 取得還可以升級(未滿級)的技能
    /// </summary>
    /// <returns>技能清單</returns>
    public static List<SkillBaseData> GetUpgradableList()
    {//使用LINQ查詢 => 未滿等技能 > 轉清單
        return list.Values.Where(skill => !skill.IsLevelMax()).ToList();
    }
    /// <summary>
    /// 取得隨機可以升級(未滿級)的技能
    /// </summary>
    /// <returns>技能</returns>
    public static SkillBaseData GetRandomUpgradableSkill()
    {
        int count = GetUpgradableList().Count;//清單物件數量
        int index = Random.Range(0, count);//用總數隨機抽號
        return GetUpgradableList()[index];//返回索引號碼對應物件
    }
    #endregion 輔助功能

    #region 正向箭
    /// <summary>
    /// 正向箭
    /// </summary>
    public class FrontArrow : SkillBaseData
    {
        public FrontArrow()
        {
            Initial(1, 5);
        }
    }
    #endregion 正向箭
    #region 斜向箭
    /// <summary>
    /// 斜向箭
    /// </summary>
    public class DiagonalArrow : SkillBaseData
    {
        public DiagonalArrow()
        {
            Initial(0, 5);
        }
    }
    #endregion 斜向箭

    #region 彈射箭
    public class BounceArrow : SkillBaseData
    {
        public BounceArrow()
        {
            Initial(0, 5);
        }
    }
    #endregion 彈射箭
    #region 追蹤箭
    public class TrackingEye : SkillBaseData
    {
        public TrackingEye()
        {
            Initial(0, 1);
        }
    }
    #endregion 追蹤箭
    #region 穿透箭
    public class Pierce : SkillBaseData
    {
        public Pierce()
        {
            Initial(1, 1);
        }
    }
    #endregion 穿透箭

}
