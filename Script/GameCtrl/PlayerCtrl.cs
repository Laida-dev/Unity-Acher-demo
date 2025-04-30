using System;
using System.Collections;
using UnityEngine;

public class PlayerCtrl : BaseCharacterCtrl
{
    #region 角色控制相關
    /// <summary>
    /// 虛擬搖桿X操作值
    /// </summary>
    private float axisX
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }
    /// <summary>
    /// 虛擬搖桿Y操作值
    /// </summary>
    private float axisY
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }
    /// <summary>
    /// 搖桿操控的方向
    /// </summary>
    private Vector2 ctrlDirt;
    /// <summary>
    /// 是否有在操控方向
    /// </summary>
    private bool isMoving
    {
        get
        {
            ctrlDirt.x = axisX;
            ctrlDirt.y = axisY;
            return ctrlDirt != Vector2.zero;
        }
    }
    /// <summary>q
    /// 搖桿操控的夾角(12點鐘為基準0度)
    /// </summary>
    private float rotaAngle
    {
        get
        {
            //計算夾角後 * 橫向操作正負值
            return Vector2.Angle(Vector2.up, ctrlDirt) * Mathf.Sign(axisX);
        }
    }
    #endregion 角色控制相關

    #region 攻擊系統



    private BaseSkillCtrl _bullet;
    private BaseSkillCtrl bullet
    {
        get
        {
            if (_bullet == null)
            {
                _bullet = BulletManager.instance.GetBulletByName("Arrow");
            }
            return _bullet;
        }
    }

    private void OnEnable()
    {
        GameMaster.SetPlayer(this);
        SkillData.Initial();
        Hp = maxHp;

    }

    private void OnDisable()
    {
        GameMaster.SetPlayer(null);
    }

    private void OnDestroy()
    {
        GameMaster.SetPlayer(null);
    }

    void Update()
    {
        CheckMonsterSurvival();
        if (GameMaster.player == null) return;
        Move(isMoving);
        AttackAni();        
    }
    
    public void CheckMonsterSurvival()
    {
        if (!GameMaster.survivalMonster) 
        {
            animator.ResetTrigger(TriggerAttack);
            animator.SetTrigger("noMonster");       
        }
        else if (GameMaster.survivalMonster)
        {
            animator.ResetTrigger("noMonster");
        }
    }

    /// <summary>
    /// 攻擊功能
    /// </summary>
    /// 

    public override void Attack()
    {
        //啟動正向箭
        if(SkillData.GetLV("FrontArrow")>0)bullet.CreatFrontArrow();
        //啟動斜向箭
        if(SkillData.GetLV("DiagonalArrow") >0) bullet.CreatDiagonalArrow();
        
    }
    public override void AttackAni()
    {
        //移動：阻擋以下程式運行

        if (isMoving || !GameMaster.survivalMonster ||GameMaster.player == null)  return;
        //攻擊冷卻
                
        attackTarget = GameMaster.NearestMonster(pos);
        if (attackTarget)
        {//如果有找到離自己最近的怪物 Debug.Log(attackTarget.name);
            LookAtTarget();
            animator.SetFloat("AttackSpeed", attackSpeed );
            //觸發攻擊動畫
            animator.SetTrigger(TriggerAttack);                              
        }                       
               
    }
    #endregion 攻擊系統
    public override void TakeDamage(int dmg)
    {
        Hp -= dmg;
        Debug.Log($"Hp={Hp},MaxHp={maxHp},Hp%={Hp / maxHp}");
        Hpbar.fillAmount = (float)Hp / maxHp;
        if (Hp <= 0)
        { //血量歸0觸發死亡
            Hp = 0;
            Dead();            
        }

    }
    /// <summary>
    /// 移動功能
    /// </summary>
    public override void Move(bool B)
    {
        //同步走路/待機動畫
        animator.SetBool(IsMoving, B);
        //攻擊動畫層[1]
        animator.SetLayerWeight(1, B ? 0 : 1);
        //沒移動：阻擋以下程式運行
        if (!B) return;
        //角色控制器的轉動
        transform.rotation = Quaternion.Euler(0, rotaAngle, 0);
        //內建的角色控制器移動方法
        controller.SimpleMove(transform.forward * moveSpeed);
    }
    /// <summary>
    /// 死亡功能
    /// </summary>
    public override void Dead()
    {
        if (GameMaster.player == null) return;

        // 播放死亡動畫
        this.GetComponent<Animator>().SetTrigger("IsDead");

        // 設置玩家為 null
        GameMaster.SetPlayer(null);

        // 延遲觸發 SetStageDone
        StartCoroutine(DelaySetStageDone(2f));  // 延遲2秒觸發 SetStageDone

    }

    // Coroutine 用來延遲觸發 SetStageDone
    private IEnumerator DelaySetStageDone(float delay)
    {
        yield return new WaitForSeconds(delay);  // 延遲指定的時間
        GameMaster.SetStageDone(true);  // 執行 SetStageDone
    }

    
}
