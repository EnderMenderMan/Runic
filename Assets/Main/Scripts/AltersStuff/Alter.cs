using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Alter : MonoBehaviour, IInteract
{
    [field: SerializeField] public Tags tags;
    private AlterCluster alterCluster;
    private int clusterIndex;
    [SerializeField] Vector3 kickOffset;
    [CanBeNull] public Rune equippedRune;
    [CanBeNull] public AlterEvents Events { get; private set; }

    public bool IsInteractDisabled { get; set; }

    public void ConnectToCluster(AlterCluster cluster, int clusterIndex)
    {
        alterCluster = cluster;
        this.clusterIndex = clusterIndex;
    }
    public void KickItem()
    {
        if (equippedRune == null)
            return;
        equippedRune.IsInteractDisabled = false;
        equippedRune.transform.position = transform.position + kickOffset;
        equippedRune.alter = null;
        equippedRune = null;
    }

    public void PlaceItem(Rune rune)
    {
        if (alterCluster.CanItemBePlaced(rune, clusterIndex) == false)
            return;
        KickItem();
        this.equippedRune = rune;
        rune.IsInteractDisabled = true;
        rune.alter = this;
        rune.transform.position = transform.position;
        alterCluster.TriggerItemPlacement(clusterIndex);
        Events?.onRunePlaced.Invoke();
    }
    void DropRune()
    {
        equippedRune.IsInteractDisabled = false;
        equippedRune = null;
    }
    void Awake()
    {
        tags.Init();
        Events = GetComponent<AlterEvents>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    bool PlayerTryPickUp(Rune rune)
    {
        if (Inventory.PlayerInventory.TryPickUpRune(rune) == false)
            return false;
        rune.OnAlterPickUp();
        return true;
    }
    public void OnInteract()
    {
        if (Inventory.PlayerInventory.heldRune && equippedRune)
        {
            Rune heldRune = Inventory.PlayerInventory.heldRune;
            Inventory.PlayerInventory.ForceDropRune();
            PlayerTryPickUp(equippedRune);
            KickItem();
            PlaceItem(heldRune);
            return;
        }

        if (equippedRune)
        {
            Rune rune = equippedRune;
            KickItem();
            PlayerTryPickUp(rune);
            return;
        }

        if (Inventory.PlayerInventory.heldRune)
        {
            Rune rune = Inventory.PlayerInventory.heldRune;
            Inventory.PlayerInventory.ForceDropRune();
            PlaceItem(rune);
            return;
        }
    }
}
