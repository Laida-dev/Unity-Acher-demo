using UnityEngine;
using System.Collections.Generic;
using System;


/// <summary>
/// [序列化]波次資訊
/// </summary>
[Serializable]
public struct WaveInfo
{
    public List<SpawnInfo> spawnList;
    public bool isBossWave;
}

/// <summary>
/// [序列化]怪物生成單元資訊
/// </summary>
[Serializable]
public struct SpawnInfo
{
    public Vector3 pos;
    public MonsterCtrl monster;
}

[CreateAssetMenu(fileName = "StageData", menuName = "GameDB/StageData", order = 2)]
public class StageScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<WaveInfo> waves = new List<WaveInfo>();
    /// <summary>
    /// 總波次數
    /// </summary>
    public int waveCount
    {
        get
        {
            return waves.Count;
        }
    }

    /// <summary>
    /// 取得波次怪物配置
    /// </summary>
    /// <param name="waveIndex">波次編號</param>
    /// <returns>怪物生成清單</returns>
    public List<SpawnInfo> GetSpawnList(int waveIndex)
    {
        return waves[waveIndex].spawnList;
    }
    /// <summary>
    /// 檢查是否為Boss波次
    /// </summary>
    /// <param name="waveIndex">波次編號</param>
    /// <returns>是/否</returns>
    public bool IsBossWave(int waveIndex)
    {
        return waves[waveIndex].isBossWave;
    }
}
