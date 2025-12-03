using System.Collections.Generic;
using UnityEngine;

public class LockRune : Rune
{
    [System.Serializable]
    public class LockIndexes
    {
        [Tooltip("Can be kicked by a rune effect. Example kick rune")] public bool isKickable;
        [Tooltip("Can be pickup by the player")] public bool isPickable;
        [Tooltip("Apply this setting to empty alters. locked alters can still be placed on but when placed can not pickup the placed rune")] public bool lockEmptyAlters;
        [Tooltip("Used to select specific rune placements to lock. IF EMPTY will lock all other runes exept this one. IF NOT EMPTY runs only selected indexes. -1 is one to the left and 2 is two to the right")] public int[] lockOffsetIndexes;
    }
    [Tooltip("Filter that determens what will be locked. First filter in array will be applyed first and can be overrided by later fillters")][SerializeField] LockIndexes[] lockFilters;
    [Tooltip("If this runes alter can be locked. If true may cause softlocks")] public bool canBeLocked;

    (Alter alter, List<int> affecteIDs)[] affectedAlters;
    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
        affectedAlters = new (Alter, List<int>)[alters.Length];
        foreach (LockIndexes filter in lockFilters)
        {
            if (filter.lockOffsetIndexes.Length == 0)
            {
                foreach (Alter alter in alters)
                    if (alter.clusterIndex != itemIndex)
                        LockAlter(alter, alter.clusterIndex, filter);
                continue;
            }
            foreach (int index in filter.lockOffsetIndexes)
            {
                LockAlter(alters[index], index, filter);
            }
        }
    }

    void LockAlter(Alter alter, int alterIndex, LockIndexes filter)
    {
        if (alter.equippedRune == null && filter.lockEmptyAlters == false)
            return;

        if (alter.equippedRune != null && alter.equippedRune is LockRune lockRune && lockRune.canBeLocked == false)
            return;

        if (affectedAlters[alterIndex].alter != null)
        {
            affectedAlters[alterIndex].affecteIDs.Add(alter.AddLockInterferance(filter.isPickable, filter.isKickable));
            return;
        }
        affectedAlters[alterIndex].alter = alter;
        affectedAlters[alterIndex].affecteIDs = new List<int>(1);
        affectedAlters[alterIndex].affecteIDs.Add(alter.AddLockInterferance(filter.isPickable, filter.isKickable));

    }
    void ReverseEffects()
    {
        foreach (var alterEffect in affectedAlters)
        {
            if (alterEffect.alter == null)
                continue;
            foreach (int id in alterEffect.affecteIDs)
                alterEffect.alter.RemoveLockInterferance(id);

            alterEffect.affecteIDs.Clear();
        }
    }
    public override void OnKicked()
    {
        ReverseEffects();
        base.OnKicked();
    }
}
