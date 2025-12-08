using System;
using System.Collections.Generic;
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


    class ChangeStackValues { public float sizeToChange, changeRate; public (float sizeToChange, float changeRate) previus; }
    List<ChangeStackValues> changeSizeStack = new List<ChangeStackValues>();
    bool stopGrow, hasReverted;
    Vector3 previusScale;

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
        UpdateSize();
    }

    void UpdateSize()
    {
        if (stopGrow && hasReverted)
            return;

        float totalChangeThisFrame = 0;

        if (stopGrow)
        {
            // check if it shrinks and if so start to grow again (or more accuracte srink)
            foreach (ChangeStackValues value in changeSizeStack)
            {
                float change = value.changeRate * Time.deltaTime;
                if (Math.Abs(value.sizeToChange) < Math.Abs(change))
                {
                    change = value.sizeToChange;
                }
                totalChangeThisFrame += change;
            }

            // if collider collided than revert size back to previus size
            if (totalChangeThisFrame > 0 && transform.localScale.x > previusScale.x)
            {
                transform.localScale = previusScale;
                foreach (ChangeStackValues value in changeSizeStack)
                {
                    value.changeRate = value.previus.changeRate;
                    value.sizeToChange = value.previus.sizeToChange;
                }
                hasReverted = true;
                return;
            }
            totalChangeThisFrame = 0;
        }

        // get size changes
        for (int i = changeSizeStack.Count - 1; i >= 0; i--)
        {
            if (changeSizeStack[i].sizeToChange == 0)
            {
                changeSizeStack.RemoveAt(i);
                continue;
            }

            changeSizeStack[i].previus = new(changeSizeStack[i].sizeToChange, changeSizeStack[i].changeRate);
            float change = changeSizeStack[i].changeRate * Time.deltaTime;
            if (Math.Abs(changeSizeStack[i].sizeToChange) < Math.Abs(change))
            {
                change = changeSizeStack[i].sizeToChange;
                changeSizeStack[i].sizeToChange = 0;
            }
            else
            {
                changeSizeStack[i].sizeToChange -= change;
            }
            totalChangeThisFrame += change;
        }


        hasReverted = false;
        Vector3 scale = transform.localScale;
        previusScale = scale;
        transform.localScale = new Vector3(scale.x + totalChangeThisFrame, scale.y + totalChangeThisFrame, scale.z + totalChangeThisFrame);


    }
    public void Resize(float size, float changeRate)
    {
        ChangeStackValues newValue = new ChangeStackValues { sizeToChange = size, changeRate = Mathf.Abs(changeRate) * Mathf.Sign(size) };
        newValue.previus = new(newValue.sizeToChange, newValue.changeRate);
        changeSizeStack.Add(newValue);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        stopGrow = true;
    }
    void OnCollisionExit2D(Collision2D col)
    {
        stopGrow = false;
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
