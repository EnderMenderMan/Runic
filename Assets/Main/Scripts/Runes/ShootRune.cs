using UnityEngine;

public class ShootRune : Rune
{
    [SerializeField] float proectileSpeed;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] GameObject projectilePrefab;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Shoot()
    {
        if (Inventory.PlayerInventory.heldRune.gameObject != this.gameObject)
            return;
        GameObject projectileObject = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectileObject.GetComponent<Rigidbody2D>();
        projectileRb.linearVelocity = PlayerMovement.FacingDirection * proectileSpeed;
    }

    public override void OnPickUp()
    {
        PlayerInteract.Instance.OnActivateRuneAbility.AddListener(Shoot);
        base.OnPickUp();
    }
    public override void OnDropped()
    {
        PlayerInteract.Instance.OnActivateRuneAbility.RemoveListener(Shoot);
        base.OnDropped();
    }
    public override void OnAlterPlace()
    {
        PlayerInteract.Instance.OnActivateRuneAbility.RemoveListener(Shoot);
        base.OnDropped();
    }

}
