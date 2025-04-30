using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class UIChooseSkillCtrl : MonoBehaviour
{
    #region 共用資料區
    /// <summary>
    /// 已抽取的備選技能名稱
    /// </summary>
    private static List<string> gotSkillNames = new List<string>();
    /// <summary>
    /// 清除以記錄的備選技能名稱
    /// </summary>
    public static void ClearGotSkillNames()
    {
        gotSkillNames.Clear();
    }
    /// <summary>
    /// 加入已抽到的備選技能名稱
    /// </summary>
    /// <param name="skillName">技能名稱</param>
    public static void AddGotSkillName(string skillName)
    {
        gotSkillNames.Add(skillName);
    }
    /// <summary>
    /// 檢查是否已抽取過的備選技能名稱
    /// </summary>
    /// <param name="skillName">技能名稱</param>
    /// <returns>是/否</returns>
    public static bool AlreadyInGotSkillNames(string skillName)
    {
        return gotSkillNames.Contains(skillName);
    }
    #endregion 共用資料區

    [SerializeField]
    private SkillIconScriptObject iconDB;
    private SkillBaseData skillData;
    private Image _image;
    private Image image
    {
        get
        {
            if (_image == null) _image = GetComponent<Image>();
            return _image;
        }
    }
    private TextMeshProUGUI _textMesh;
    private TextMeshProUGUI textMesh
    {
        get
        {
            if (_textMesh == null) _textMesh = GetComponentInChildren<TextMeshProUGUI>();
            return _textMesh;
        }
    }
    /// <summary>
    /// 抽取技能(更新UI圖示)
    /// </summary>
    public void GetSkill()
    {
        skillData = SkillData.GetRandomUpgradableSkill();
        if (!AlreadyInGotSkillNames(skillData.Name))
        {
            image.sprite = iconDB.SerachByName(skillData.Name);
            textMesh.text = skillData.Name.SplitByUpperCase();
            AddGotSkillName(skillData.Name);//成功抽取
        }
        else GetSkill();//抽到重覆技能，觸發遞迴
    }
    /// <summary>
    /// 選擇技能(For UI 被按下)
    /// </summary>
    public void ChooseSkill()
    {
        skillData?.LevelUp();
    }
    /// <summary>
    /// 設定是否啟用此選項
    /// </summary>
    /// <param name="B">是/否</param>
    public void SetActive(bool B)
    {
        gameObject.SetActive(B);
        if (B) GetSkill();
    }
}
