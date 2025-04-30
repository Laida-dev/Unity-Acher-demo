using UnityEngine;
using UnityEngine.SceneManagement;
namespace ArcherGame
{
    public class LoadScenes : MonoBehaviour
    {
        
        /// <summary>
        /// 切換場景功能
        /// </summary>
        /// <param name="sceneName">場景名稱</param>
        public void LoadNext()
        {
            SceneManager.LoadSceneAsync(SceneChanger.nextScene, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(SceneChanger.currentScene);
            
         }
        
        public void Loading(string sceneName)
        {
            SceneChanger.nextScene = sceneName;
            SceneChanger.currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("LoadCover", LoadSceneMode.Additive);
        }
        public static void Loading()
        {
            SceneChanger.currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("LoadCover", LoadSceneMode.Additive);
        }

        public void LoadComplete()
        {
         
            SceneManager.UnloadSceneAsync("LoadCover");
        }
    }
}