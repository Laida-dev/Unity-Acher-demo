using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// 遊戲進行用基礎角色控制器(抽象層)
/// </summary>
[RequireComponent(typeof(CharacterController))]
public abstract class BaseCharacterCtrl : MonoBehaviour
{
    #region 動畫控制相關
    /// <summary>
    /// UNITY動畫控制器實體
    /// </summary>
    private Animator _animator;
    public Animator animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            return _animator;
        }
    }
    public const string IsMoving = "IsMoving";
    public const string IsAttacking = "IsAttacking";
    public const string TriggerAttack = "TriggerAttack";
    public const string IsDead = "Dead";
    #endregion 動畫控制相關

    #region 角色控制相關
    public bool IsAlive()
    {
        return (Hp > 0);
    }
    [Header("角色參數")]
    public float moveSpeed = 5.0f;
    public int maxHp = 50;    
    protected int Hp;
    /// <summary>
    /// UNITY角色控制器實體
    /// </summary>
    private CharacterController _controller;
    public CharacterController controller
    {
        get
        {
            if (_controller == null)
                _controller = GetComponent<CharacterController>();
            return _controller;
        }
    }
    /// <summary>
    /// 物件所在世界座標
    /// </summary>
    public Vector3 pos
    {
        get
        {
            return transform.position;
        }
    }
    [Header("攻擊設定")]
    [Range(0.5f,3f)]
    public float attackSpeed;
    
    /// <summary>
    /// 攻擊冷卻時間(疊加超過至 attackInterval)
    /// </summary>
    protected float attackCD ;
    /// <summary>
    /// 是否觸發攻擊
    /// </summary>
    /*protected bool triggerAttack
    {
        get
        {
            return attackCD >= attackInterval;
        }
    }*/
    /// <summary>
    /// 當前鎖定的攻擊對象
    /// </summary>
    protected BaseCharacterCtrl attackTarget;
    public Image Hpbar;

    #endregion

    #region 功能區塊
    /// <summary>
    /// 攻擊功能(虛擬化)
    /// </summary>
    public virtual void Attack()
    {
        Debug.LogWarning($"未撰寫{name}的攻擊邏輯。");
    }
    public virtual void AttackAni()
    {
        Debug.LogWarning($"未撰寫{name}的攻擊邏輯。");
    }
    /// <summary>
    /// 看向攻擊目標
    /// </summary>
    protected virtual void LookAtTarget()
    {
        transform.LookAt(attackTarget.pos, Vector3.up);
    }
    /// <summary>
    /// 受到傷害
    /// </summary>
    public virtual void TakeDamage(int dmg)
    {
        
    }

    /// <summary>
    /// 移動功能
    /// </summary>
    public virtual void Move(bool B)
    {
        Debug.LogWarning($"未撰寫{name}的移動邏輯。");
    }

    
    /// <summary>
    /// 死亡功能
    /// </summary>
    public virtual void Dead()
    {
        Debug.LogWarning($"未撰寫{name}的死亡邏輯。");
    }
    #endregion 功能區塊
}
