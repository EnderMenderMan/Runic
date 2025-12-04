using UnityEngine;

[RequireComponent(typeof(Rune))]
public class OnRuneShootExtra : MonoBehaviour, IInteract
{
    public bool equpRuneWhenShoot;
    public bool IsInteractDisabled { get; set; }

    public void OnInteract(InteractData data)
    {
        Debug.Log("Aw");
        switch (data.type)
        {
            case InteractType.Bullet:
                if (equpRuneWhenShoot)
                {
                    Inventory.PlayerInventory.DropRune();
                    Inventory.PlayerInventory.TryPickUpRune(GetComponent<Rune>());
                }

                break;
        }
    }
}
