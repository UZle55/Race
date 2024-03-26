using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameInterface : MonoBehaviour
{
    [SerializeField] private GameObject coinsCount;
    [SerializeField] private GameObject timeLeft;
    [SerializeField] private bool isTimerStart;
    [SerializeField] private KeyCode pauseButton;
    [SerializeField] private GameObject pauseBackground;
    private bool isOnPause;
    private int[] givenTime;
    private float t;
    private int coins;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CalculateTimer();
        UpdateTimer();
        if (Input.GetKeyDown(pauseButton) && !isOnPause)
        {
            pauseBackground.SetActive(true);
            Time.timeScale = 0;
            isOnPause = true;
        }
        else if (Input.GetKeyDown(pauseButton) && isOnPause)
        {
            pauseBackground.SetActive(false);
            Time.timeScale = 1;
            isOnPause = false;
        }
    }

    public void SetGivenTime(int[] givenTime)
    {
        this.givenTime = givenTime;
    }

    private void CalculateTimer()
    {
        if (isTimerStart)
        {
            t += Time.deltaTime;
            if (t >= 1)
            {
                t--;
                if (givenTime[1] != 0)
                {
                    givenTime[1]--;
                }
                else if (givenTime[0] != 0)
                {
                    givenTime[0]--;
                    givenTime[1] = 59;
                }
                else
                {
                    TimeEnd();
                }
            }
        }
    }

    private void UpdateTimer()
    {
        if (isTimerStart)
        {
            var minutes = "0";
            if (givenTime[0] < 10)
            {
                minutes += givenTime[0].ToString();
            }
            else
            {
                minutes = givenTime[0].ToString();
            }
            minutes += ":";
            var seconds = "0";
            if (givenTime[1] < 10)
            {
                seconds += givenTime[1].ToString();
            }
            else
            {
                seconds = givenTime[1].ToString();
            }
            timeLeft.GetComponent<Text>().text = "Время " + minutes + seconds;
        }
    }

    private void TimeEnd()
    {

    }

    public void IncreaseCoinsCount(int value)
    {
        coins += value;
        GetComponent<AudioSource>().Play();
        coinsCount.GetComponent<Text>().text = "Монеты " + coins.ToString();
    }

    public void OnExitGameButtonClick()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
        SceneManager.UnloadScene("Game");
    }
}
