using System;
using JetBrains.Annotations;
using UnityEngine;

public abstract class Rune : MonoBehaviour, IInteract
{
    [NonSerialized][CanBeNull] public Alter alter;
    [field: SerializeField] public int ValueID { get; protected set; }
    public bool IsInteractDisabled { get; set; }

    public virtual void TriggerPillarPlacement(int itemIndex, Alter[] pillars)
    {

    }


    public virtual void OnInteract()
    {
        Inventory.PlayerInventory.PickUpRune(this);
    }
}
