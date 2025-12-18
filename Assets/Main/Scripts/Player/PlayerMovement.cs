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

    [Header("Grid Based Movement")]
    [SerializeField] bool enableGridBasedMovement;
    [SerializeField] Vector2 gridOffset;
    [SerializeField] LayerMask gridMovementCollideWithLayers;
    Vector2 gridTargetPos;

    private Rigidbody2D rb;
    Animator animator;
    Collider2D interactColider;


    #region ChangeSize
    class ChangeStackValues { public float sizeToChange, changeRate; public (float sizeToChange, float changeRate) previus; }
    List<ChangeStackValues> changeSizeStack = new List<ChangeStackValues>();
    bool stopGrow, hasReverted;
    Vector3 previusScale;
    #endregion

    #region KeyPressOrder
    private Vector2 dirClamped;
    private Vector2[] dirPressedOrder = new Vector2[4];
    private int dirPressedOrderCountainLength;

    void AddDirPressedOrder(Vector2 dir)
    {
        for (int i = 0; i < dirPressedOrder.Length; i++)
        {
            if (dirPressedOrder[i] != Vector2.zero)
                continue;
            dirPressedOrder[i] = dir;
            dirPressedOrderCountainLength++;
            break;
        }
    }
    void RemovePressedOrder(Vector2 dir)
    {
        int foundDirIndex = -1;
        for (int i = 0; i < dirPressedOrder.Length; i++)
        {
            if (foundDirIndex == -1 && dirPressedOrder[i] != dir)
                continue;
            if (foundDirIndex == -1)
            {
                dirPressedOrderCountainLength--;
                foundDirIndex = i;
                if (i == dirPressedOrder.Length - 1)
                    dirPressedOrder[i] = Vector2.zero;
                continue;
            }
            dirPressedOrder[i - 1] = dirPressedOrder[i];
            if (i == dirPressedOrder.Length - 1)
                dirPressedOrder[i] = Vector2.zero;

        }
    }
    Vector2 GetLastDirPressed() => dirPressedOrder[dirPressedOrderCountainLength - 1];
    #endregion


    void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayerInteract.Instance.interactColliderDetection.offset = PlayerInteract.Instance.interactColiderDownOffest;

        if (enableGridBasedMovement && WorldData.Instance == null)
            enableGridBasedMovement = false;

        if (enableGridBasedMovement)
        {
            transform.position = WorldData.Instance.GetCorrenctionToCellCenter(transform.position) + (Vector3)gridOffset;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (enableGridBasedMovement)
            UpdateGridMovement();
        else
            UpdateNormalMovement();

        UpdateSize();
    }
    void UpdateGridMovement()
    {
        if (MovingSpeed == 0)
            return;
        Vector2 movePoint = Vector2.MoveTowards(transform.position, gridTargetPos, speed * Time.deltaTime);
        transform.position = movePoint;
        if (Vector2.Distance(transform.position, gridTargetPos) < 0.01f)
        {
            MovingSpeed = 0;
            transform.position = gridTargetPos;
            if (dirPressedOrderCountainLength == 0)
                return;

            SetGridMovementTargetPosition(transform.position + Utility.ScaleByWorlGridCellSize(GetLastDirPressed()));
        }

    }

    void SetGridMovementTargetPosition(Vector2 position)
    {
        if (MovingSpeed != 0)
            return;

        gridTargetPos = WorldData.Instance.GetCorrenctionToCellCenter(position - gridOffset);
        if (IsGridSpaceFree(gridTargetPos, gridMovementCollideWithLayers) == false)
            return;
        gridTargetPos += gridOffset;
        MovingSpeed = speed;
    }
    
    public bool IsGridSpaceFree(Vector2 position, LayerMask collideWithLayers)
    {
        if (Physics2D.OverlapArea(position, transform.position, collideWithLayers) != null)
            return false;
        return true;
    }
    void UpdateNormalMovement()
    {
        rb.linearVelocity = dir * speed;
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
            Vector2 contextValue = context.ReadValue<Vector2>();
            Vector2 contextValueClamped = Utility.GetVectorClampToOne(contextValue);
            if (Utility.CountVectorValues(contextValueClamped, 0) < Utility.CountVectorValues(dir, 0))
                AddDirPressedOrder(contextValueClamped - dirClamped);
            else
                RemovePressedOrder(dirClamped - contextValueClamped);
            dirClamped = contextValueClamped;
            dir = contextValue;

            FacingDirection = dir;

            if (enableGridBasedMovement)
                SetGridMovementTargetPosition(transform.position + Utility.ScaleByWorlGridCellSize(GetLastDirPressed()));
            else if (enableGridBasedMovement == false)
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
            if (enableGridBasedMovement == false)
                MovingSpeed = 0;
            animator.SetBool("IsMoving", false);

            dir = Vector2.zero;
            dirClamped = Vector2.zero;
            for (int i = 0; i < dirPressedOrderCountainLength; i++)
                dirPressedOrder[i] = Vector2.zero;
            dirPressedOrderCountainLength = 0;
        };
        PlayerInteract.Instance.InputActions.Player.Move.Enable();
    }

    private void OnDisable()
    {
        PlayerInteract.Instance.InputActions.Player.Move.Disable();
    }
}
