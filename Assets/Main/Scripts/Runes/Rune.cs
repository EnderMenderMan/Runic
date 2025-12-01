using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Rune : MonoBehaviour, IInteract
{
    [CanBeNull] public RuneEvents Events { get; protected set; }
    [CanBeNull] public RuneAfterEvents AfterEvents { get; protected set; }
    [SerializeField] protected AlterFilter alterFilter;
    [NonSerialized][CanBeNull] public Alter alter;
    [field: SerializeField] public Tags tags;
    public bool IsInteractDisabled { get; set; }


    public virtual void TriggerAbility(int alterIndex, Alter[] alters)
    {

    }

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

        return true;
    }

    // OnEvents
    public virtual void OnPickUp() => Events?.onPickup.Invoke();
    public virtual void OnGroundPickUp() => Events?.onGroundPickup.Invoke();
    public virtual void OnAlterPickUp() => Events?.onAlterPickup.Invoke();
    public virtual void OnDropped() => Events?.onDrop.Invoke();
    public virtual void OnKicked() => Events?.onAlterKicked.Invoke();
    public virtual void OnAlterPlace() => Events?.onAlterPlaced.Invoke();
    // AfterEvents
    //public virtual void AfterPickUp() => AfterEvents?.afterPickup.Invoke();
    //public virtual void AfterGroundPickUp() => AfterEvents?.afterGroundPickup.Invoke();
    //public virtual void AfterAlterPickUp() => AfterEvents?.afterAlterPickup.Invoke();
    public virtual void AfterDropped() => AfterEvents?.afterDrop.Invoke();
    public virtual void AfterKicked() => AfterEvents?.afterAlterKicked.Invoke();
    public virtual void AfterAlterPlace() => AfterEvents?.afterAlterPlaced.Invoke();



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

    public (int thisAlterIndex, Alter[] alters) GetAlters() => alter.GetAlters();


}
