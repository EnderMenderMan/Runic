using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SeeInvisibleRune : Rune
{
    [Header("SeeInvisibleSettings")]
    [SerializeField] private float stopDelay;
    [SerializeField] private bool enableDefaultBehaviour = true;
    [SerializeField] ParticleSystem radarParticles;
    [SerializeField] private ParticleSystem stencilParticles;

    private bool isSeeEnabled;
    private bool isReversed;
    private bool isParticleSystemsPaused;
    private Coroutine startRadarParticles;

    public void EnableSeeInvisible()
    {
        if (isSeeEnabled)
            return;
        isSeeEnabled = true;
        
        startRadarParticles = StartCoroutine(StartRadarParticles());
    }

    public void DisableSeeInvisible()
    {
        if (isSeeEnabled == false)
            return;
        isSeeEnabled = false;
        
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

    public override void OnInteract(InteractData data)
    {
        switch (data.type)
        {
            case InteractType.Player:
                base.OnInteract(data);
                break;
        }
    }

    public override void AfterDropped()
    {
        base.AfterDropped();
        if (enableDefaultBehaviour)
            DisableSeeInvisible();
    }

    public override void OnKicked()
    {
        base.OnKicked();
        if (enableDefaultBehaviour)
            DisableSeeInvisible();
    }

    public override void OnPickUp()
    {
        base.OnPickUp();
        if (enableDefaultBehaviour) 
            EnableSeeInvisible();
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