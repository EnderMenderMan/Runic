using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector2 dir;
    public static Vector2 FacingDirection { get; private set; }
    public static float MovingSpeed { get; private set; }
    public static PlayerMovement Instance { get; private set; }

    private Rigidbody2D rb;
    Animator animator;
    Collider2D interactColider;


    void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayerInteract.Instance.interactColliderDetection.offset = PlayerInteract.Instance.interactColiderDownOffest;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = dir * speed;
    }

    private void OnEnable()
    {
        Instance = this;
        PlayerInteract.Instance.InputActions.Player.Move.performed += context =>
        {
            animator.SetBool("IsMoving", true);
            dir = context.ReadValue<Vector2>();
            FacingDirection = dir;
            MovingSpeed = speed;

            if (dir.x != 0)
            {
                animator.SetInteger("DirState", 0);
                PlayerInteract.Instance.interactColliderDetection.offset = PlayerInteract.Instance.interactColiderSideOffest;
            }
            else if (dir.y > 0)
            {
                animator.SetInteger("DirState", 1);
                PlayerInteract.Instance.interactColliderDetection.offset = PlayerInteract.Instance.interactColiderTopOffest;
            }
            else
            {
                animator.SetInteger("DirState", -1);
                PlayerInteract.Instance.interactColliderDetection.offset = PlayerInteract.Instance.interactColiderDownOffest;
            }


            Vector3 newRotation = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            if (dir.x > 0)
                newRotation.y = 180;
            transform.eulerAngles = newRotation;
        };
        PlayerInteract.Instance.InputActions.Player.Move.canceled += context =>
        {
            MovingSpeed = 0;
            animator.SetBool("IsMoving", false);
            dir = Vector2.zero;
        };
        PlayerInteract.Instance.InputActions.Player.Move.Enable();
    }

    private void OnDisable()
    {
        PlayerInteract.Instance.InputActions.Player.Move.Disable();
    }
}
