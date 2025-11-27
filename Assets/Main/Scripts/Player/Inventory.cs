using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory PlayerInventory { get; private set; }
    [field: SerializeField] public Rune heldRune { get; private set; }
    public void PickUpRune(Rune rune)
    {
        if (heldRune != null)
        {
            heldRune.transform.position = rune.transform.position;
            heldRune = null;
        }

        heldRune = rune;
        heldRune.IsInteractDisabled = true;
    }

    public void DropRune()
    {
        if (heldRune == null)
            return;
        heldRune.IsInteractDisabled = false;
        heldRune = null;
    }

    void Awake()
    {
        if (GetComponent<PlayerInteract>() != null)
            PlayerInventory = this;
    }
    void Start()
    {
        // PlayerInteract.Instance.OnNoInteract.AddListener(DropRune);
    }


    void Update()
    {
        if (heldRune)
            heldRune.transform.position = transform.position;

    }
}


