using UnityEngine;

public enum ItemType
{
    Money,
    Exp,
}

public class ItemCtrl : MonoBehaviour
{
    /// <summary>
    /// 物品類型
    /// </summary>
    public ItemType type;
    private Rigidbody _rb;
    private Rigidbody rb
    {
        get
        { 
            if (_rb == null) _rb = GetComponent<Rigidbody>();
            return _rb; 
        }
    }
    private bool isGround;
    public static bool autoPick;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>(); // 確保 Rigidbody 先被初始化
    }
    void Start()
    {
         rb.AddForce(Vector3.up * 200f);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }
    void Update()
    {

        if (isGround && autoPick)
        {
            transform.LookAt(GameMaster.playerPos + Vector3.up);
            transform.position = Vector3.Lerp(transform.position,
                GameMaster.playerPos, Time.deltaTime * 6);
            if (Vector3.Distance(transform.position, GameMaster.playerPos + Vector3.up) < 1.5f)
            {
                //取得對應數值更新
                UpdateData();
                //依照銷毀物件類型增加對應數據
                
                Destroy(gameObject);
            }
        }
    }
    void UpdateData()
    {
        switch(type)
        {
            case ItemType.Money:
                GameData.GoldCtrl(10);
                break;
            case ItemType.Exp:
                GameData.CombatExpCtrl(10);
                break;

        }
    }
}
