using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Rune : MonoBehaviour, IInteract
{
    [Header("Audio")]
    [SerializeField] bool enablePlaySounds = true;
    [SerializeField] AudioClip pickUpSound;
    [SerializeField] AudioClip dropSound;
    [SerializeField] AudioClip placeSound;
    [SerializeField] AudioClip kickSound;
    AudioSource audioSource;

    [CanBeNull] public RuneEvents Events { get; protected set; }
    [CanBeNull] public RuneAfterEvents AfterEvents { get; protected set; }

    [Header("Other")]
    [Tooltip("Is used to determine if the rune can be placed on a alter")][SerializeField] protected AlterFilter placeOnAlterFilter;
    [NonSerialized][CanBeNull] public Alter alter;
    public bool resetPositionWhenDropedOrKicked;
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

    protected virtual void PlaySound(SoundManager.SoundType soundType, AudioClip? audioClip)
    {
        if (enablePlaySounds == false)
            return;
        if (audioClip == null)
        {

            SoundManager.instance.PlaySound(gameObject, soundType, SoundManager.MixerType.SFX);
            return;
        }
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    // OnEvents
    public virtual void OnPickUp()
    {
        Events?.onPickup.Invoke();
        PlaySound(SoundManager.SoundType.RunePickup, pickUpSound);
    }
    public virtual void OnGroundPickUp() => Events?.onGroundPickup.Invoke();
    public virtual void OnAlterPickUp() => Events?.onAlterPickup.Invoke();
    public virtual void OnDropped()
    {
        Events?.onDrop.Invoke();
        PlaySound(SoundManager.SoundType.RunePlacement, dropSound);
    }
    public virtual void OnKicked()
    {
        Events?.onAlterKicked.Invoke();
        PlaySound(SoundManager.SoundType.RunePlacement, kickSound);
    }
    public virtual void OnAlterPlace()
    {
        Events?.onAlterPlaced.Invoke();
        PlaySound(SoundManager.SoundType.RunePlacement, placeSound);
    }
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
        audioSource = GetComponent<AudioSource>();
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
