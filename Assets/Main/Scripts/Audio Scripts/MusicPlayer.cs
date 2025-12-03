using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicPlayer : MonoBehaviour
{
    public MusicPlayer Instance { private set; get; }
    [SerializeField] private AudioClip[] audioClips;
    public float transitionDelay;
    float transitionCurrentDelay = -100;
    float transitionTimer;
    private float shortenTransitionTimer;
    private bool halfPoint;
    private AudioSource audioSource;
    private AudioClip changeToClip;
    private int audioClipsIndex;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeMusic(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransitionTimer();
    }

    void UpdateTransitionTimer()
    {
        if (transitionTimer == -100)
            return;
        if (halfPoint)
        {
            if (transitionTimer < 0)
                transitionTimer = 0;
            
            audioSource.volume = Mathf.Lerp(1, 0, transitionTimer/transitionCurrentDelay);
        }
        else
        {
            float currentTime = transitionTimer - transitionCurrentDelay-shortenTransitionTimer;
            if (currentTime <= 0)
            {
                currentTime = 0;
                halfPoint = true;
                audioSource.clip = changeToClip;
                audioSource.Play();
            }
            audioSource.volume = Mathf.Lerp(0, 1, currentTime/transitionCurrentDelay);
        }
        
        if (transitionTimer > 0)
        { transitionTimer -= Time.deltaTime; return; }

        ResetTransition();
    }

    void ResetTransition()
    {
        shortenTransitionTimer = 0;
        transitionTimer = -100;
        transitionCurrentDelay = -100;
        halfPoint = false;
    }

    public void ChangeToRandomSong()
    {
        ChangeWithTransitionMusic(Random.Range(0,audioClips.Length));
    }
    public void NextSong()
    {
        ChangeWithTransitionMusic(audioClipsIndex+1);
    }
    
    public void ChangeWithTransitionMusic(int index)
    {
        audioClipsIndex = index;
        index %= audioClips.Length;
        changeToClip = audioClips[index];
        
        if (transitionCurrentDelay == -100)
        {
            shortenTransitionTimer = 0;
        }
        else
        {
            if (halfPoint)
                shortenTransitionTimer =  transitionCurrentDelay-transitionTimer;
            else
                shortenTransitionTimer =  transitionTimer-transitionCurrentDelay;
        }
        
        transitionTimer = transitionDelay;
        transitionCurrentDelay = transitionDelay/2;
    }

    public void ChangeMusic(int index)
    {
        audioClipsIndex = index;
        ResetTransition();
        index %= audioClips.Length;
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
