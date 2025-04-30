using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [序列化]掉落物基本資訊
/// </summary>
[Serializable]
public struct SkillIconInfo
{
    public string name;
    /// <summary>
    /// 技能圖示
    /// </summary>
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "SkillIcon", menuName = "GameDB/SkillIcon", order = 3)]
public class SkillIconScriptObject : ScriptableObject
{
    [SerializeField]
    private List<SkillIconInfo> iconList = new List<SkillIconInfo>();

    /// <summary>
    /// 用名稱查詢圖示
    /// </summary>
    /// <param name="name">名稱</param>
    /// <returns>圖示</returns>
    public Sprite SerachByName(string name)
    {
        Sprite sprite = null;
        //遍歷掉落物清單
        foreach (SkillIconInfo icon in iconList)
        {
            if (icon.name == name)
            {
                sprite = icon.sprite;
                //如果找到對應物件，立刻中斷
                break;
            }
        }
        return sprite;
    }
}
