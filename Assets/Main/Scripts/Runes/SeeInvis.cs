using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SeeInvis : Rune
{
    [Header("SeeInvis")]
    [SerializeField] private float stopDelay;
    [SerializeField] ParticleSystem radarParticles;
    [SerializeField] private ParticleSystem stencilParticles;
    [SerializeField] UnityEvent onPickup;
    [SerializeField] UnityEvent onDrop;

    private bool isReversed;
    private bool isParticleSystemsPaused;
    private Coroutine startRadarParticles;

    public override void OnInteract(InteractData data)
    {
        switch (data.type)
        {
            case InteractType.Player:
                base.OnInteract(data);
                onPickup.Invoke();
                startRadarParticles = StartCoroutine(StartRadarParticles());
                break;
        }
    }

    public override void OnDropped()
    {
        base.OnDropped();
        onDrop.Invoke();
        
        StopCoroutine(startRadarParticles);
        if (isParticleSystemsPaused)
        {
            isParticleSystemsPaused = false;
            stencilParticles.Play();
            radarParticles.Play();
        }
        
        if (isReversed == false)
            ReverseParticleSystems();
    }

    IEnumerator StartRadarParticles()
    {
        yield return null;

        if (isReversed)
            ReverseParticleSystems();

        if (radarParticles.time == 0)
        {
            stencilParticles.Clear();
            radarParticles.Clear();
            stencilParticles.Stop();
            radarParticles.Stop();
            stencilParticles.Play();
            radarParticles.Play();
            yield return new WaitForSeconds(stopDelay);
        }
        else
            yield return new WaitForSeconds(stopDelay-radarParticles.time);
        
        radarParticles.Simulate(stopDelay);
        stencilParticles.Simulate(stopDelay);
        isParticleSystemsPaused = true;
    }

    void ReverseParticleSystems()
    {
        radarParticles.Simulate(radarParticles.main.duration-radarParticles.time);
        radarParticles.Play();
        stencilParticles.Simulate(stencilParticles.main.duration-stencilParticles.time);
        stencilParticles.Play();
        
        isReversed = !isReversed;
        
        var ps = radarParticles.main;
        ps.startSpeed = -1 * ps.startSpeed.constant;
        var ss = stencilParticles.main;
        ss.startSpeed = -1 * ps.startSpeed.constant;
        
        var psz = radarParticles.sizeOverLifetime;
        psz.size = new ParticleSystem.MinMaxCurve(psz.sizeMultiplier, Utility.GetFlipAnimationCurve(psz.size.curve));
        var ssz = stencilParticles.sizeOverLifetime;
        ssz.size = new ParticleSystem.MinMaxCurve(ssz.sizeMultiplier, Utility.GetFlipAnimationCurve(ssz.size.curve));
        
    }
}