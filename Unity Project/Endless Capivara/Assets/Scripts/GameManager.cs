using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerData
{
    public int coins;
    public int[] max;
    public int[] progress;
    public int[] currentProgress;
    public int[] reward;
    public string[] missionType;
    public int[] skinsCost;
}

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int coins;
    public int[] skinsCost;

    public int skinIndex;

    private MissionBase[] missions;
    private string filePath;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/playerinfo.dat";

        missions = new MissionBase[2];

        if (File.Exists(filePath))
        {
            Load();
        }
        else
        {
            for (int i = 0; i < missions.Length; i++)
            {
                GameObject newMission = new GameObject("Mission" + i);
                newMission.transform.SetParent(transform);
                MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter, MissionType.CoinsSingleRun };
                int randomType = Random.Range(0, missionType.Length);
                if (randomType == (int)MissionType.SingleRun)
                {
                    missions[i] = newMission.AddComponent<SingleRun>();
                }
                else if (randomType == (int)MissionType.TotalMeter)
                {
                    missions[i] = newMission.AddComponent<TotalMeter>();
                }
                else if (randomType == (int)MissionType.CoinsSingleRun)
                {
                    missions[i] = newMission.AddComponent<CoinsSingleRun>();
                }

                missions[i].Created();
            }
        }

    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData data = new PlayerData();

        data.coins = coins;

        data.max = new int[2];
        data.progress = new int[2];
        data.currentProgress = new int[2];
        data.reward = new int[2];
        data.missionType = new string[2];
        data.skinsCost = new int[skinsCost.Length];

        for (int i = 0; i < 2; i++)
        {
            data.max[i] = missions[i].max;
            data.progress[i] = missions[i].progress;
            data.currentProgress[i] = missions[i].currentProgress;
            data.reward[i] = missions[i].reward;
            data.missionType[i] = missions[i].missionType.ToString();
        }

        for (int i = 0; i < skinsCost.Length; i++)
        {
            data.skinsCost[i] = skinsCost[i];
        }

        bf.Serialize(file, data);
        file.Close();
    }

    void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

        PlayerData data = (PlayerData)bf.Deserialize(file);
        file.Close();

        coins = data.coins;

        for (int i = 0; i < 2; i++)
        {
            GameObject newMission = new GameObject("Mission" + i);
            newMission.transform.SetParent(transform);
            if (data.missionType[i] == MissionType.SingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<SingleRun>();
                missions[i].missionType = MissionType.SingleRun;
            }
            else if (data.missionType[i] == MissionType.TotalMeter.ToString())
            {
                missions[i] = newMission.AddComponent<TotalMeter>();
                missions[i].missionType = MissionType.TotalMeter;
            }
            else if (data.missionType[i] == MissionType.CoinsSingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<CoinsSingleRun>();
                missions[i].missionType = MissionType.CoinsSingleRun;
            }

            missions[i].max = data.max[i];
            missions[i].progress = data.progress[i];
            missions[i].currentProgress = data.currentProgress[i];
            missions[i].reward = data.reward[i];
        }

        for (int i = 0; i < data.skinsCost.Length; i++)
        {
            skinsCost[i] = data.skinsCost[i];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRun(int skIndex)
    {
        skinIndex = skIndex;
        SceneManager.LoadScene("Game");
    }

    public void EndRun()
    {
        SceneManager.LoadScene("Menu");
    }

    public MissionBase GetMission(int index)
    {
        return missions[index];
    }

    public void StartMissions()
    {
        for (int i = 0; i < 2 ; i++)
        {
            missions[i].RunStart();
        }
    }
    public void GenerateMission(int i)
    {
        Destroy(missions[i].gameObject);

        GameObject newMission = new GameObject("Mission" + i);
        newMission.transform.SetParent(transform);
        MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter, MissionType.CoinsSingleRun };
        int randomType = Random.Range(0, missionType.Length);
        if (randomType == (int)MissionType.SingleRun)
        {
            missions[i] = newMission.AddComponent<SingleRun>();
        }
        else if (randomType == (int)MissionType.TotalMeter)
        {
            missions[i] = newMission.AddComponent<TotalMeter>();
        }
        else if (randomType == (int)MissionType.CoinsSingleRun)
        {
            missions[i] = newMission.AddComponent<CoinsSingleRun>();
        }

        missions[i].Created();

        FindObjectOfType<Menu>().SetMission();
    }
}
