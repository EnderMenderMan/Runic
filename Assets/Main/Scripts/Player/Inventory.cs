using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform runePickupTransformUp;
    [SerializeField] Transform runePickupTransformDown;
    [SerializeField] Transform runePickupTransformLeft;
    Transform currenctPickupTransform;

    [SerializeField] Transform ogRuneTransform;
    [SerializeField] int[] ogLayerOrder;
    [SerializeField] SpriteRenderer[] heldRuneRenderers;

    public static Inventory PlayerInventory { get; private set; }
    [field: SerializeField] public Rune heldRune { get; private set; }


    public void PickupUp() => ChangeHelgRunePickDir(runePickupTransformUp, 4);
    public void PickupDown() => ChangeHelgRunePickDir(runePickupTransformDown, 6);
    public void PickupSide() => ChangeHelgRunePickDir(currenctPickupTransform = runePickupTransformLeft, 4);
    void ChangeHelgRunePickDir(Transform pickupTransform, int order)
    {
        currenctPickupTransform = pickupTransform;

        if (ogLayerOrder == null || heldRuneRenderers == null)
            return;

        for (int i = 0; i < heldRuneRenderers.Length; i++)
            heldRuneRenderers[i].sortingOrder = order;


    }


    public bool TryPickUpRune(Rune rune)
    {
        if (heldRune != null)
        {
            rune.SetCollidersActive(false);
            if (DropRuneAtPosition(rune.transform.position) == false)
            { rune.SetCollidersActive(true); return false; }
            rune.SetCollidersActive(true);
        }

        ShadowForcePickUp(rune);
        heldRune.OnPickUp();
        return true;
    }

    public bool CanPickupRune(Rune rune)
    {
        if (heldRune != null && CanDropRune() == false)
            return false;

        return true;
    }
    public bool CanDropRune()
    {
        if (heldRune == null)
            return false;
        return CanDropRune(heldRune.transform.position);
    }
    public bool CanDropRune(Vector3 position)
    {
        if (heldRune == null)
            return false;
        if (WorldData.Instance != null && WorldData.Instance.IsGridSpaceFree(position) == false)
            return false;

        return true;
    }

    public void ShadowForcePickUp(Rune rune)
    {
        if (rune == null)
            return;
        heldRune = rune;
        heldRune.IsInteractDisabled = true;
        Utility.CopyTransform(ogRuneTransform, rune.transform);
        heldRuneRenderers = rune.GetComponentsInChildren<SpriteRenderer>();
        ogLayerOrder = new int[heldRuneRenderers.Length];
        for (int i = 0; i < heldRuneRenderers.Length; i++)
            ogLayerOrder[i] = heldRuneRenderers[i].sortingOrder;
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
        heldRune.SetCollidersActive(false);
        if (WorldData.Instance != null && WorldData.Instance.IsGridSpaceFree(position) == false)
        { heldRune.SetCollidersActive(true); return false; }
        heldRune.SetCollidersActive(true);

        heldRune.transform.position = WorldData.Instance != null ? WorldData.Instance.WorldGrid.WorldToCell(position) + WorldData.Instance.WorldGrid.cellSize / 2 : position;
        heldRune.AfterDropped();
        ShadowForceDropRune(); // TODO: test change order
        return true;
    }

    public void ShadowForceDropRune()
    {
        if (heldRune == null)
            return;

        Utility.CopyTransform(heldRune.transform, ogRuneTransform);
        if (ogLayerOrder != null)
            for (int i = 0; i < ogLayerOrder.Length; i++)
                heldRuneRenderers[i].sortingOrder = ogLayerOrder[i];
        ogLayerOrder = null;
        heldRuneRenderers = null;


        heldRune.transform.position = transform.position;
        heldRune.IsInteractDisabled = false;
        heldRune = null;
    }

    void Awake()
    {
        currenctPickupTransform = runePickupTransformUp;

        if (GetComponent<PlayerInteract>() != null)
            PlayerInventory = this;
    }
    void Start()
    {
        PlayerInteract.Instance.OnNoInteract.AddListener(DropRuneSub);
    }


    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Debuging
        {
            ShadowForceDropRune();
        }
        if (heldRune)
        {

            Utility.CopyTransform(heldRune.transform, currenctPickupTransform);
            // heldRune.transform.position = runePickupTransformUp.position;
        }

    }

    void DropRuneSub() => DropRune();
}


