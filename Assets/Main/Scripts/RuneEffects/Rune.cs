using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public abstract class Rune : MonoBehaviour, IInteract
{
    [SerializeField] protected AlterFilter alterFilter;
    [NonSerialized][CanBeNull] public Alter alter;
    [field: SerializeField] public int ValueID { get; protected set; }
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
        if (alterFilter.clusterFilterType == FilterType.Exclusive && alterFilter.clusterFilter.Contains(cluster.ValueID) == true)
            return false;
        if (alterFilter.clusterFilterType == FilterType.Inclusive && alterFilter.clusterFilter.Contains(cluster.ValueID) == false)
            return false;

        if (alterFilter.alterFilterType == FilterType.Exclusive && alterFilter.alterFillter.Contains(alter.ValueID) == true)
            return false;
        if (alterFilter.alterFilterType == FilterType.Inclusive && alterFilter.alterFillter.Contains(alter.ValueID) == false)
            return false;
        return true;
    }



    public virtual void OnInteract()
    {
        Inventory.PlayerInventory.PickUpRune(this);
    }


}
