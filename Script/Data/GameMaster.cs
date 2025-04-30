using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 遊戲管理
/// </summary>
public static class GameMaster 
{
    #region 遊戲速度控制相關
    /// <summary>
    /// 預設標準速度
    /// </summary>
    private const float normalSpeed = 1f;
    /// <summary>
    /// 預設速度：1
    /// </summary>
    public static void DefautGameSpeed()
    {
        Time.timeScale = normalSpeed;
    }
    /// <summary>
    /// 暫停速度：0
    /// </summary>
    public static void GamePause()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// 速度控制
    /// </summary>
    /// <param name="speed">速度值</param>
    public static void GameSpeedCtrl(float speed)
    {
        Time.timeScale = speed;
    }
    #endregion 遊戲速度控制相關
    #region 技能抽取類型
    public enum SoltsCount
    {
        One = 1, Two = 2, Three = 3
    }
    /// <summary>
    /// [擴充功能]取得選擇技能欄位數量
    /// </summary>
    /// <param name="count">欄位數量列舉</param>
    /// <returns>數量</returns>
    public static int GetCount(this SoltsCount count)
    {
        return (int)count;
    }
    #endregion 技能抽取類型
    #region 場景控制相關
    /// <summary>
    /// 加載場景
    /// </summary>
    /// <param name="sceneName">場景名稱</param>
    public static void AddScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public static bool stageDone { get; private set; }
    public static void SetStageDone(bool B)
    {
        stageDone = B;
      if (stageDone) GameData.GameOverCtrl();
    }
    /// <summary>
    /// 切換場景
    /// </summary>
    /// <param name="sceneName">場景名稱</param>
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    /// <summary>
    /// 卸載遊戲戰鬥UI
    /// </summary>
    public static void UnloadGameUI()
    {
        SceneManager.UnloadSceneAsync("BattleUI");
    }
    /// <summary>
    /// 切回主選單
    /// </summary>
    public static void GoBackMainMenu()
    {
        SceneChanger.currentScene = SceneManager.GetActiveScene().name;
        SceneChanger.nextScene = "MainMenu";
        SceneManager.LoadScene("LoadCover", LoadSceneMode.Additive);
    }
    #endregion 場景控制相關
    #region 玩家控制相關
    /// <summary>
    /// 玩家控制器物件實體(限制讀取)
    /// </summary>
    public static PlayerCtrl player { get; private set; }
    /// <summary>
    /// 玩家是否存在
    /// </summary>
    public static bool gotPlayer
    {
        get
        {
            return player != null;
        }
    }
    /// <summary>
    /// 玩家控制及時座標
    /// </summary>
    public static Vector3 playerPos
    {
        get
        {
            return player.transform.position;
        }
    }
    /// <summary>
    /// 設定登場玩家控制器
    /// </summary>
    /// <param name="ctrl">玩家控制器</param>
    public static void SetPlayer(PlayerCtrl ctrl)
    {
        player = ctrl;
    }
    #endregion 玩家控制相關

    #region 怪物控制相關
    /// <summary>
    /// 登場怪物控制器實體清單
    /// </summary>
    private static List<MonsterCtrl> monsterCtrlList = new List<MonsterCtrl>();
    /// <summary>
    /// 是否還有殘餘登場怪物
    /// </summary>
    public static bool survivalMonster
    {
        get
        {
            return monsterCtrlList.Count > 0;
        }
    }
    /// <summary>
    /// 登場怪物控制器加入管理清單
    /// </summary>
    /// <param name="ctrl">怪物控制器</param>
    public static void AddMonster(MonsterCtrl ctrl)
    {
        monsterCtrlList.Add(ctrl);
    }
    /// <summary>
    /// 登場怪物控制器移出管理清單
    /// </summary>
    /// <param name="ctrl">怪物控制器</param>
    public static void RemoveMonster(MonsterCtrl ctrl)
    {
        monsterCtrlList.Remove(ctrl);
    }
    /// <summary>
    /// 找尋最近的怪物
    /// </summary>
    /// <param name="centerPos">搜尋中心位置</param>
    /// <returns>找到的怪物(離搜索點最近)</returns>
    public static MonsterCtrl NearestMonster(Vector3 centerPos, MonsterCtrl exclude = null)
    {
        MonsterCtrl monster = null;
        //運算邏輯
        float range = 999f;
        //迴圈：起始值 = 0; 終點值 = 清單總數-1; 增值1
        for(int i = 0; i < monsterCtrlList.Count; i++)
        {//掃描清單內所有MonsterCtrl
            if (monsterCtrlList[i] == exclude) continue;//這圈跳過以下的程式
            float distance = Vector3.Distance(centerPos, monsterCtrlList[i].pos);
            if (distance < range)
            {//兩點取直線距離 < 最短搜索範圍
                //紀錄找到的物件
                monster = monsterCtrlList[i];
                //紀錄該物件的距離
                range = distance;
            }
        }
        //回傳結果
        return monster;
    }


    #endregion 怪物控制相關

    #region 彈幕管理相關
    /// <summary>
    /// [擴充功能]發射點產生功能
    /// </summary>
    /// <param name="center">發射物中心</param>
    /// <param name="angle">偏移角</param>
    /// <returns>發射點座標</returns>
    public static Vector3 ShootPoint(this Transform center, float angle, float distance, float offsetX)
    {
        // 角色面向角度修正，讓發射方向根據 `angle` 進行微調
        float fixAngle = center.rotation.eulerAngles.y + angle;

        // 計算玩家前方的方向
        Vector3 forwardDirection = Quaternion.Euler(0, fixAngle, 0) * Vector3.forward;

        // 計算左右偏移量，將偏移量加入前方方向
        Vector3 rightDirection = Quaternion.Euler(0, 90, 0) * forwardDirection; // 右方向
        Vector3 offset = rightDirection * offsetX;

        // 計算發射點位置：距離玩家的偏移 + 左右偏移量 + 高度偏移
        Vector3 shootPosition = center.position + forwardDirection * distance + offset;

        // 回傳計算後的發射點位置
        return shootPosition + Vector3.up; 
    }
    #endregion 彈幕管理相關
    #region 文字工具
    private static StringBuilder result = new StringBuilder();
    /// <summary>
    /// 將字串以大寫字母分割換行
    /// </summary>
    /// <param name="input">輸入字串</param>
    /// <returns>輸出字串</returns>
    public static string SplitByUpperCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        result.Clear();//確保資訊乾淨
        result.Append(input[0]); // 加入第一個字元

        // 從第二個字元開始檢查
        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                result.Append("\n"); // 在大寫字母前加入換行
            }
            result.Append(input[i]);
        }

        return result.ToString();
    }
    #endregion 文字工具
}
