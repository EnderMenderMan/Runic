using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IDataPersitiens
{
    [SerializeField] GameObject startButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] TMPro.TMP_Dropdown dropDownDifficulty;

    public void ResetGameData()
    {
        DataPersistenceManager.Instance.NewLevelDataReset();
        startButton.SetActive(true);
        continueButton.SetActive(false);
    }
    public void DeleteData()
    {
        DataPersistenceManager.Instance.DeleteData();
        DataPersistenceManager.Instance.LoadGame();
        startButton.SetActive(true);
        continueButton.SetActive(false);
    }
    public void ChangeDifficulty(int value)
    {
        GameData.difficulty = (GameData.Difficulty)value;
    }

    private void OnDisable()
    {
        DataPersistenceManager.Instance.SaveDataExcludeGameData();
    }

    public void LoadData(GameData data)
    {
        dropDownDifficulty.value = (int)GameData.difficulty;

        if (data.loadedSceneIndex < 0)
            return;
        startButton.SetActive(false);
        continueButton.SetActive(true);
    }

    public void SaveData(ref GameData data)
    {
        GameData.difficulty = (GameData.Difficulty)dropDownDifficulty.value;
    }
}
