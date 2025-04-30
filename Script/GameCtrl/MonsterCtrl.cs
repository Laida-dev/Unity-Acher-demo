using UnityEditor;
using UnityEngine;

public class MonsterCtrl : BaseCharacterCtrl
{
    public float attackInterval = 8f;
    private BaseSkillCtrl _bullet;
    private BaseSkillCtrl bullet
    {
        get
        {
            if (_bullet == null)
            {
                _bullet = BulletManager.instance.GetBulletByName("MagicBall");
            }
            return _bullet;
        }
    }
    private ItemDrop _itemdrop;
    private ItemDrop itemDrop
    {
        get 
        { 
            if (_itemdrop == null) _itemdrop = GetComponent<ItemDrop>();
            return _itemdrop; 
        }
    }
    private Vector3 targetPos
    {
        get
        {
            return attackTarget ?
                attackTarget.pos : Vector3.zero;
        }
    }
    
    
    private void OnEnable()
    {
        tag = "Monster";
        GameMaster.AddMonster(this);
    }

    private void OnDisable()
    {
        GameMaster.RemoveMonster(this);
    }

    private void OnDestroy()
    {
        GameMaster.RemoveMonster(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackTarget = GameMaster.player;
        Hp = maxHp;
        attackCD = attackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive()) return; //死亡：停止動作

        SimpleAI();
    }

    public override void TakeDamage(int dmg)
    {
        Hp -= dmg;
        if (Hp <= 0)
        { //血量歸0觸發死亡
            Hp = 0;
            Dead();
        }
        
    }

    public override void Dead()
    {
        tag = "isDead";
        GameMaster.RemoveMonster(this);
        this.GetComponent<Animator>().SetTrigger("isDead");
        itemDrop?.Drop();
    }

    public void destroy()
    {
        Destroy(gameObject);
    }
    #region 怪物AI相關
    Vector3 vectorRandom;
    private void SimpleAI()
    {
         //執行攻擊(冷卻完成)
         if (attackCD >= attackInterval)
         {
             Attack();
             //重置計時
             attackCD = 0;
             Vector3 dirPoint = Random.insideUnitSphere;
             dirPoint.y = 0;
             vectorRandom = dirPoint;
         }
         //隨機移動(攻擊冷卻時)
         else
         {
             attackCD += Time.deltaTime;//累進計時器
             if (attackCD > 0.5f) Move();
         }
        
    }
    public override void Attack()
    {
        LookAtTarget();
        animator.SetTrigger(TriggerAttack);

        // 設定散射角度（這裡是左右 ±5°）
        float[] angles = { 0};

        foreach (float angle in angles)
        {
            Vector3 spawnPos = transform.ShootPoint(angle, 2, 0);
            Quaternion shootRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + angle, 0);
            Instantiate(bullet, spawnPos, shootRotation);
        }
    }
    public void Move()
    {
        transform.rotation = Quaternion.LookRotation(vectorRandom, Vector3.up);
        //內建的角色控制器移動方法
        controller.SimpleMove((transform.forward * moveSpeed));
    }
    #endregion 怪物AI相關
}
