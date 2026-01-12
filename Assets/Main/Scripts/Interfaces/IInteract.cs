using UnityEngine;

public interface IInteract
{
    public bool OnInteract(InteractData data);
    public bool IsInteractDisabled { get; set; }
}
public class InteractData
{
    public InteractType type;
    public GameObject senderObject;
}
public enum InteractType
{
    Player,
    Bullet,
}
