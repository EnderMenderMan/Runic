using System.Linq;
using UnityEngine;

public class TransformRune : Rune
{
[SerializedField] private FilterType TranformFilterType;

     public override void TriggerRunePlacement(int itemIndex, Alter[] alters) 
{
 for (int i = 0; i < alters.Length; i++)
            {
                if (i == itemIndex)
                    continue;
                if (alters[i].equippedRune == null)
                    continue;
               
            }
}
}
