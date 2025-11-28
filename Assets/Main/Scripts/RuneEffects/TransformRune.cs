using System;
using System.Linq;
using UnityEngine;

public class TransformRune : Rune
{
    Rune selectedChild;
    [SerializeField] Rune[] runes;
    int selectedRuneIndex;
    protected override void Awake()
    {
        runes = GetComponentsInChildren<Rune>();
        base.Awake();
    }

    public void SwapSelectedRune()
    {
        if (runes.Length == 1)
            return;
        if (selectedRuneIndex == runes.Length - 1) // skip this rune (TransformRune) 
            selectedRuneIndex = 0;

        selectedRuneIndex++;
        selectedRuneIndex %= runes.Length;

        selectedChild.gameObject.SetActive(false);
        if (selectedChild.alter != null)
            SwapAlterPlace(selectedChild.alter);
        selectedChild = runes[selectedRuneIndex];
    }

    void SwapAlterPlace(Alter alter)
    {
        alter.equippedRune = runes[selectedRuneIndex];
    }

    public override void TriggerRunePlacement(int itemIndex, Alter[] alters) => selectedChild.TriggerRunePlacement(itemIndex, alters);
    public override bool TryBePlaced(int alterIndex, Alter[] alters, AlterCluster cluster) => selectedChild.TryBePlaced(alterIndex, alters, cluster);
}
