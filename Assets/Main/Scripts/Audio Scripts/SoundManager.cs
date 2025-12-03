
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    private AudioMixerGroup[] mixerGroups;
    private readonly List<AudioLifeTime> audioLifeTimes = new List<AudioLifeTime>();
    private readonly List<AudioData> delayedAudio = new List<AudioData>();
    public static SoundManager instance;

    private class AudioLifeTime
    {
        public AudioSource source;
        public float timer;
    }
    private class AudioData
    {
        public GameObject playAt;
        public float timer;
        public SoundType soundType;
        public MixerType mixerType;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        mixerGroups = mixer.FindMatchingGroups("");
    }

    private void Update()
    {
        for (int i = 0; i < audioLifeTimes.Count; i++)
        {
            if (audioLifeTimes[i].timer <= 0)
            {
                Destroy(audioLifeTimes[i].source);
                audioLifeTimes.RemoveAt(i);
                i--;
                continue;
            }
            
            audioLifeTimes[i].timer -= Time.deltaTime;
        }
        
        for (int i = 0; i < delayedAudio.Count; i++)
        {
            if (delayedAudio[i].timer <= 0)
            {
                if (delayedAudio[i].playAt != null)
                    PlaySound(delayedAudio[i].playAt, delayedAudio[i].soundType, delayedAudio[i].mixerType);
                delayedAudio.RemoveAt(i);
                i--;
                continue;
            }
            
            delayedAudio[i].timer -= Time.deltaTime;
        }
    }

    public AudioMixerGroup GetMixerGroup(MixerType type)
    {
        return mixerGroups[(int)type];
    }

    public enum SoundType
    {
        PlayerMove, RunePlacement, RunePickup, RuneAmbience, LevelAmbience, 
        
    }
    public enum MixerType
    {
        Master,
        Music,
        Environment,
        Player,
        SFX,
    }
    
    [SerializeField] private Sounds[] soundsArray;

    [System.Serializable]
    public class Sounds
    {
        public string name;
        public SoundType soundType;
        public AudioResource[] audioResource;
    }

    public void PlaySound(SoundType soundType, MixerType mixerType, float startDelay)
    {
        delayedAudio.Add(new AudioData{playAt = gameObject, soundType = soundType, mixerType = mixerType, timer = startDelay });
    }
    public void PlaySound(GameObject audioOrigin, SoundType soundType, MixerType mixerType, float startDelay)
    {
        delayedAudio.Add(new AudioData{playAt = audioOrigin, soundType = soundType, mixerType = mixerType, timer = startDelay });
    }
    public AudioSource PlaySound(GameObject audioOrigin, SoundType soundType, MixerType mixerType)
    {
        Sounds soundsClass = Array.Find(soundsArray, soundsClass => soundsClass.soundType == soundType);

        if (soundsClass != null && soundsClass.audioResource.Length > 0)
        {
            AudioResource randomAudioClip = soundsClass.audioResource[UnityEngine.Random.Range(0, soundsClass.audioResource.Length)];
            AudioSource audioSource = audioOrigin.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = GetMixerGroup(mixerType);
            audioSource.resource = randomAudioClip;
            audioSource.Play();
            
            if (audioSource.clip == null)
                audioLifeTimes.Add(new AudioLifeTime{source = audioSource, timer = 5f });
            else
                audioLifeTimes.Add(new AudioLifeTime{source = audioSource, timer = audioSource.clip.length });
            
            return audioSource;
        }

        return null;
    }
    public AudioSource PlaySound(SoundType soundType, MixerType mixerType)
    {
        return PlaySound(gameObject, soundType, mixerType);
    }

    private System.Collections.IEnumerator RemoveAudioSourceAfterPlayback(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        Destroy(audioSource);
    }
}