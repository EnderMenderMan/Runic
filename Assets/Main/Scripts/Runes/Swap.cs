using System;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
public class Swap : Rune
{
    [Tooltip("When player places the rune on alter")] public UnityEvent onAlterPlaced;
  //  public void SwapSelectedRune() => SwapSelectedRune((selectedRuneIndex + 1) % transformToRunes.Length);
}
