using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class LevelLoad : MonoBehaviour
{
    [NonSerialized] public int levelIndex = -1;
    [Tooltip("if true will enable this gameobject (SetActive(true)) when this level is loaded (the function LoadLevel() is called)")][SerializeField] bool enableGameObjectOnLoad = true;
    [Tooltip("if true when this gameobject gets enabled (SetActive(true)) then call the function OnThisLevelEnter()")][SerializeField] bool enableAutoCallOnThisLevelEnter = true;
    [Tooltip("if bigger then 0 then this level will trigger progression of journal hints X amount of times. X being the this number")][SerializeField] private int triggerNextjournalHintAmount = 1;
    [SerializeField] Transform playerPositionSpawnPoint;
    [SerializeField] UnityEvent onLevelLoad;
    private bool hasLoaded = false;
    bool levelLoadedFromSave;
    public void LoadLevel()
    {
        if (playerPositionSpawnPoint != null)
            PlayerMovement.Instance.transform.position = playerPositionSpawnPoint.position;
        if (enableGameObjectOnLoad)
            gameObject.SetActive(true);

        if (hasLoaded == false)
            StartCoroutine(CallOnLevelEnterNextFrame());
        hasLoaded = true;
    }
    public void OnThisLevelEnter() => LoadSaveLevelManager.levelIndex = levelIndex;

    void OnEnable()
    {
        if (levelIndex == -1 || enableAutoCallOnThisLevelEnter == false)
            return;

        if (hasLoaded == false)
            StartCoroutine(CallOnLevelEnterNextFrame());
        hasLoaded = true;
    }

    private void OnDisable()
    {
        if (triggerNextjournalHintAmount > 0 && GameData.difficulty == GameData.Difficulty.Normal)
            for (int i = 0; i < triggerNextjournalHintAmount; i++)
            {
                Journal.Instance.TriggerNextHint();
            }
    }

    IEnumerator CallOnLevelEnterNextFrame()
    {
        yield return null;
        onLevelLoad?.Invoke();
        OnThisLevelEnter();
        DataPersistenceManager.Instance.SaveData();
        if (triggerNextjournalHintAmount > 0 && GameData.difficulty == GameData.Difficulty.Easy)
            for (int i = 0; i < triggerNextjournalHintAmount; i++)
                Journal.Instance.TriggerNextHint();
        hasLoaded = false;

    }
}
