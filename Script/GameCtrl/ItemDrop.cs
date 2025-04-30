using UnityEngine;
using System.Collections.Generic;
public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private ItemScriptableObject itemDB;
    [SerializeField]
    private int moneyAmount = 1;
    [SerializeField]
    private int expAmount = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Drop()
    {
        if (itemDB != null)
        {
            foreach (DropItemInfo drop in itemDB.ItemList)
            {
                switch(drop.name)
                {
                    case ItemType.Money:
                        CreatDropItem(drop.itemCtrl, moneyAmount);
                        break;
                    case ItemType.Exp:
                        CreatDropItem(drop.itemCtrl, expAmount);
                        break;
                }    
            }
        }
    }
    /// <summary>
    /// 生成掉落物件
    /// </summary>
    /// <param name="item">掉落物件</param>
    /// <param name="amount">數量</param>
    private void CreatDropItem(ItemCtrl item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // 產生隨機方向 (確保是由中心向外)
            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.y = Mathf.Abs(randomDirection.y); // 確保物體有向上的分量

            // 計算生成位置 (稍微偏離中心，避免完全重疊)
            Vector3 pos = randomDirection * Random.Range(0.2f, 1.0f);

            // 生成道具
            ItemCtrl spawnedItem = Instantiate(item, transform.position + pos, Quaternion.identity);

            // 取得 Rigidbody
            Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 施加一個從中心向外的力
                rb.AddForce(randomDirection * Random.Range(200f, 400f));

                // 給予隨機旋轉，讓物品更自然
                rb.AddTorque(Random.insideUnitSphere * Random.Range(10f, 50f));
            }
        }
    }


}
