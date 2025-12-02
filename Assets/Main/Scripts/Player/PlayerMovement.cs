using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 dir;

    private Rigidbody2D rb;
    Animator animator;

    [SerializeField] Collider2D interactCollider;
    Vector2 interactColliderOffset;

    void Awake()
    {
        interactColliderOffset = interactCollider.offset;
        interactCollider.offset = new Vector2(0, interactColliderOffset.y);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = dir * speed;
    }

    private void OnEnable()
    {
        PlayerInteract.InputActions.Player.Move.performed += context =>
        {
            animator.SetBool("IsMoving", true);
            dir = context.ReadValue<Vector2>();

            if (dir.x != 0)
            {
                animator.SetInteger("DirState", 0);
                interactCollider.offset = new Vector2(interactColliderOffset.x, 0);
            }
            else if (dir.y > 0)
            {
                animator.SetInteger("DirState", 1);
                interactCollider.offset = new Vector2(0, interactColliderOffset.y);
            }
            else
            {
                animator.SetInteger("DirState", -1);
                interactCollider.offset = new Vector2(0, -interactColliderOffset.y);
            }


            Vector3 newRotation = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            if (dir.x > 0)
                newRotation.y = 180;
            transform.eulerAngles = newRotation;
        };
        PlayerInteract.InputActions.Player.Move.canceled += context =>
        {
            animator.SetBool("IsMoving", false);
            dir = Vector2.zero;
        };
        PlayerInteract.InputActions.Player.Move.Enable();
    }

    private void OnDisable()
    {
        PlayerInteract.InputActions.Player.Move.Disable();
    }
}
