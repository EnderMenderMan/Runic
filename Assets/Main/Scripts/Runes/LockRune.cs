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
    [SerializeField] LockIndexes[] lockFilters;
    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
        foreach (LockIndexes filter in lockFilters)
        {
            foreach (int index in filter.lockOffsetIndexes)
            {
                LockAlter(alters[index], filter);
            }
        }
    }

    void LockAlter(Alter alter, LockIndexes filter)
    {
        if (alter.equippedRune == null && filter.lockEmptyAlters == false)
            return;
    }
}
