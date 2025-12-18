using System;
using UnityEngine;

public class MainMenu : MonoBehaviour, IDataPersitiens
{
    [SerializeField] GameObject startButton;
    [SerializeField] private GameObject continueButton;

    public void ResetGameData()
    {
        DataPersistenceManager.Instance.NewLevelDataReset();
        startButton.SetActive(true);
        continueButton.SetActive(false);
    }

    private void OnDisable()
    {
        DataPersistenceManager.Instance.SaveDataExcludeGameData();
    }

    public void LoadData(GameData data)
    {
        if (data.loadedSceneIndex < 0)
            return;
        startButton.SetActive(false);
        continueButton.SetActive(true);
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
