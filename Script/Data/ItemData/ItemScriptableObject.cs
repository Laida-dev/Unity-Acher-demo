using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// [序列化]掉落物基本資訊
/// </summary>
[Serializable]
public struct DropItemInfo
{
    public ItemType name;
    public string id;
    /// <summary>
    /// 掉落物預製物
    /// </summary>
    public ItemCtrl itemCtrl;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "GameDB/ItemData", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<DropItemInfo> itemList = new List<DropItemInfo>();
    public List<DropItemInfo> ItemList {  get { return itemList; } }

    

    /// <summary>
    /// 用名稱查詢掉落物件
    /// </summary>
    /// <param name="name">名稱</param>
    /// <returns>掉落物件</returns>
    public ItemCtrl SerachByName(ItemType name)
    {
        ItemCtrl item = null;
        //遍歷道具清單
        foreach (DropItemInfo drop in itemList)
        {
            if (drop.name == name)
            {
                item = drop.itemCtrl;
                //如果找到對應物件，立刻中斷
                break;
            }
        }
        return item;
    }
}
