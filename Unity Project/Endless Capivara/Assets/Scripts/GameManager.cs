using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    private MissionBase[] missions;

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

        missions = new MissionBase[3];

        for (int i = 0; i < missions.Length; i++)
        {
            GameObject newMission = new GameObject("Mission" + i);
            newMission.transform.SetParent(transform);
            MissionType[] missionType = {MissionType.SingleRun, MissionType.TotalMeter, MissionType.CoinsSingleRun};
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRun()
    {
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
        for (int i = 0; i < 3 ; i++)
        {
            missions[i].RunStart();
        }
    }
}
