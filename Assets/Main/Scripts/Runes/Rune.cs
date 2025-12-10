using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class Rune : MonoBehaviour, IInteract
{
    [System.Serializable]
    class RuneAudio
    {

        [NonSerialized] public AudioSource audioSource;
        public bool enablePlaySounds = true;
        public AudioClip pickUpSound;
        public AudioClip dropSound;
        public AudioClip placeSound;
        public AudioClip kickSound;

    }

    [SerializeField] RuneAudio audio;


    [CanBeNull] public RuneEvents Events { get; protected set; }
    [CanBeNull] public RuneAfterEvents AfterEvents { get; protected set; }

    [Tooltip("Is used to determine if the rune can be placed on a alter")][SerializeField] protected AlterFilter placeOnAlterFilter;
    [NonSerialized][CanBeNull] public Alter alter;
    public bool resetPositionWhenDropedOrKicked;
    protected Vector3 originalPosition;
    [field: SerializeField] public Tags tags;
    public bool IsInteractDisabled { get; set; }
    public Animator animator { get; private set; }


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

    protected virtual void PlaySound(SoundManager.SoundType soundType, [CanBeNull] AudioClip audioClip)
    {
        if (audio.enablePlaySounds == false)
            return;
        if (audioClip == null)
        {

            SoundManager.instance.PlaySound(gameObject, soundType, SoundManager.MixerType.SFX);
            return;
        }
        audio.audioSource.Stop();
        audio.audioSource.clip = audioClip;
        audio.audioSource.Play();
    }

    // OnEvents
    public virtual void OnPickUp()
    {
        Events?.onPickup.Invoke();
        PlaySound(SoundManager.SoundType.RunePickup, audio.pickUpSound);
    }
    public virtual void OnGroundPickUp() => Events?.onGroundPickup.Invoke();
    public virtual void OnAlterPickUp() => Events?.onAlterPickup.Invoke();
    public virtual void OnDropped()
    {
        Events?.onDrop.Invoke();
        PlaySound(SoundManager.SoundType.RunePlacement, audio.dropSound);
    }
    public virtual void OnKicked()
    {
        Events?.onAlterKicked.Invoke();
        PlaySound(SoundManager.SoundType.RunePlacement, audio.kickSound);
    }
    public virtual void OnAlterPlace()
    {
        Events?.onAlterPlaced.Invoke();
        PlaySound(SoundManager.SoundType.RunePlacement, audio.placeSound);
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
        animator = GetComponent<Animator>();
        Events = GetComponent<RuneEvents>();
        AfterEvents = GetComponent<RuneAfterEvents>();
        originalPosition = transform.position;
        audio.audioSource = GetComponent<AudioSource>();
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
