using UnityEngine;
using UnityEngine.Events;

public class RuneEvents : MonoBehaviour
{
    [Tooltip("When the rune is pick up from alter or ground")] public UnityEvent onPickup;
    [Tooltip("When the rune is pick up from ground")] public UnityEvent onGroundPickup;
    [Tooltip("When the rune is pick up from alter")] public UnityEvent onAlterPickup;
    [Tooltip("When player drops the rune to the ground")] public UnityEvent onDrop;
    [Tooltip("When player places the rune on alter")] public UnityEvent onAlterPlaced;
    [Tooltip("When rune is kick of alter by an effect (not player)")] public UnityEvent onAlterKicked;
}
