using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance;
    public InputSystem_Actions InputActions;
    [field: SerializeField] public Collider2D interactColliderDetectionClose { get; private set; }
    [field: SerializeField] public Collider2D interactColliderDetectionFar { get; private set; }
    [field: SerializeField] public Vector2 interactColiderTopOffest { get; private set; }
    [field: SerializeField] public Vector2 interactColiderDownOffest { get; private set; }
    [field: SerializeField] public Vector2 interactColiderSideOffest { get; private set; }

    public UnityEvent OnNoInteract;
    public UnityEvent OnActivateRuneAbility;

    private void Awake()
    {
        // if (OnNoInteract == null)
        //     OnNoInteract = new UnityEvent();
        InputActions = new InputSystem_Actions();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // OnNoInteract.AddListener(Inventory.PlayerInventory.DropRune);
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool TriggerInteractWithCollider(Collider2D collider)
    {
        List<Collider2D> collisions = new List<Collider2D>();
        collider.Overlap(collisions);
        foreach (var collision in collisions)
        {
            if (!collision.CompareTag("Interactable"))
                continue;

            IInteract interact = collision.GetComponent<IInteract>();
            if (interact == null || interact.IsInteractDisabled)
                continue;

            bool couldInteract = interact.OnInteract(new InteractData { type = InteractType.Player, senderObject = gameObject });
            if (couldInteract)
                return true;
        }

        return false;
    }

    private void OnEnable()
    {
        PlayerInteract.Instance = this;

        InputActions.Player.Interact.performed += context =>
        {
            bool interacted = TriggerInteractWithCollider(interactColliderDetectionClose);
            if (interacted == false)
                interacted = TriggerInteractWithCollider(interactColliderDetectionFar);

            if (interacted == false)
                OnNoInteract?.Invoke();
        };
        InputActions.Player.Interact.Enable();

        InputActions.Player.RuneAbility.performed += context =>
        {
            OnActivateRuneAbility.Invoke();
        };
        InputActions.Player.RuneAbility.Enable();
    }

    private void OnDisable()
    {
        InputActions.Player.RuneAbility.Disable();
        InputActions.Player.Interact.Disable();
    }
}
