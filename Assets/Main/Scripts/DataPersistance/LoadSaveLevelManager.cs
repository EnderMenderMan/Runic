using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DataPersistenceManager))]
public class LoadSaveLevelManager : MonoBehaviour, IDataPersitiens
{
    public static int levelIndex;
    [SerializeField] LevelLoad[] levels;
    bool hasLoaded;

    public void LoadData(GameData data)
    {
        if (hasLoaded)
            return;
        hasLoaded = true;

        if (data.loadedSceneIndex > 0 && data.loadedSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(data.loadedSceneIndex);
            return;
        }

        int levelIndex = 0;
        if (data.loadedLevelIndex > 0)
        {

            levelIndex = data.loadedLevelIndex;
            FindAnyObjectByType<PlayerInteract>().transform.position = new Vector3(data.playerPosition.x, data.playerPosition.y, 0);
        }

        levels[levelIndex].LoadLevel();
    }

    public void SaveData(ref GameData data)
    {
        if (data.isSavingGameData == false)
            return;
        
        data.loadedLevelIndex = levelIndex;
        data.loadedSceneIndex = SceneManager.GetActiveScene().buildIndex;
        data.playerPosition.x = PlayerInteract.Instance.transform.position.x;
        data.playerPosition.y = PlayerInteract.Instance.transform.position.y;
    }

    void Awake()
    {
        for (int i = 0; i < levels.Length; i++)
            levels[i].levelIndex = i;
        
        DataPersistenceManager dataPresistence = GetComponent<DataPersistenceManager>();
        if (dataPresistence.enabled)
            dataPresistence.CallLoadData(this);
    }
}
