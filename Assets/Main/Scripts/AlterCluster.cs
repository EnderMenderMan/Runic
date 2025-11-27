using System;
using System.Collections.Generic;
using UnityEngine;

public class AlterCluster : MonoBehaviour
{
    private Alter[] alters;

    public void TriggerItemPlacement(int pillarIndex)
    {
        alters[pillarIndex].equippedRune.TriggerRunePlacement(pillarIndex, alters);
    }
    public bool CanItemBePlaced(Rune rune, int alterIndex)
    {
        return rune.TryBePlaced(alterIndex, alters);
    }
    private void Awake()
    {
        SetUpAllConnectedAlters();
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
