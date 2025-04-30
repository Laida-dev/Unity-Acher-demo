using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [序列化]子彈基本資訊
/// </summary>
[Serializable]
public struct BulletInfo
{
    public string name;
    public string id;
    /// <summary>
    /// 子彈預製物
    /// </summary>
    public BaseSkillCtrl skillCtrl;
}

/// <summary>
/// [資源選單]子彈物件腳本
/// </summary>
[CreateAssetMenu(fileName = "BulletData", menuName = "GameDB/BulletData", order = 0)]
public class BulletScriptObject : ScriptableObject
{
    /// <summary>
    /// 子彈列表
    /// </summary>
    public List<BulletInfo> bulletList;

    /// <summary>
    /// 用名稱查詢子彈物件
    /// </summary>
    /// <param name="name">名稱</param>
    /// <returns>子彈物件</returns>
    public BaseSkillCtrl SerachByName(string name)
    {
        BaseSkillCtrl skill = null;
        //遍歷子彈清單
        foreach (BulletInfo bullet in bulletList)
        {
            if(bullet.name == name)
            {
                skill = bullet.skillCtrl;
                //如果找到對應物件，立刻中斷
                break;
            }
        }
        return skill;
    }


    /// <summary>
    /// 用編碼查詢子彈物件
    /// </summary>
    /// <param name="name">編碼</param>
    /// <returns>子彈物件</returns>
    public BaseSkillCtrl SerachByID(string id)
    {
        BaseSkillCtrl skill = null;
        //遍歷子彈清單
        foreach (BulletInfo bullet in bulletList)
        {
            if (bullet.id == id)
            {
                skill = bullet.skillCtrl;
                //如果找到對應物件，立刻中斷
                break;
            }
        }
        return skill;
    }
}
