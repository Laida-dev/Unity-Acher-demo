using UnityEngine;
public class UIBillBoard : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 2f, -1.75f); // 血條的偏移量
    private Camera mainCamera;

    void Awake()
    {
        // 在 Awake 中先嘗試獲取相機引用
        mainCamera = Camera.main;
    }

    void Start()
    {
        // 如果在 Awake 中沒有獲取到，再次嘗試
        if (mainCamera == null)
            mainCamera = Camera.main;

        // 確保血條不是人物的子物件
        transform.SetParent(null);
    }

    void LateUpdate()  // 使用 LateUpdate 而不是 Update
    {
        if (GameMaster.player != null && mainCamera != null)
        {
            // 讓血條位置跟隨角色 + 偏移量
            transform.position = GameMaster.playerPos + offset;

            // 讓血條面向相機
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
        else
        {
            // 如果引用丟失，重新獲取
            if (mainCamera == null)
                mainCamera = Camera.main;
        }
    }
}


