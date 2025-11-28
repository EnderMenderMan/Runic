using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public abstract class Rune : MonoBehaviour, IInteract
{
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
        return true;
    }

    protected virtual void Awake()
    {
        tags.Init();
    }

    public virtual void OnInteract()
    {
        Inventory.PlayerInventory.PickUpRune(this);
    }


}
