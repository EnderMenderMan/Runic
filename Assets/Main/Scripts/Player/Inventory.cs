using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory PlayerInventory { get; private set; }
    [field: SerializeField] public Rune heldRune { get; private set; }
    public void PickUpRune(Rune rune)
    {
        if (heldRune != null)
        {
            rune.gameObject.SetActive(false);
            if (DropRuneAtPosition(rune.transform.position) == false)
            { rune.gameObject.SetActive(true); return; }
            rune.gameObject.SetActive(true);
        }

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
        heldRune.gameObject.SetActive(false);
        if (WorldData.Instance != null && WorldData.Instance.IsGridSpaceFree(position) == false)
        { heldRune.gameObject.SetActive(true); return false; }
        heldRune.gameObject.SetActive(true);
        
        heldRune.IsInteractDisabled = false;
        heldRune.transform.position = WorldData.Instance != null ? WorldData.Instance.WorldGrid.WorldToCell(position) : position;
        heldRune = null;
        return true;
    }

    public void ForceDropRune()
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


