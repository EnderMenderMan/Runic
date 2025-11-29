using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Rune : MonoBehaviour, IInteract
{
    [CanBeNull] public RuneEvents Events { get; protected set; }
    [SerializeField] protected AlterFilter alterFilter;
    [NonSerialized][CanBeNull] public Alter alter;
    [field: SerializeField] public Tags tags;
    public bool IsInteractDisabled { get; set; }

    public virtual void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {

    }
    public virtual bool TryBePlaced(int alterIndex, Alter[] alters, AlterCluster cluster)
    {
        return TryBePlacedWithAlterFilter(alters[alterIndex], cluster);
    }

    public virtual bool TryBePlacedWithAlterFilter(Alter alter, AlterCluster cluster)
    {
        if (alterFilter.clusterFilterType == FilterType.Exclusive && alter.tags.Contains(alterFilter.clusterFilter) == true)
            return false;
        if (alterFilter.clusterFilterType == FilterType.Inclusive && alter.tags.Contains(alterFilter.clusterFilter) == false)
            return false;

        if (alterFilter.alterFilterType == FilterType.Exclusive && alter.tags.Contains(alterFilter.alterFillter) == true)
            return false;
        if (alterFilter.alterFilterType == FilterType.Inclusive && alter.tags.Contains(alterFilter.alterFillter) == false)
            return false;
        
        Events?.onAlterPlaced.Invoke();
        return true;
    }
    
    public virtual void OnPickUp() => Events?.onPickup.Invoke();
    public virtual void OnGroundPickUp() => Events?.onGroundPickup.Invoke();
    public virtual void OnAlterPickUp() => Events?.onAlterPickup.Invoke();
    public virtual void OnDropped() => Events?.onDrop.Invoke();
    public virtual void OnKicked() => Events?.onAlterKicked.Invoke();
    
    protected virtual void Awake()
    {
        tags.Init();
        Events = GetComponent<RuneEvents>();
    }

    public virtual void OnInteract()
    {
        if (Inventory.PlayerInventory.TryPickUpRune(this) == false)
            return;
        OnGroundPickUp();
    }


}
