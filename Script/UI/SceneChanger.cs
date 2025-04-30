using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static string currentScene;
    public static string nextScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SceneChanger.currentScene = SceneManager.GetActiveScene().name;
        
    }
}
