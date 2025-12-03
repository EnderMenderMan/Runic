using UnityEngine;

public class SpeedRune : Rune
{
    [SerializeField] float whenActiveSpeed;
    float origninalSpeedDif;

    public void Activate() => Activate(whenActiveSpeed);
    public void Activate(float speedValue)
    {
        origninalSpeedDif += speedValue - PlayerMovement.Instance.speed;
        PlayerMovement.Instance.speed = speedValue;
    }
    public void Deactivate()
    {
        PlayerMovement.Instance.speed -= origninalSpeedDif;
        origninalSpeedDif = 0;
    }
}
