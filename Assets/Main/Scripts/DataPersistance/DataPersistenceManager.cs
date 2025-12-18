using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage")]
    [SerializeField] string fileName;

    [Header("Developer Tools")]
    [SerializeField] bool DebugLogWhereDoesItSave;
    [SerializeField] bool DeleteSaveFile;
    [SerializeField] private bool TriggerSave;

    private GameData gameData;
    private List<IDataPersitiens> dataPersistenceObjcets;

    public FileDataHandler DataHandler { get; private set; }
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        gameData = null;
    }
    private void Start()
    {
        this.DataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjcets = FindAllDataPersistenceObjects();
        LoadGame();
    }

    void TryInit()
    {
        if (DataHandler != null)
            return;
        this.DataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    public void NewGameData()
    {
        gameData = new GameData();
    }
    void LoadGameDataFromFile()
    {
        TryInit();
        this.gameData = DataHandler.Load();

        if (gameData != null)
            return;

        NewGameData();
        Debug.Log("created savefile at: " + Application.persistentDataPath + "/" + fileName);


    }
    public void CallLoadData(IDataPersitiens persistens)
    {
        LoadGameDataFromFile();
        persistens.LoadData(gameData);
    }
    public void LoadGame()
    {
        LoadGameDataFromFile();
        foreach (IDataPersitiens dataPer in dataPersistenceObjcets)
            dataPer.LoadData(gameData);
    }
    public void SaveData()
    {
        gameData.isSavingGameData = true;
        Save();
    }

    public void SaveDataExcludeGameData()
    {
        gameData.isSavingGameData = false;
        Save();
    }

    void Save()
    {
        foreach (IDataPersitiens dataPer in dataPersistenceObjcets)
            dataPer.SaveData(ref gameData);

        DataHandler.Save(gameData);
    }
    public void DeleteData()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path) == false)
        {
            Debug.Log("File already deleted");
            return;
        }
        File.Delete(path);
            
        Debug.Log("Deleted file at: " + Application.persistentDataPath + "/" + fileName);
    }
    public void WriteSaveFile()
    { DataHandler.Save(gameData); }
    public void NewLevelDataReset()
    { gameData.NewLevelDataReset(); }
    private void OnApplicationQuit()
    {
        SaveDataExcludeGameData();
    }
    private List<IDataPersitiens> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersitiens> dataPersistenceObjcets = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersitiens>();

        return new List<IDataPersitiens>(dataPersistenceObjcets);
    }

    void OnValidate()
    {
        if (DebugLogWhereDoesItSave)
        {
            DebugLogWhereDoesItSave = false;
            Debug.Log("Save file location: " + Application.persistentDataPath + "/" + fileName);
        }

        if (DeleteSaveFile)
        {
            DeleteSaveFile = false;
            DeleteData();
        }

        if (TriggerSave)
        {
            gameData.isSavingGameData = true;
            TriggerSave = false;
            
            FileDataHandler handler = new FileDataHandler(Application.persistentDataPath, fileName);
            List<IDataPersitiens> saveObjects = FindAllDataPersistenceObjects();
            
            foreach (IDataPersitiens dataPer in saveObjects)
                dataPer.SaveData(ref gameData);

            handler.Save(gameData);
            Debug.Log("Saved to location: " + Application.persistentDataPath + "/" + fileName);
        }
    }
}
