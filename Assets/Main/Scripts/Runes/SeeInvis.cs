using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SeeInvis : Rune
{
    [SerializeField] UnityEvent onPickup;
    [SerializeField] UnityEvent onDrop;
    public override void OnInteract()
    {
        base.OnInteract();
        onPickup.Invoke();
    }

    public override void OnDropped()
    {
        base.OnDropped();
        Debug.Log("SEEthis");
        onDrop.Invoke();
    }
}
