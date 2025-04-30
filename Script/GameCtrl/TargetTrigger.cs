using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CameraTargetCtrl cameraController; // Camera 腳本的引用

    private void OnTriggerEnter(Collider other)
    {
       
        // 通知 Camera 腳本處理觸發事件
        if (cameraController != null)
        {
            cameraController.OnTargetTriggerEnter(other);
        }
    }
}
