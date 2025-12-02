using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance;
    public static InputSystem_Actions InputActions;
    [field: SerializeField] public Collider2D interactColliderDetection { get; private set; }
    [field: SerializeField] public Vector2 interactColiderTopOffest { get; private set; }
    [field: SerializeField] public Vector2 interactColiderDownOffest { get; private set; }
    [field: SerializeField] public Vector2 interactColiderSideOffest { get; private set; }

    public UnityEvent OnNoInteract;

    private void Awake()
    {
        // if (OnNoInteract == null)
        //     OnNoInteract = new UnityEvent();
        Instance = this;
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

    private void OnEnable()
    {
        PlayerInteract.InputActions.Player.Attack.performed += context =>
        {
            bool interacted = false;
            List<Collider2D> collisions = new List<Collider2D>();
            interactColliderDetection.Overlap(collisions);
            foreach (var collision in collisions)
            {
                if (!collision.CompareTag("Interactable"))
                    continue;

                IInteract interact = collision.GetComponent<IInteract>();
                if (interact == null || interact.IsInteractDisabled)
                    continue;

                interact.OnInteract();
                interacted = true;
                break;
            }

            if (interacted == false)
                Inventory.PlayerInventory.DropRune();//OnNoInteract?.Invoke();
        };
        PlayerInteract.InputActions.Player.Attack.Enable();
    }

    private void OnDisable()
    {
        PlayerInteract.InputActions.Player.Attack.Disable();
    }
}
