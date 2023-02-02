using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public Image[] lifeHearts;
    public Text coinText;
    public GameObject gameOverPanel;
    public Text scoreText;


    public GameObject timer;
    public Text timerText;
    IEnumerator _timerCR;

    private void Awake()
    {
        ResetTimer();
    }
    public void StartTimerCall()
    {
        _timerCR = StartTimer();
        StartCoroutine(_timerCR);
    }
    IEnumerator StartTimer(int timeRemaing = 3)
    {
        timer.SetActive(true);
        for (int i = timeRemaing; i > 0; i--)
        {
            timerText.text = i.ToString("0");
            yield return new WaitForSeconds(1);
        }
        timer.SetActive(false);
        ResetTimer();
    }
    void ResetTimer()
    {
        timerText.text = "3";
    }
    public void UpdateCoins(int coin)
    {
        coinText.text = coin.ToString();
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
    public void UpdateLive(int lives)
    {
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if (lives > i)
            {
                lifeHearts[i].color = Color.white;
            }
            else
            {
                lifeHearts[i].color = Color.black;
            }
        }
    }

}

