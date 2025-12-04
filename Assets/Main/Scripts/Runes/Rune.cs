using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Rune : MonoBehaviour, IInteract
{
    [CanBeNull] public RuneEvents Events { get; protected set; }
    [CanBeNull] public RuneAfterEvents AfterEvents { get; protected set; }
    [Tooltip("Is used to determine if the rune can be placed on a alter")][SerializeField] protected AlterFilter placeOnAlterFilter;
    [NonSerialized][CanBeNull] public Alter alter;
    [SerializeField] bool resetPositionWhenDropedOrKicked;
    protected Vector3 originalPosition;
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
        if (placeOnAlterFilter.RunClusterFilter(cluster) == false)
            return false;
        if (placeOnAlterFilter.RunAlterFilter(alter) == false)
            return false;
        if (placeOnAlterFilter.RunRuneFilter(alter.clusterIndex, cluster.alters) == false)
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
    public virtual void AfterDropped()
    {
        AfterEvents?.afterDrop.Invoke();
        if (resetPositionWhenDropedOrKicked)
            ResetPosition();

    }
    public virtual void AfterKicked()
    {
        AfterEvents?.afterAlterKicked.Invoke();
        if (resetPositionWhenDropedOrKicked)
            ResetPosition();
    }
    public virtual void AfterAlterPlace() => AfterEvents?.afterAlterPlaced.Invoke();


    public void ResetPosition()
    {
        transform.position = originalPosition;
    }

    protected virtual void Awake()
    {
        tags.Init();
        Events = GetComponent<RuneEvents>();
        AfterEvents = GetComponent<RuneAfterEvents>();
        originalPosition = transform.position;
    }
    protected virtual void Start()
    {
    }

    protected virtual void BulletInteract(InteractData data)
    {
    }

    public virtual void OnInteract(InteractData data)
    {
        switch (data.type)
        {
            case InteractType.Player:

                if (Inventory.PlayerInventory.TryPickUpRune(this) == false)
                    return;
                OnGroundPickUp();

                break;
            case InteractType.Bullet:
                BulletInteract(data);
                break;
        }
    }

    public (int thisAlterIndex, Alter[] alters) GetAlters() => alter.GetAlters();


}
