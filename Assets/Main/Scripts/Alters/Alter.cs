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
    [SerializeField] UnityEvent OnRunePlaced;

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
        OnRunePlaced.Invoke();
    }
    void DropRune()
    {
        equippedRune.IsInteractDisabled = false;
        equippedRune = null;
    }
    void Awake()
    {
        tags.Init();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInteract()
    {
        if (Inventory.PlayerInventory.heldRune && equippedRune)
        {
            Rune heldRune = Inventory.PlayerInventory.heldRune;
            Inventory.PlayerInventory.ForceDropRune();
            Inventory.PlayerInventory.PickUpRune(equippedRune);
            KickItem();
            PlaceItem(heldRune);
            return;
        }

        if (equippedRune)
        {
            Rune rune = equippedRune;
            KickItem();
            Inventory.PlayerInventory.PickUpRune(rune);
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
