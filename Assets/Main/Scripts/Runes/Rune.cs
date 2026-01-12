using System;
using System.Collections;
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
        public AudioClip runeAmbienceSound;

    }

    [SerializeField] private ParticleSystem trailParticles;
    [SerializeField] RuneAudio audio;


    [CanBeNull] public RuneEvents Events { get; protected set; }
    [CanBeNull] public RuneAfterEvents AfterEvents { get; protected set; }

    [Tooltip("Is used to determine if the rune can be placed on a alter")][SerializeField] protected AlterFilter placeOnAlterFilter;
    [NonSerialized][CanBeNull] public Alter alter;
    public bool resetPositionWhenDropedOrKicked;
    private Vector3 originalPosition;
    private Collider2D[] colliders;
    [field: SerializeField] public Tags tags;
    public bool IsInteractDisabled { get; set; }
    public Animator animator { get; private set; }
    public bool countToAlterClusterComplete = true;
    AudioSource runeAmbiencePlayingSource;


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

    protected virtual AudioSource PlaySound(SoundManager.SoundType soundType, [CanBeNull] AudioClip audioClip)
    {
        if (audio.enablePlaySounds == false)
            return null;
        if (audioClip == null)
        {
            return SoundManager.instance.PlaySound(gameObject, soundType, SoundManager.MixerType.Runes);
        }
        audio.audioSource.Stop();
        audio.audioSource.clip = audioClip;
        audio.audioSource.Play();
        return null;
    }

    void CancleRuneAmbiance()
    {
        if (runeAmbiencePlayingSource == null)
            return;
        runeAmbiencePlayingSource.Stop();
        Destroy(runeAmbiencePlayingSource);
        runeAmbiencePlayingSource = null;
    }

    // OnEvents
    public virtual void OnPickUp()
    {
        Events?.onPickup.Invoke();
        PlaySound(SoundManager.SoundType.RunePickup, audio.pickUpSound);
        runeAmbiencePlayingSource = PlaySound(SoundManager.SoundType.RuneAmbience, audio.runeAmbienceSound);

        StopAllCoroutines();
        OnEndResetPosition();

    }
    public virtual void OnGroundPickUp() => Events?.onGroundPickup.Invoke();
    public virtual void OnAlterPickUp() => Events?.onAlterPickup.Invoke();
    public virtual void OnDropped()
    {
        Events?.onDrop.Invoke();
        CancleRuneAmbiance();
        PlaySound(SoundManager.SoundType.RuneDrop, audio.dropSound);
    }
    public virtual void OnKicked()
    {
        Events?.onAlterKicked.Invoke();
        PlaySound(SoundManager.SoundType.RuneDrop, audio.kickSound);
    }
    public virtual void OnAlterPlace()
    {
        Events?.onAlterPlaced.Invoke();
        CancleRuneAmbiance();
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
        if (resetPositionWhenDropedOrKicked && Inventory.PlayerInventory.heldRune != this)
            ResetPosition();
    }
    public virtual void AfterAlterPlace() => AfterEvents?.afterAlterPlaced.Invoke();


    public void ResetPosition()
    {
        StartCoroutine(RestPositionCorutine());
    }
    IEnumerator RestPositionCorutine()
    {
        IsInteractDisabled = true;
        if (trailParticles == null)
            trailParticles = GetComponentInChildren<ParticleSystem>();
        if (trailParticles != null)
            trailParticles.Play();
        yield return null;
        while (Vector2.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, 20f * Time.deltaTime);
            yield return null;
        }
        transform.position = originalPosition;
        OnEndResetPosition();

        IsInteractDisabled = false;

    }
    void OnEndResetPosition()
    {
        if (trailParticles != null)
            trailParticles.Stop();

    }

    public void SetCollidersActive(bool value)
    {
        colliders ??= GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders)
            col.enabled = value;
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

    public virtual bool OnInteract(InteractData data)
    {
        switch (data.type)
        {
            case InteractType.Player:

                if (Inventory.PlayerInventory.TryPickUpRune(this) == false)
                    return false;
                OnGroundPickUp();

                break;
            case InteractType.Bullet:
                BulletInteract(data);
                break;
        }
        return true;
    }

    public (int thisAlterIndex, Alter[] alters) GetAlters() => alter.GetAlters();


}
