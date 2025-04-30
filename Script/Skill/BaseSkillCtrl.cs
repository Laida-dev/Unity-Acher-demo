using UnityEditor;
using UnityEngine;

public class BaseSkillCtrl : MonoBehaviour
{
    public enum TargetType
    {
        None,
        Player,
        Monster,
    }
    
    [Header("基本參數")]
    public TargetType targetType;
    private string targetTag
    {
        get { return targetType.ToString(); }
    }
    [Range(0f,20f)]
    public float flySpeed = 5f;
    public float lifetime = 3f;
    public int damage = 10;
    private float fixedY;
    [Header("特效")]
    public GameObject hitEffect;    
    #region 障礙物處理
    private bool isPierce ;   
    private bool isPenetrated = false; // 記錄是否已經穿透目標
    #endregion 障礙物處理
    #region 追蹤參數
    private BaseCharacterCtrl hitTarget;
    private BaseCharacterCtrl trackTarget;
    private float turnSpeed = 5f;
    private float turnAcceleration = 5f;
    private float maxTurnSpeed = 30f; // 限制最大角速度
    private bool isTracking
    {
        get
        {
            return targetType == TargetType.Monster ?
                SkillData.IsMaxLV("TrackingEye") : false;
        }
    }
    #endregion 追蹤參數
    #region 彈射參數
    /// <summary>
    /// 彈射等級
    /// </summary>
    private int bounceLV
    {
        get
        {
            return targetType == TargetType.Monster?
                SkillData.GetLV("BounceArrow") : 0;
        }
    }
    /// <summary>
    /// 彈射次數
    /// </summary>
    private int bounceCount;
    /// <summary>
    /// 還是否可以彈射
    /// </summary>
    private bool isBounce
    {
        get
        {//彈射等級 > 0 並且 彈跳次數還有剩餘
            return bounceLV > 0 && bounceCount <= bounceLV;
        }
    }
    #endregion 彈射參數
    #region 子彈參數
    private int frontArrowAmount 
    { 
        get
        {
            return targetType == TargetType.Monster ?
                SkillData.GetLV("FrontArrow"): 0;
        } 
    }
    private int diagonalArrowAmount
    {
        get
        {
            return targetType == TargetType.Monster ?
                SkillData.GetLV("DiagonalArrow") : 0;
        }
    }
    #endregion 子彈參數


    void Start()
    {
        fixedY = 1;              
        DeleteObject(lifetime);
        if (isTracking) GetNearestMonster();
    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
        //物件往特定方向位移
        transform.Translate(Vector3.forward * Time.deltaTime * flySpeed*5);
        
        if (isTracking) Tracking();//追蹤箭
        
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == targetTag)
        {//撞擊指定標籤物件
           hitTarget = other.gameObject.GetComponent<BaseCharacterCtrl>();
           hitTarget?.TakeDamage(damage);
           if (hitEffect) Instantiate(hitEffect, transform.position, transform.rotation);
           if (isBounce) Bounce();
           if (isPierce) isPenetrated = true;
           else if(!isPierce && !isBounce)Destroy(gameObject);
           
        }
        if (other.tag == "Wall")
        {
            if (hitEffect) Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    

    #region 技能特性
    /// <summary>
    /// 技能物件回收
    /// </summary>
    /// <param name="time">延遲時間</param>
    private void DeleteObject(float time )
    {
        Destroy(gameObject,time);
    }
    /// <summary>
    /// 取得最近的怪物
    /// </summary>
    private void GetNearestMonster()
    {
        trackTarget = GameMaster.NearestMonster(transform.position,
            (MonsterCtrl)hitTarget);
    }
    /// <summary>
    /// 技能物件追蹤目標(逐步旋轉)
    /// </summary>
    private void Tracking()
    {
        //追蹤物是否還存(活)在
        if (trackTarget && trackTarget.IsAlive() && !isPenetrated)
        {
            Vector3 direction = trackTarget.transform.position - transform.position;

            // 增加旋轉速度，但不超過最大值
            turnSpeed = Mathf.Min(turnSpeed + turnAcceleration * Time.deltaTime, maxTurnSpeed);

            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(direction), Time.deltaTime * turnSpeed);

            // 固定速度移動
            transform.position += transform.forward * flySpeed * Time.deltaTime;
        }
        else if (isPenetrated)
        {
            transform.position += transform.forward * flySpeed * Time.deltaTime; ;            
        }
        else
        {
            GetNearestMonster();
        }
    }
    /// <summary>
    /// 技能物件彈射(敵人)
    /// </summary>
    private void Bounce()
    {
        bounceCount++; //以彈射+1
        Debug.Log("Bounced");
        GetNearestMonster();
        if (trackTarget)transform.LookAt(trackTarget.transform, Vector3.up);
            
        else if (!isPierce) Destroy(gameObject, 2);
    }

    public void CreatFrontArrow()
    {
        for(int i = 0; i < frontArrowAmount; i++) 
        {
            float baseOffset = 1f;
            float centerOffset = (frontArrowAmount - 1) * 0.5f * baseOffset;
            float arrowOffset = (i* baseOffset) - centerOffset; // 計算左右偏移，使得箭矢均勻分佈
            Instantiate(this, GameMaster.player.transform.ShootPoint(0,1f, arrowOffset),
            GameMaster.player.transform.rotation);
        }
        
    }

    public void CreatDiagonalArrow()
    {
        if (diagonalArrowAmount <= 0) return; // 確保箭矢數量有效

        float baseOffset = 0.1f;  // 箭矢在X軸上的偏移量
        float centerOffset = (diagonalArrowAmount - 1) * 0.5f * baseOffset;

        // 設定角度範圍，從 -7.5 到 7.5 度，每支箭逐步偏移
        for (int i = 0; i < diagonalArrowAmount; i++)
        {
            float arrowOffset = (i * baseOffset) - centerOffset;

            // 計算箭矢的角度，這裡使用了條件邏輯來實現不同的角度範圍
            float angle = 0;
            
                // 偶數索引（例如：0, 2, 4...）使用負角度，奇數索引（例如：1, 3, 5...）使用正角度
                if (i % 2 == 0) // 偶數索引
                {
                    angle = -(7.5f * (i / 2 + 1));  // 根據索引來確定負角度
                }
                else // 奇數索引
                {
                    angle = 7.5f * (i / 2 + 1); // 根據索引來確定正角度
                }
       // 使用計算出來的角度來生成箭矢
            Instantiate(this, GameMaster.player.transform.ShootPoint(angle, 0.1f, arrowOffset),
                GameMaster.player.transform.rotation * Quaternion.Euler(0, angle, 0));
        }
    }
    #endregion 技能特性
}
