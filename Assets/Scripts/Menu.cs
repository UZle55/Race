using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuBackground;
    [SerializeField] private GameObject levelChooseBackground;
    [SerializeField] private GameObject[] playLevelButtons;
    [SerializeField] private GameObject clearSavesConfirmPanel;
    [SerializeField] private GameObject endlessLevelCoinsCountRecordText;
    public static int CompletedLevelNumber { get; private set; } = -1;
    public static bool WasOnLevel { get; private set; }
    private bool isActivatedButton = false;
    private static List<int> CompletedLevels = new List<int>();
    public static int EndlessLevelCoinsCountRecord { get; private set; }
    private static bool isFirstStart = true;
    // Start is called before the first frame update
    void Start()
    {
        if (isFirstStart)
        {
            LoadProgress();
            isFirstStart = false;
        }
        Time.timeScale = 1;
        if (WasOnLevel)
        {
            OnPlayButtonClick();
            WasOnLevel = false;
        }
        if(CompletedLevels.Count != 0)
        {
            foreach(var completedLevelNumber in CompletedLevels)
            {
                playLevelButtons[completedLevelNumber + 1].GetComponent<Button>().interactable = true;
            }
        }
        UpdateRecord();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelChooseBackground.activeSelf)
        {
            if (!isActivatedButton)
            {
                if(CompletedLevelNumber + 1 < playLevelButtons.Length)
                {
                    playLevelButtons[CompletedLevelNumber + 1].GetComponent<Button>().interactable = true;
                    CompletedLevels.Add(CompletedLevelNumber);
                }
                isActivatedButton = true;
            }
        }
        UpdateRecord();
    }

    private void SaveProgress()
    {
        var completedLevelsStr = "";
        if (CompletedLevels.Count != 0)
        {
            completedLevelsStr += CompletedLevels[0].ToString();
            for (var i = 1; i < CompletedLevels.Count; i++)
            {
                completedLevelsStr += " " + CompletedLevels[i];
            }
        }
        PlayerPrefs.SetString("CompletedLevels", completedLevelsStr);
        PlayerPrefs.SetInt("EndlessRecord", EndlessLevelCoinsCountRecord);
    }

    private void LoadProgress()
    {
        var completedLevelsStr = PlayerPrefs.GetString("CompletedLevels");
        if (completedLevelsStr != null && completedLevelsStr.Length != 0)
        {
            var completedLevelsSplitted = completedLevelsStr.Split(" ");
            foreach (var levelNumber in completedLevelsSplitted)
            {
                CompletedLevels.Add(int.Parse(levelNumber));
            }
        }
        EndlessLevelCoinsCountRecord = PlayerPrefs.GetInt("EndlessRecord");
    }

    private void UpdateRecord()
    {
        endlessLevelCoinsCountRecordText.GetComponent<Text>().text = "Рекорд " + EndlessLevelCoinsCountRecord.ToString() + " монет";
    }

    public void OnClearSavesButtonClick()
    {
        clearSavesConfirmPanel.SetActive(true);
    }

    public void OnConfirmClearSavesButtonClick()
    {
        clearSavesConfirmPanel.SetActive(false);
        ClearSaves();
    }

    public void OnCancelClearSavesButtonClick()
    {
        clearSavesConfirmPanel.SetActive(false);
    }

    private void ClearSaves()
    {
        PlayerPrefs.DeleteAll();
        CompletedLevelNumber = -1;
        WasOnLevel = false;
        CompletedLevels = new List<int>();
        for(var i = 1; i < playLevelButtons.Length; i++)
        {
            playLevelButtons[i].GetComponent<Button>().interactable = false;
        }
        EndlessLevelCoinsCountRecord = 0;
    }

    public void OnPlayButtonClick()
    {
        levelChooseBackground.SetActive(true);
        mainMenuBackground.SetActive(false);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void OnExitLevelChooseButtonClick()
    {
        SaveProgress();
        levelChooseBackground.SetActive(false);
        mainMenuBackground.SetActive(true);
    }

    public void OnPlayTutorialLevelButtonClick()
    {
        RoadManager.SetSettings(true, new int[] { 0, 1, 0 }, 10, false, 0);
        SceneManager.LoadScene("Game");
        SceneManager.UnloadScene("Menu");
    }

    public void OnPlayFirstLevelButtonClick()
    {
        RoadManager.SetSettings(false, new int[] { 0, 1, 0 }, 20, false, 1);
        SceneManager.LoadScene("Game");
        SceneManager.UnloadScene("Menu");
    }

    public void OnPlaySecondLevelButtonClick()
    {
        RoadManager.SetSettings(false, new int[] { 0, 1, 30 }, 35, false, 2);
        SceneManager.LoadScene("Game");
        SceneManager.UnloadScene("Menu");
    }

    public void OnPlayThirdLevelButtonClick()
    {
        RoadManager.SetSettings(false, new int[] { 0, 2, 0 }, 55, false, 3);
        SceneManager.LoadScene("Game");
        SceneManager.UnloadScene("Menu");
    }

    public void OnPlayFouthLevelButtonClick()
    {
        RoadManager.SetSettings(false, new int[] { 0, 2, 30 }, 80, false, 4);
        SceneManager.LoadScene("Game");
        SceneManager.UnloadScene("Menu");
    }

    public void OnPlayEndlessLevelButtonClick()
    {
        RoadManager.SetSettings(false, new int[] { 0, 0, 0 }, 0, true, 5);
        SceneManager.LoadScene("Game");
        SceneManager.UnloadScene("Menu");
    }

    public static void CompleteLevel(int levelNumber)
    {
        CompletedLevelNumber = levelNumber;
        WasOnLevel = true;
    }

    public static void SetWasOnLevel()
    {
        WasOnLevel = true;
    }

    public static void SetNewRecord(int record)
    {
        EndlessLevelCoinsCountRecord = record;
    }
}
