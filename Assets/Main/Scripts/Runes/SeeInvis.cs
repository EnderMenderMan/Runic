using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SeeInvis : Rune
{
    [SerializeField] ParticleSystem radarParticles;
    [SerializeField] UnityEvent onPickup;
    [SerializeField] UnityEvent onDrop;

    public override void OnInteract(InteractData data)
    {
        switch (data.type)
        {
            case InteractType.Player:
                base.OnInteract(data);
                onPickup.Invoke();
                StartCoroutine(StartRadarParticles());
                break;
        }
    }

    public override void OnDropped()
    {
        base.OnDropped();
        onDrop.Invoke();
        radarParticles.Stop();
    }

    IEnumerator StartRadarParticles()
    {
        yield return null;
        radarParticles.Play();
    }
}