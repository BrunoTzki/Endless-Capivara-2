using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public Text[] missionDescription, missionReward, missionProgress;
    public GameObject[] rewardButton;
    public Text coinsText;
    public Text costText;
    public GameObject[] skins;
    public GameObject startButton;
    public GameObject buyButton;

    private int skinIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetMission();
        UpdateCoins(GameManager.gm.coins);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RefreshMenu() 
    {
        SetMission();
        UpdateCoins(GameManager.gm.coins);
    }

    public void UpdateCoins(int coins)
    {

        coinsText.text = coins.ToString();
    }

    public void StartRun()
    {
            GameManager.gm.StartRun(skinIndex);
    }

    public void BuySkin()
    {
        if (GameManager.gm.skinsCost[skinIndex] <= GameManager.gm.coins)
        {
            GameManager.gm.coins -= GameManager.gm.skinsCost[skinIndex];
            GameManager.gm.skinsCost[skinIndex] = 0;
            GameManager.gm.Save();
            string cost = "";
            costText.text = cost;
            startButton.SetActive(true);
            buyButton.SetActive(false);
        }
    }
    public void SetMission()
    {
        for (int i = 0; i < 2; i++)
        {
            MissionBase mission = GameManager.gm.GetMission(i);
            missionDescription[i].text = mission.GetMissionDescription();
            missionReward[i].text = "Recompensa: " + mission.reward;
            missionProgress[i].text = mission.progress + mission.currentProgress + " / " + mission.max;
            if (mission.GetMissionComplete())
            {
                rewardButton[i].SetActive(true);
            }

        }
        
        GameManager.gm.Save();
    }

    public void GetReward(int missionIndex)
    {
        GameManager.gm.coins = +GameManager.gm.GetMission(missionIndex).reward;
        UpdateCoins(GameManager.gm.coins);
        rewardButton[missionIndex].SetActive(false);
        GameManager.gm.GenerateMission(missionIndex);
    }

    public void CHangeSkin(int index) 
    {
        skinIndex += index;
        if(skinIndex >= skins.Length)
        {
            skinIndex = 0;
        }
        else if(skinIndex < 0)
        {
            skinIndex = skins.Length - 1;
        }

        for (int i = 0; i < skins.Length; i++)
        {
            if(i == skinIndex)
                skins[i].SetActive(true);
            else
                skins[i].SetActive(false);
        }

        string cost = "";
        if (GameManager.gm.skinsCost[skinIndex] != 0)
        {
            cost = GameManager.gm.skinsCost[skinIndex].ToString();
            startButton.SetActive(false);
            buyButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(true);
            buyButton.SetActive(false);
        }
        costText.text = cost;
    }

}
     