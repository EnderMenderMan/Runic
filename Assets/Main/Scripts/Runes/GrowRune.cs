using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRune : Rune
{
    [SerializeField] Collider2D playerGrowCheckCollider;
    [SerializeField] float resizeSpeed;
    [SerializeField] float whenActiveSize;
    float origninalSizeDif;
    (float size, float changedSoFar) forceResizeIfCanceledValues;

    public void Activate() => Activate(whenActiveSize);
    public void Activate(float sizeValue)
    {
        origninalSizeDif += sizeValue;
        PlayerMovement.Instance.Resize(sizeValue, resizeSpeed);
    }
    public void Deactivate()
    {
        PlayerMovement.Instance.Resize(-origninalSizeDif, resizeSpeed);
        origninalSizeDif = 0;
    }
}
