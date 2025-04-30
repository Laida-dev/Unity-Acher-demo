using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private StageScriptableObject stageDB;
    /// <summary>
    /// 目前關卡進度(波次)
    /// </summary>
    private int currentWave;
    /// <summary>
    /// 是否進入戰鬥
    /// </summary>
    private bool inBattle;
    /// <summary>
    /// 戰鬥是否結束
    /// </summary>
    private bool endBattle
    {
        get
        {
            return currentWave >= stageDB.waveCount;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //播放關卡的背景音樂
       // AudioManager.instance.PlayBGM(bgmStage);
        GameMaster.SetStageDone(false);
        Invoke("Initial", 2f);
    }

    void Initial()
    {
       GameMaster.AddScene("BattleUI");
        Invoke("WaveStart", 2f);
    }
    /// <summary>
    /// 開啟新波次
    /// </summary>
    void WaveStart()
    {
        ItemCtrl.autoPick = false;//自動吸寶：關閉
        //是否Boss來臨：換音樂
       /* if (stageDB.IsBossWave(currentWave))
            AudioManager.instance.PlayBGM(bgmBoss);*/
        foreach (SpawnInfo info in stageDB.GetSpawnList(currentWave))
        {//依照清單內的資訊依序生產怪物
            Instantiate(info.monster, info.pos, Quaternion.identity);
        }
        inBattle = true;//開戰
    }
    /// <summary>
    /// 波次進行中
    /// </summary>
    void WaveUpdate()
    {
        if (inBattle && !GameMaster.survivalMonster)
        {//波次完成
            ItemCtrl.autoPick = true;//自動吸寶：開啟
            inBattle = false;
            currentWave++;//跳至下一波
            GameData.RewardExpCtrl(20);
            if (endBattle)
            {
                GameMaster.SetStageDone(true);
            }
            else Invoke("WaveStart", 2f);//2秒後啟動  
        }
    }

    // Update is called once per frame
    void Update()
    {
        WaveUpdate();
    }
}
