using UnityEngine;

[RequireComponent(typeof(Rune))]
public class OnRuneShootExtra : MonoBehaviour, IInteract
{
    public bool equpRuneWhenShoot;
    public bool IsInteractDisabled { get; set; }

    public bool OnInteract(InteractData data)
    {
        switch (data.type)
        {
            case InteractType.Bullet:
                if (equpRuneWhenShoot)
                {
                    Rune rune = Inventory.PlayerInventory.heldRune;
                    rune.OnDropped();
                    Inventory.PlayerInventory.ShadowForceDropRune();
                    rune.AfterDropped();
                    Inventory.PlayerInventory.TryPickUpRune(GetComponent<Rune>());
                }

                break;
        }
        return true;
    }
}
