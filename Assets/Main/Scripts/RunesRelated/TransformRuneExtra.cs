using UnityEngine;

[RequireComponent(typeof(TransformRune))]
public class TransformRuneExtra : MonoBehaviour
{
    TransformRune transformRune;
    void Awake()
    {
        transformRune = GetComponent<TransformRune>();
    }
    public void SwitchIfEmpty()
    {
        var alterData = transformRune.GetAlters();
        bool foundAlter = false;
        for (int i = 0; i < alterData.alters.Length; i++)
        {
            if (i == alterData.thisAlterIndex || alterData.alters[i].equippedRune == null)
                continue;
            foundAlter = true;
            break;
        }
        if (foundAlter)
            return;
        transformRune.SwapSelectedRune();
    }
}
