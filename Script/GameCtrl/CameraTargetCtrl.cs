using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTargetCtrl : MonoBehaviour
{
    #region Camera設定相關
    [Header("CinemachineCamera物件")]
    public CinemachineCamera cinemachineCamera;
    [Header("追蹤物件")]
    public GameObject cameraTarget;
    /// <summary>
    /// 可設定的縮放倍率
    /// </summary>
    [Range(1.0f, 2.0f)]
    public float followOffectMult = 1.0f;
    private float originalCamaraOffset;
    private float zoomCameraOffect;
    private float clickY;
    private float dragY;
    /// <summary>
    /// 對焦島嶼位置(對焦座標暫存)
    /// </summary>
    private Vector3 islandPos;
    /// <summary>
    /// 最小Z軸值
    /// </summary>
    public float zPosMin = -5f;
    /// <summary>
    /// 最大Z軸值
    /// </summary>
    public float zPosMax = 15f;
    /// <summary>
    /// 焦點位置實際存放欄位
    /// </summary>
    private Vector3 _focusPos;
    /// <summary>
    /// 焦點位置數值接口
    /// </summary>
    
    public Vector3 focusPos
    {
        get
        {
            return cameraTarget.transform.position;
        }
        set
        {
            _focusPos = value;
            //Y值的平面鎖定
            _focusPos.y = 1;
            //Z值的範圍限制
            _focusPos.z = Mathf.Clamp(_focusPos.z, zPosMin, zPosMax);
            cameraTarget.transform.position = _focusPos;
        }
    }
    /// <summary>
    /// 拖曳中線數據實際存放欄位
    /// </summary>
    private Vector3 _centerLinePos;
    /// <summary>
    /// 拖曳中線數據接口
    /// </summary>
    private Vector3 centerLinePos
    {
        get
        {
            return _centerLinePos;
        }
        set
        {
            _centerLinePos = value;
            //X值的直線鎖定
            _centerLinePos.x = 0;
        }
    }
    #endregion Camera設定相關
    #region UI控制相關
    public Animator penalAnimator;
    /// <summary>
    /// 動畫延遲速率範圍
    /// </summary>
    [Range(activeSpeedMini, cativeSpeedMax)]
    public float activeSpeed;
    /// <summary>
    /// 動畫延遲時間
    /// </summary>
    private const float activeSpeedMini = 1f, cativeSpeedMax = 10f;
    /// <summary>
    /// Penal透明度
    /// </summary>
    private float penalAlpha = 1;

    #endregion UI控制相關


    void Start()
    {
        //紀錄原始的 cinemachineFollowOffset
        originalCamaraOffset = cinemachineCamera.Lens.OrthographicSize;
        zoomCameraOffect = cinemachineCamera.Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0))

        {//按下滑鼠左鍵(一次性)
            
            zoomCameraOffect = originalCamaraOffset * followOffectMult;
            //點擊初始位置
            clickY = Input.mousePosition.y;
            //焦點回到中軸
            centerLinePos = islandPos;
            //重新對焦
            focusPos = centerLinePos;
        }
        else if (Input.GetMouseButton(0))
        {//按住滑鼠左鍵：偵測拖曳方位
            
            dragY = Input.mousePosition.y - clickY;
            //拖曳後的方向向量
            //朝向方向向量移動
            if (cameraTarget.transform.position.z <= zPosMax && cameraTarget.transform.position.z >= zPosMin)
            {
                cameraTarget.transform.Translate(Vector3.forward * dragY * Time.deltaTime * 6 * -1);
                //紀錄並運算限制的座標
                focusPos = cameraTarget.transform.position;
                
               
            }
            cameraTarget.transform.position = focusPos;

            //更新停頓位置
            clickY = Input.mousePosition.y;
            //按住滑鼠時隱藏介面
            PenalDragEvent();
        }
        else if(Input.GetMouseButtonUp(0))
        {//鬆開滑鼠左鍵(一次性)
            
            zoomCameraOffect = originalCamaraOffset;
            cameraTarget.transform.position = islandPos;
        }
        else
        {
            //放開滑鼠時顯示介面
            PenalDropEvent();
        }
        //用漸進式的方式改變FollowOffset
        cinemachineCamera.Lens.OrthographicSize = 
            Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, zoomCameraOffect, Time.deltaTime * activeSpeed);
    }
    public void OnTargetTriggerEnter(Collider other)
    {
        // 更新對焦位置
        islandPos = other.transform.position;
        SceneChanger.nextScene = other.name;
        Debug.Log(SceneChanger.nextScene);
    }
    /// <summary>
    /// 使用拖曳行為同步Penal透明度
    /// </summary>
    private void PenalDragEvent()
    {
        penalAlpha -= Time.deltaTime * activeSpeed;
        penalAlpha = Mathf.Clamp(penalAlpha,0,1);
        penalAnimator?.SetFloat("Alpha", penalAlpha);
    }
    /// <summary>
    /// 使用拖放行為同步Penal透明度
    /// </summary>
    private void PenalDropEvent()
    {
        penalAlpha += Time.deltaTime * activeSpeed;
        penalAlpha = Mathf.Clamp(penalAlpha, 0, 1);
        penalAnimator?.SetFloat("Alpha", penalAlpha);
    }

}
