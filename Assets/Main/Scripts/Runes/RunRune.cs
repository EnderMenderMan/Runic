using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RunRune : Rune
{
    public enum State
    {
        Idle,
        Run,
        Dead,
    }
    [SerializeField] bool canRunFromAlter;
    [SerializeField] bool canStartRunnigWhenIdle;
    [SerializeField] float idleForTime;
    float idleTimer;
    [SerializeField] float moveForce;
    [SerializeField] float moveMaxForce;
    [SerializeField] float distanceWhenStartStop;
    [SerializeField] Transform[] moveToPoints;
    State state;
    int moveToPointsIndex;
    Rigidbody2D rb;

    public void SetCanRunFromAlter(bool value)
    {
        canRunFromAlter = value;
    }
    public void SetCanStartRunningWhenIdle(bool value)
    {
        canStartRunnigWhenIdle = value;
        if (value = true)
            StartRunningCheck();
    }

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        state = State.Run;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        if (state != State.Idle || idleTimer <= 0 || Inventory.PlayerInventory.heldRune.gameObject == gameObject)
            return;
        idleTimer -= Time.deltaTime;
        StartRunningCheck();
    }

    void StartRunningCheck()
    {
        if (idleTimer > 0)
            return;

        state = State.Run;
        alter?.KickItem();
    }

    void FixedUpdateMove()
    {
        if (state != State.Run)
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
        if (vectorDif.magnitude < moveForce * Time.deltaTime)
            rb.linearVelocity = distanceVecToPoint;
        else
            rb.linearVelocity += vectorDif.normalized * moveForce * Time.deltaTime;

    }

    void FixedUpdate()
    {
        FixedUpdateMove();
    }
    protected override void BulletInteract(InteractData data)
    {
        if (state != State.Run)
            return;

        idleTimer = idleForTime;
        state = State.Idle;
        rb.linearVelocity = Vector2.zero;
        if (WorldData.Instance != null)
            transform.position = WorldData.Instance.GetCorrenctionToCellCenter(transform.position);
    }
}
