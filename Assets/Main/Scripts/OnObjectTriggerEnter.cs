using UnityEngine;
using UnityEngine.Events;

public class OnObjectTriggerEnter : MonoBehaviour
{
    public UnityEvent onTriggerEvent;
    void OnTriggerEnter2D(Collider2D col)
    {
        onTriggerEvent.Invoke();
    }
}
