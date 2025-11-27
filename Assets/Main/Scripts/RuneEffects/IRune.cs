public interface IRune
{
    int ValueID { get; }

    void Drop();
    void OnInteract();
    void PickUp();
    void TriggerPillarPlacement(int itemIndex, Alter[] pillars);
}
