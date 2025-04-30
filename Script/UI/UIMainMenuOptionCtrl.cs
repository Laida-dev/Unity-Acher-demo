using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuOptionCtrl : MonoBehaviour
{
    private Animator _animator;
    private Animator animator 
    {

        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        } 
    }
    private Toggle _toggle;
    private Toggle Toggle
    {
        get
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<Toggle>();
            }
            return _toggle;
        }
    }

    void Start()
    {
        UpdateUI();
    }


    public void UpdateUI()
    {
        animator.SetBool("isOn", Toggle.isOn);        
    }
}
