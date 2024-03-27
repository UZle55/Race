using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameInterface : MonoBehaviour
{
    public enum LoseReason
    {
        TimeIsOver,
        CarCrash,
        FenceCrash
    }
    public enum WinReason
    {
        CoinsCollected
    }
    [SerializeField] private GameObject coinsCount;
    [SerializeField] private GameObject timeLeft;
    [SerializeField] private bool isTimerStart;
    [SerializeField] private KeyCode pauseButton;
    [SerializeField] private GameObject pauseBackground;
    [SerializeField] private GameObject loseBackground;
    [SerializeField] private GameObject loseReasonText;
    [SerializeField] private GameObject winBackground;
    [SerializeField] private GameObject winReasonText;
    [SerializeField] private GameObject tutorialFirstStep;
    [SerializeField] private GameObject tutorialSecondStep;
    [SerializeField] private GameObject tutorialThirdStep;
    [SerializeField] private float tutorialWindowsAppearingSpeed;
    [SerializeField] private GameObject audioManager;
    private bool isOnPause;
    private int[] givenTime = new int[] { 99, 99, 99 };
    private float t;
    private int coins;
    private bool isPlayingTutorial = false;
    private bool isShowingFirstStep = false;
    private bool isShowingSecondStep = false;
    private bool isShowingThirdStep = false;
    private bool isUnshowingThirdStep = false;
    private int goLeftButtonPressedCount = 0;
    private int goRightButtonPressedCount = 0;
    private int accelerateButtonPressedCount = 0;
    private int decelerateButtonPressedCount = 0;
    private float thirdStepShowingTime;
    // Start is called before the first frame update
    void Start()
    {
        SetGivenTime(RoadManager.TimeToSet);
        if (!RoadManager.IsEndlessMode)
        {
            coinsCount.GetComponent<Text>().text = "Монеты " + coins.ToString() + "/" + RoadManager.CoinsCountAim.ToString();
        }
        else
        {
            coinsCount.GetComponent<Text>().text = "Монеты " + coins.ToString();
        }
        isPlayingTutorial = RoadManager.IsTutorial;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayingTutorial)
        {
            CalculateTimer();
        }
        else
        {
            PlayTutorial();
        }
        UpdateTimer();
        if (Input.GetKeyDown(pauseButton))
        {
            OnPauseButtonClick();
        }
    }

    private void PlayTutorial()
    {
        if(!isShowingFirstStep && !isShowingSecondStep && !isShowingThirdStep)
        {
            isShowingFirstStep = true;
        }
        if (isShowingFirstStep)
        {
            if (!tutorialFirstStep.activeSelf)
            {
                tutorialFirstStep.SetActive(true);
            }
            if(tutorialFirstStep.GetComponent<Image>().color.a < 1
                && (goRightButtonPressedCount < 3 || goLeftButtonPressedCount < 3))
            {
                var alpha = tutorialFirstStep.GetComponent<Image>().color.a + Time.deltaTime * tutorialWindowsAppearingSpeed;
                if (alpha > 1)
                    alpha = 1;
                tutorialFirstStep.GetComponent<Image>().color = new Color(tutorialFirstStep.GetComponent<Image>().color.r, 
                    tutorialFirstStep.GetComponent<Image>().color.g, 
                    tutorialFirstStep.GetComponent<Image>().color.b, alpha);
                tutorialFirstStep.transform.GetChild(0).gameObject.GetComponent<Text>().color 
                    = new Color(tutorialFirstStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.r,
                    tutorialFirstStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.g,
                    tutorialFirstStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.b, alpha);
            }
            else if (tutorialFirstStep.GetComponent<Image>().color.a == 1 
                && (goRightButtonPressedCount < 3 || goLeftButtonPressedCount < 3))
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    goLeftButtonPressedCount++;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    goRightButtonPressedCount++;
                }
            }
            else if (tutorialFirstStep.GetComponent<Image>().color.a > 0
                && goRightButtonPressedCount >= 3 && goLeftButtonPressedCount >= 3)
            {
                var alpha = tutorialFirstStep.GetComponent<Image>().color.a - Time.deltaTime * tutorialWindowsAppearingSpeed;
                if (alpha < 0)
                    alpha = 0;
                tutorialFirstStep.GetComponent<Image>().color = new Color(tutorialFirstStep.GetComponent<Image>().color.r,
                    tutorialFirstStep.GetComponent<Image>().color.g,
                    tutorialFirstStep.GetComponent<Image>().color.b, alpha);
                tutorialFirstStep.transform.GetChild(0).gameObject.GetComponent<Text>().color
                    = new Color(tutorialFirstStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.r,
                    tutorialFirstStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.g,
                    tutorialFirstStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.b, alpha);
            }
            else if (tutorialFirstStep.GetComponent<Image>().color.a == 0
                && goRightButtonPressedCount >= 3 && goLeftButtonPressedCount >= 3)
            {
                isShowingFirstStep = false;
                isShowingSecondStep = true;
            }
        }
        if (isShowingSecondStep)
        {
            if (!tutorialSecondStep.activeSelf)
            {
                tutorialSecondStep.SetActive(true);
            }
            if (tutorialSecondStep.GetComponent<Image>().color.a < 1
                && (accelerateButtonPressedCount < 2 || decelerateButtonPressedCount < 2))
            {
                var alpha = tutorialSecondStep.GetComponent<Image>().color.a + Time.deltaTime * tutorialWindowsAppearingSpeed;
                if (alpha > 1)
                    alpha = 1;
                tutorialSecondStep.GetComponent<Image>().color = new Color(tutorialSecondStep.GetComponent<Image>().color.r,
                    tutorialSecondStep.GetComponent<Image>().color.g,
                    tutorialSecondStep.GetComponent<Image>().color.b, alpha);
                tutorialSecondStep.transform.GetChild(0).gameObject.GetComponent<Text>().color
                    = new Color(tutorialSecondStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.r,
                    tutorialSecondStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.g,
                    tutorialSecondStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.b, alpha);
            }
            else if (tutorialSecondStep.GetComponent<Image>().color.a == 1
                && (accelerateButtonPressedCount < 2 || decelerateButtonPressedCount < 2))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    accelerateButtonPressedCount++;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    decelerateButtonPressedCount++;
                }
            }
            else if (tutorialSecondStep.GetComponent<Image>().color.a > 0
                && accelerateButtonPressedCount >= 2 && decelerateButtonPressedCount >= 2)
            {
                var alpha = tutorialSecondStep.GetComponent<Image>().color.a - Time.deltaTime * tutorialWindowsAppearingSpeed;
                if (alpha < 0)
                    alpha = 0;
                tutorialSecondStep.GetComponent<Image>().color = new Color(tutorialSecondStep.GetComponent<Image>().color.r,
                    tutorialSecondStep.GetComponent<Image>().color.g,
                    tutorialSecondStep.GetComponent<Image>().color.b, alpha);
                tutorialSecondStep.transform.GetChild(0).gameObject.GetComponent<Text>().color
                    = new Color(tutorialSecondStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.r,
                    tutorialSecondStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.g,
                    tutorialSecondStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.b, alpha);
            }
            else if (tutorialSecondStep.GetComponent<Image>().color.a == 0
                && accelerateButtonPressedCount >= 2 && decelerateButtonPressedCount >= 2)
            {
                isShowingSecondStep = false;
                isShowingThirdStep = true;
            }
        }
        if (isShowingThirdStep)
        {
            if (!tutorialThirdStep.activeSelf)
            {
                tutorialThirdStep.SetActive(true);
            }
            if (tutorialThirdStep.GetComponent<Image>().color.a < 1 && !isUnshowingThirdStep)
            {
                var alpha = tutorialThirdStep.GetComponent<Image>().color.a + Time.deltaTime * tutorialWindowsAppearingSpeed;
                if (alpha > 1)
                    alpha = 1;
                tutorialThirdStep.GetComponent<Image>().color = new Color(tutorialThirdStep.GetComponent<Image>().color.r,
                    tutorialThirdStep.GetComponent<Image>().color.g,
                    tutorialThirdStep.GetComponent<Image>().color.b, alpha);
                tutorialThirdStep.transform.GetChild(0).gameObject.GetComponent<Text>().color
                    = new Color(tutorialThirdStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.r,
                    tutorialThirdStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.g,
                    tutorialThirdStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.b, alpha);
            }
            else if (tutorialThirdStep.GetComponent<Image>().color.a == 1 && !isUnshowingThirdStep)
            {
                thirdStepShowingTime += Time.deltaTime;
                if(thirdStepShowingTime > 5)
                {
                    isUnshowingThirdStep = true;
                }
            }
            else if (tutorialThirdStep.GetComponent<Image>().color.a > 0
                && isUnshowingThirdStep)
            {
                var alpha = tutorialThirdStep.GetComponent<Image>().color.a - Time.deltaTime * tutorialWindowsAppearingSpeed;
                if (alpha < 0)
                    alpha = 0;
                tutorialThirdStep.GetComponent<Image>().color = new Color(tutorialThirdStep.GetComponent<Image>().color.r,
                    tutorialThirdStep.GetComponent<Image>().color.g,
                    tutorialThirdStep.GetComponent<Image>().color.b, alpha);
                tutorialThirdStep.transform.GetChild(0).gameObject.GetComponent<Text>().color
                    = new Color(tutorialThirdStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.r,
                    tutorialThirdStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.g,
                    tutorialThirdStep.transform.GetChild(0).gameObject.GetComponent<Text>().color.b, alpha);
            }
            else if (tutorialThirdStep.GetComponent<Image>().color.a == 0
                && isUnshowingThirdStep)
            {
                isShowingThirdStep = false;
                isUnshowingThirdStep = false;
                EndTutorial();
            }
        }

    }

    private void EndTutorial()
    {
        isPlayingTutorial = false;
        RoadManager.EndTutorial();
    }

    private void OnPauseButtonClick()
    {
        if (!isOnPause && !loseBackground.activeSelf && !winBackground.activeSelf)
        {
            audioManager.GetComponent<AudioManager>().StopMotorSound();
            pauseBackground.SetActive(true);
            Time.timeScale = 0;
            isOnPause = true;
        }
        else if (isOnPause)
        {
            audioManager.GetComponent<AudioManager>().ContinueMotorSound();
            pauseBackground.SetActive(false);
            Time.timeScale = 1;
            isOnPause = false;
        }
    }

    public void SetGivenTime(int[] givenTime)
    {
        this.givenTime[0] = givenTime[0];
        this.givenTime[1] = givenTime[1];
        this.givenTime[2] = givenTime[2];
    }

    private void CalculateTimer()
    {
        if (isTimerStart)
        {
            t += Time.deltaTime;
            if (t >= 1)
            {
                t--;
                if (!RoadManager.IsEndlessMode)
                {
                    if (givenTime[2] != 0)
                    {
                        givenTime[2]--;
                    }
                    else if (givenTime[1] != 0)
                    {
                        givenTime[1]--;
                        givenTime[2] = 59;
                    }
                    else
                    {
                        Lose(LoseReason.TimeIsOver);
                    }
                }
                else if (RoadManager.IsEndlessMode)
                {
                    if (givenTime[2] < 60)
                    {
                        givenTime[2]++;
                    }
                    else if (givenTime[1] < 60)
                    {
                        givenTime[1]++;
                        givenTime[2] = 0;
                    }
                    else if (givenTime[0] < 60)
                    {
                        givenTime[0]++;
                        givenTime[1] = 0;
                        givenTime[2] = 0;
                    }
                }
            }
        }
    }

    private void UpdateTimer()
    {
        if (isTimerStart)
        {
            var hours = "";
            if (RoadManager.IsEndlessMode)
            {
                hours = "0";
                if (givenTime[0] < 10)
                {
                    hours += givenTime[0].ToString();
                }
                else
                {
                    hours = givenTime[0].ToString();
                }
                hours += ":";
            }
            var minutes = "0";
            if (givenTime[1] < 10)
            {
                minutes += givenTime[1].ToString();
            }
            else
            {
                minutes = givenTime[1].ToString();
            }
            minutes += ":";
            var seconds = "0";
            if (givenTime[2] < 10)
            {
                seconds += givenTime[2].ToString();
            }
            else
            {
                seconds = givenTime[2].ToString();
            }
            timeLeft.GetComponent<Text>().text = "Время " + hours + minutes + seconds;
        }
    }

    public void IncreaseCoinsCount(int value)
    {
        coins += value;
        if (!RoadManager.IsEndlessMode)
        {
            coinsCount.GetComponent<Text>().text = "Монеты " + coins.ToString() + "/" + RoadManager.CoinsCountAim.ToString();
            if (coins >= RoadManager.CoinsCountAim)
            {
                Win(WinReason.CoinsCollected);
            }
        }
        else
        {
            coinsCount.GetComponent<Text>().text = "Монеты " + coins.ToString();
        }
    }

    public void Win(WinReason winReason)
    {
        audioManager.GetComponent<AudioManager>().StopMotorSound();
        Time.timeScale = 0;
        if (winReason == WinReason.CoinsCollected)
        {
            winReasonText.GetComponent<Text>().text = "Монеты собраны";
        }
        winBackground.SetActive(true);
    }

    public void OnContinueButtonClick()
    {
        Menu.CompleteLevel(RoadManager.CurrentLevelNumber);
        SceneManager.LoadScene("Menu");
        SceneManager.UnloadScene("Game");
    }

    public void Lose(LoseReason loseReason)
    {
        audioManager.GetComponent<AudioManager>().StopMotorSound();
        Time.timeScale = 0;
        if(loseReason == LoseReason.TimeIsOver)
        {
            loseReasonText.GetComponent<Text>().text = "Кончилось время";
        }
        else if(loseReason == LoseReason.CarCrash)
        {
            loseReasonText.GetComponent<Text>().text = "Столкновение с машиной";
        }
        else if (loseReason == LoseReason.FenceCrash)
        {
            loseReasonText.GetComponent<Text>().text = "Столкновение с оградой";
        }
        loseBackground.SetActive(true);
    }

    public void OnExitGameButtonClick()
    {
        if (RoadManager.IsEndlessMode)
        {
            if(coins > Menu.EndlessLevelCoinsCountRecord)
            {
                Menu.SetNewRecord(coins);
            }
        }
        Menu.SetWasOnLevel();
        SceneManager.LoadScene("Menu");
        SceneManager.UnloadScene("Game");
    }

    public void OnReplayButtonClick()
    {
        if (RoadManager.IsEndlessMode)
        {
            if (coins > Menu.EndlessLevelCoinsCountRecord)
            {
                Menu.SetNewRecord(coins);
            }
        }
        SceneManager.UnloadScene("Game");
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
}
