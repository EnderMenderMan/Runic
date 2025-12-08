using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class RunRune : Rune
{
    public enum State
    {
        Idle,
        Run,
        Dead,
    }

    [Tooltip("Can jump off alter and start running")][field: SerializeField] public bool CanRunFromAlter { get; private set; }
    [Tooltip("Can jump off when held by the player and start running")][field: SerializeField] public bool CanStartRunningWhenHold { get; private set; }
    [Tooltip("Can start running from the idle state (idle state mean that the rune is not moving)")][field: SerializeField] public bool CanStartRunningWhenIdle { get; private set; }
    [Tooltip("How much time to be idle for. 0 or less mean infinite time. (idle state mean that the rune is not moving)")][SerializeField] float idleForTime;
    float idleTimer;
    [Tooltip("How fast the runes movement accelerate")][SerializeField] float accelerationForce;
    [Tooltip("The maximum speed the rune can travel at")][SerializeField] float moveMaxForce;
    [Tooltip("How close to its targeted move point is has to go before it switches to a new point (too small values can cause problems)")][SerializeField] float distanceWhenStartStop;
    [Tooltip("Points that the runes moves towards. Loop around when it reaches the final point")][SerializeField] Transform[] moveToPoints;
    State state;
    int moveToPointsIndex;
    Rigidbody2D rb;

    [Header("Events")]
    public UnityEvent whenChangedStateToIdle;
    public UnityEvent whenChangedStateToRun;


    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        state = State.Run;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    public override void AfterDropped()
    {
        if (state == State.Run)
            transform.position = Inventory.PlayerInventory.transform.position;
        base.AfterDropped();
    }

    public override void OnPickUp()
    {
        base.OnPickUp();
        if (CanStartRunningWhenHold && state == State.Run)
            Inventory.PlayerInventory.ShadowForceDropRune();
    }

    public override bool TryBePlaced(int alterIndex, Alter[] alters, AlterCluster cluster)
    {
        if (state == State.Run && CanRunFromAlter)
        {
            Inventory.PlayerInventory.ShadowForceDropRune();
            return false;
        }
        else if (state == State.Run)
        {
            rb.linearVelocity = Vector2.zero;
        }
        return base.TryBePlaced(alterIndex, alters, cluster);
    }

    void UpdateTimer()
    {
        if (CanStartRunningWhenIdle == false || (CanRunFromAlter == false && alter != null))
            return;
        if (CanStartRunningWhenHold == false && Inventory.PlayerInventory.heldRune != null && Inventory.PlayerInventory.heldRune.gameObject == gameObject)
            return;
        if (state != State.Idle || idleTimer <= 0)
            return;
        idleTimer -= Time.deltaTime;

        if (idleTimer > 0)
            return;

        SwitchStates(State.Run);
        if (alter != null)
        {
            Vector3 alterPos = alter.transform.position;
            alter?.TryKickItem(false);
            transform.position = alterPos;
        }
        else if (Inventory.PlayerInventory.heldRune != null && Inventory.PlayerInventory.heldRune.gameObject == gameObject)
        {
            Inventory.PlayerInventory.ShadowForceDropRune();
        }
    }

    void FixedUpdateMove()
    {
        if (state != State.Run || (CanRunFromAlter == false && alter != null))
            return;

        if (Vector2.Distance(transform.position, moveToPoints[moveToPointsIndex].position) <= distanceWhenStartStop)
        {
            moveToPointsIndex++;
            moveToPointsIndex %= moveToPoints.Length;
        }

        Vector2 distanceVecToPoint = moveToPoints[moveToPointsIndex].position - transform.position;
        // float sameDir = Vector2.Dot(rb.linearVelocity, distanceVecToPoint) - (distanceVecToPoint.magnitude * rb.linearVelocity.magnitude);
        // if (Mathf.Abs(sameDir) < 0.2 && rb.linearVelocity.magnitude >= moveMaxForce)
        // 	return;
        distanceVecToPoint.Normalize();
        distanceVecToPoint *= moveMaxForce;
        Vector2 vectorDif = distanceVecToPoint - rb.linearVelocity;
        if (vectorDif.magnitude < accelerationForce * Time.deltaTime)
            rb.linearVelocity = distanceVecToPoint;
        else
            rb.linearVelocity += vectorDif.normalized * (accelerationForce * Time.deltaTime);

    }

    void FixedUpdate()
    {
        FixedUpdateMove();
    }

    public void SwitchToIdleState() => SwitchStates(State.Idle);
    public void SwitchToRunState() => SwitchStates(State.Run);
    public void SwitchStates(State newState)
    {
        switch (newState)
        {
            case State.Idle:
                if (state == State.Idle)
                    return;
                idleTimer = idleForTime;
                state = State.Idle;
                whenChangedStateToIdle.Invoke();
                break;
            case State.Run:
                if (state == State.Run)
                    return;
                state = State.Run;
                idleTimer = idleForTime;
                whenChangedStateToRun.Invoke();
                break;
        }
    }
    protected override void BulletInteract(InteractData data)
    {
        if (state != State.Run)
            return;

        SwitchStates(State.Idle);
        rb.linearVelocity = Vector2.zero;
        if (WorldData.Instance != null)
            transform.position = WorldData.Instance.GetCorrenctionToCellCenter(transform.position);
    }
}
