using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class LevelLoad : MonoBehaviour
{
    [NonSerialized] public int levelIndex = -1;
    [Tooltip("if true will enable this gameobject (SetActive(true)) when this level is loaded (the function LoadLevel() is called)")][SerializeField] bool enableGameObjectOnLoad = true;
    [Tooltip("if true when this gameobject gets enabled (SetActive(true)) then call the function OnThisLevelEnter()")][SerializeField] bool enableAutoCallOnThisLevelEnter = true;
    [Tooltip("if true then this level will trigger progression of journal hints")][SerializeField] private bool triggerJournalHint = true;
    [SerializeField] UnityEvent onLevelLoad;
    private bool hasLoaded = false;
    public void LoadLevel()
    {
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
        if (triggerJournalHint && GameData.difficulty == GameData.Difficulty.Normal)
            Journal.Instance.TriggerNextHint();
    }

    IEnumerator CallOnLevelEnterNextFrame()
    {
        yield return null;
        onLevelLoad?.Invoke();
        OnThisLevelEnter();
        if (triggerJournalHint && GameData.difficulty == GameData.Difficulty.Easy)
            Journal.Instance.TriggerNextHint();
        hasLoaded = false;
    }
}
