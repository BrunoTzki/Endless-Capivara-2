using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public GameObject canva;
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
        canva.SetActive(true);
        for (int i = timeRemaing; i > 0; i--)
        {
            timerText.text = i.ToString("0");
            yield return new WaitForSeconds(1);
        }
        canva.SetActive(false);
        ResetTimer();
    }
    void ResetTimer()
    {
        timerText.text = "3";
    }
}
