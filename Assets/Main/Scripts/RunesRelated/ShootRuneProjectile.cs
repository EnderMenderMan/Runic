using UnityEngine;

public class ShootRuneProjectile : MonoBehaviour
{
    public ShootRune shootRune;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Interactable") == false)
            return;
        IInteract[] interacts = col.GetComponents<IInteract>();
        foreach (IInteract interact in interacts)
            interact.OnInteract(new InteractData { type = InteractType.Bullet, senderObject = gameObject });
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
