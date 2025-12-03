using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory PlayerInventory { get; private set; }
    [field: SerializeField] public Rune heldRune { get; private set; }
    public bool TryPickUpRune(Rune rune)
    {
        if (heldRune != null)
        {
            rune.gameObject.SetActive(false);
            if (DropRuneAtPosition(rune.transform.position) == false)
            { rune.gameObject.SetActive(true); return false; }
            rune.gameObject.SetActive(true);
        }

        ShadowForcePickUp(rune);
        heldRune.OnPickUp();
        return true;
    }
    public void ShadowForcePickUp(Rune rune)
    {
        if (rune == null)
            return;
        heldRune = rune;
        heldRune.IsInteractDisabled = true;

    }

    public bool DropRune()
    {
        if (heldRune == null)
            return false;
        return DropRuneAtPosition(heldRune.transform.position);

    }
    public bool DropRuneAtPosition(Vector2 position)
    {
        if (heldRune == null)
            return false;

        heldRune.OnDropped();
        heldRune.gameObject.SetActive(false);
        if (WorldData.Instance != null && WorldData.Instance.IsGridSpaceFree(position) == false)
        { heldRune.gameObject.SetActive(true); return false; }
        heldRune.gameObject.SetActive(true);

        heldRune.IsInteractDisabled = false;
        heldRune.transform.position = WorldData.Instance != null ? WorldData.Instance.WorldGrid.WorldToCell(position) + WorldData.Instance.WorldGrid.cellSize / 2 : position;
        heldRune.AfterDropped();
        heldRune = null;
        return true;
    }

    public void ShadowForceDropRune()
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


