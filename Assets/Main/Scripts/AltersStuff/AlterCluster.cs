using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlterCluster : MonoBehaviour
{
    [field: SerializeField] public Tags tags;
    public UnityEvent onCompletion;
    public Alter[] alters { get; private set; }

    public bool isClusterCompleted { get; private set; }
    bool isClusterDisabled;
    public void DisableCluster()
    {
        isClusterDisabled = true;
        foreach (Alter alter in alters)
            alter.IsInteractDisabled = true;
    }
    public void EnableCluster()
    {
        isClusterDisabled = false;
        foreach (Alter alter in alters)
            alter.IsInteractDisabled = false;
    }

    public void TriggerItemPlacement(int alterIndex)
    {
        alters[alterIndex].equippedRune.TriggerRunePlacement(alterIndex, alters);
        bool hasSpace = false;
        foreach (Alter alter in alters)
        {
            if (alter.equippedRune != null)
                continue;
            hasSpace = true;
            break;
        }
        if (hasSpace == false)
        {
            isClusterCompleted = true;
            onCompletion.Invoke();
        }
        else
            isClusterCompleted = false;
    }
    public bool CanItemBePlaced(Rune rune, int alterIndex)
    {
        if (isClusterDisabled)
            return false;

        return rune.TryBePlaced(alterIndex, alters, this);
    }
    private void Awake()
    {
        SetUpAllConnectedAlters();
        tags.Init();
    }

    void SetUpAllConnectedAlters()
    {
        alters = GetComponentsInChildren<Alter>();
        for (int i = 0; i < alters.Length; i++)
        {
            alters[i].ConnectToCluster(this, i);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
