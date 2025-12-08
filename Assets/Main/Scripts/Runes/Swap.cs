using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
public class Swap : Rune
{
    [SerializeField] GameObject SwapWith;

    public override void AfterAlterPlace()
    {
        base.AfterAlterPlace();
        Position1();
        Vector3 position1 = Position1();
        transform.position = SwapWith.transform.position;
        SwapWith.transform.position = position1;
        Position2();
        string position2 = Position2();
        Debug.Log($"Position2: {position2}");
        Debug.Log($"Position1: {position1}");
    }

    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
        // base.TriggerRunePlacement(itemIndex, alters);


    }
    public Vector3 Position1()
    {
        return transform.position;
    }
    public string Position2()
    {
        return transform.position.ToString();
    }
}
