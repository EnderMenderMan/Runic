using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Alter : MonoBehaviour, IInteract
{
    private static readonly int IsKicked = Animator.StringToHash("IsKicked");
    [field: SerializeField] public Tags tags;
    [SerializeField] Vector2 offsetRunePosition;
    public AlterCluster alterCluster { get; private set; }
    public int clusterIndex { get; private set; }
    [SerializeField] Vector3 kickOffset;
    [CanBeNull] public Rune equippedRune;
    [CanBeNull] public AlterEvents Events { get; private set; }
    [SerializeField] public SpriteRenderer AlterSymbol;
    [SerializeField] public Sprite OnSprite;
    [SerializeField] public Sprite OffSprite;

    [SerializeField] float kickDelay = 1f;
    Coroutine kickCorutine;
    bool stopkickCorutine;



    [Tooltip("Can be items be kicked by an effect. Example of such effect is kick rune")] public bool canKickItemsOrigninalValue = true;
    [Tooltip("Can player pickup from this alter")] public bool canPickupItemsOriginalValue = true;
    List<(bool canPickupItems, bool canKickItems, int id)> lockInterferance = new List<(bool, bool, int)>();
    int interferanceCount;
    bool canKickItems;
    bool canPickupItems;
    public int AddLockInterferance(bool canPickupItems, bool canKickItems)
    {
        interferanceCount++;
        lockInterferance.Add(new(canPickupItems, canKickItems, interferanceCount));
        SetLock(canPickupItems, canKickItems);
        return interferanceCount;
    }
    public void RemoveLockInterferance(int id)
    {
        for (int i = 0; i < lockInterferance.Count; i++)
        {
            if (lockInterferance[i].id != id)
                continue;

            lockInterferance.RemoveAt(i);
            if (i == 0)
            {
                ResetLockInterfenrance();
            }
            else
            {
                SetLock(lockInterferance[^1].canPickupItems, lockInterferance[^1].canKickItems);
            }
            break;
        }
    }
    public void ResetLockInterfenrance()
    {
        interferanceCount = 0;
        lockInterferance.Clear();
        SetLock(canPickupItemsOriginalValue, canKickItemsOrigninalValue);
    }
    void SetLock(bool canPickupItems, bool canKickItems)
    {
        this.canKickItems = canKickItems;
        this.canPickupItems = canPickupItems;
        if (equippedRune == null)
            return;
        if (canKickItems)
            equippedRune.animator.SetBool("IsLocked", false);
        else
            equippedRune.animator.SetBool("IsLocked", true);

    }



    public bool IsInteractDisabled { get; set; }

    public (int alterIndex, Alter[] alters) GetAlters() => (clusterIndex, alterCluster.alters);

    public void ConnectToCluster(AlterCluster cluster, int clusterIndex)
    {
        alterCluster = cluster;
        this.clusterIndex = clusterIndex;
    }
    public void StopKickCorutine()
    {
        stopkickCorutine = true;
    }
    public bool TryKickItem(bool forceDrop)
    {
        if (equippedRune == null)
            return false;
        if (forceDrop == false && canKickItems == false)
            return false;
        if (kickCorutine != null)
            StopCoroutine(kickCorutine);

        kickCorutine = StartCoroutine(KickCorutine(kickDelay));
        return true;
    }
    bool KickItemWithoutDelay(bool forceDrop = true)
    {
        if (equippedRune == null)
            return false;
        if (forceDrop == false && canKickItems == false)
            return false;

        equippedRune.OnKicked();
        if (Inventory.PlayerInventory.heldRune == null || Inventory.PlayerInventory.heldRune.gameObject != equippedRune.gameObject) // rune may have been pickup and if so dont activate it
            equippedRune.IsInteractDisabled = false;
        equippedRune.transform.position = transform.position + kickOffset;
        equippedRune.alter = null;
        equippedRune.AfterKicked();
        equippedRune = null;
        return true;
    }

    public void PlaceItem(Rune rune)
    {
        rune.OnAlterPlace();
        TryKickItem(false);
        this.equippedRune = rune;
        rune.IsInteractDisabled = true;
        rune.alter = this;
        rune.transform.position = transform.position + (Vector3)offsetRunePosition;
        alterCluster.TriggerItemPlacement(clusterIndex);
        Events?.onRunePlaced.Invoke();
        AlterSymbol.sprite = OnSprite;
        rune.AfterAlterPlace();

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
        ResetLockInterfenrance();
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
        Rune heldRune = Inventory.PlayerInventory.heldRune;
        Inventory.PlayerInventory.ShadowForceDropRune();
        if (Inventory.PlayerInventory.TryPickUpRune(rune) == false)
        { Inventory.PlayerInventory.ShadowForcePickUp(heldRune); return false; }

        rune.OnAlterPickUp();
        AlterSymbol.sprite = OffSprite;
        return true;
    }
    public bool OnInteract(InteractData data)
    {
        switch (data.type)
        {
            case InteractType.Player:
                PlayerInteract(data);
                break;
        }
        return true;

    }
    void PlayerInteract(InteractData data)
    {
        if (Inventory.PlayerInventory.heldRune && alterCluster.TryItemBePlaced(Inventory.PlayerInventory.heldRune, clusterIndex) == false)
        {
            if (Inventory.PlayerInventory.heldRune == null) // heldRune can be drop by TryItemBePlaced() function. see RuneRune TryBePlaced() override function
                return;

            Rune heldRune = Inventory.PlayerInventory.heldRune;
            Inventory.PlayerInventory.DropRune();
            if (heldRune.resetPositionWhenDropedOrKicked == false)
                heldRune.transform.position = transform.position + kickOffset;
            return;
        }

        if (Inventory.PlayerInventory.heldRune && equippedRune)
        {
            if (canPickupItems == false)
                return;

            Rune heldRune = Inventory.PlayerInventory.heldRune;

            // if (Inventory.PlayerInventory.CanPickupRune(equippedRune) == false)
            //     return;

            Rune oldEquippedRune = equippedRune;
            if (kickCorutine != null)
            {
                StopCoroutine(kickCorutine);
                OnKickCorutineEnd();
            }
            KickItemWithoutDelay(true);
            PlayerTryPickUp(oldEquippedRune);
            PlaceItem(heldRune);
            return;
        }

        if (equippedRune)
        {
            if (canPickupItems == false)
                return;

            if (Inventory.PlayerInventory.CanPickupRune(equippedRune) == false)
                return;

            Rune oldEquippedRune = equippedRune;
            if (kickCorutine != null)
            {
                StopCoroutine(kickCorutine);
                OnKickCorutineEnd();
            }
            KickItemWithoutDelay(true);
            PlayerTryPickUp(oldEquippedRune);
            return;
        }

        if (Inventory.PlayerInventory.heldRune)
        {
            Rune rune = Inventory.PlayerInventory.heldRune;
            Inventory.PlayerInventory.ShadowForceDropRune();
            PlaceItem(rune);
            return;
        }
    }

    IEnumerator KickCorutine(float kickTime)
    {
        equippedRune.countToAlterClusterComplete = false;
        equippedRune.animator.SetBool(IsKicked, true);
        while (kickTime > 0 && stopkickCorutine == false)
        {
            kickTime -= Time.deltaTime;
            yield return null;
        }
        OnKickCorutineEnd();
    }
    void OnKickCorutineEnd()
    {
        equippedRune.animator.SetBool(IsKicked, false);

        equippedRune.countToAlterClusterComplete = true;
        if (stopkickCorutine == false)
            KickItemWithoutDelay(true);
        AlterSymbol.sprite = OffSprite;
        stopkickCorutine = false;

    }
}
