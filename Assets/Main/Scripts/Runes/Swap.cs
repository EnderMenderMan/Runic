using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
public class Swap : Rune
{
    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
       // base.TriggerRunePlacement(itemIndex, alters);
        Console.WriteLine("hi");
        Debug.Log("hiii");
        //alter.equippedRune.transform.position 
        // if (alter.clusterIndex == 0)
        //transform.position = alter.equippedRune.transform.position;
        //alter.equippedRune.transform.position = transform.position;

        Console.WriteLine("hi");
        Debug.Log("hiii");

    }
}
