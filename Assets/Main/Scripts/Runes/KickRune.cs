using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KickRune : Rune
{
    private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
    [Tooltip("Filter and kick all runes that matches the filter (At least one Kick Filter returns true)")][SerializeField] private Filter[] kickFilters;
    [Tooltip("Used to select specific rune placements to run the Kick Filters on. IF EMPTY will run all runes. IF NOT EMPTY runs only selected indexes. -1 is one to the left and 2 is two to the right")][SerializeField] private int[] kickItemIndexOffsets;

    [SerializeField] float kickDelay;
    Coroutine kickAnimationCoroutine;

    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
        if (kickAnimationCoroutine != null)
            StopCoroutine(kickAnimationCoroutine);
        kickAnimationCoroutine = StartCoroutine(KickAnimation(kickDelay));
    }
    public override void OnKicked()
    {
        // when kick rune is kicked stop all other runes to be kicked
        // foreach (Alter filteredAlter in GetKickFilterAlters(alter.clusterIndex, alter.alterCluster.alters))
        //     filteredAlter.StopKickCorutine();
        base.OnKicked();
    }
    public IEnumerator KickAnimation(float delay)
    {
        countToAlterClusterComplete = false;
        if (delay > 0.01f)
            animator.SetTrigger(JumpTrigger);
        yield return new WaitForSeconds(delay);

        foreach (Alter filteredAlter in GetKickFilterAlters(alter.clusterIndex, alter.alterCluster.alters))
            filteredAlter.TryKickItem(false);

        countToAlterClusterComplete = true;
    }

    List<Alter> GetKickFilterAlters(int alterIndex, Alter[] alters)
    {
        List<Alter> selectedAlters = new List<Alter>();
        foreach (var filter in kickFilters)
        {
            if (kickItemIndexOffsets.Length == 0)
            {
                for (int i = 0; i < alters.Length; i++)
                {
                    if (i == alterIndex)
                        continue;
                    if (alters[i].equippedRune == null)
                        continue;
                    if (filter.RunFilter(alters[i].equippedRune.tags) == false)
                        continue;
                    selectedAlters.Add(alters[i]);
                }
                continue;
            }

            for (int i = 0; i < kickItemIndexOffsets.Length; i++)
            {
                int tryIndex = alterIndex + kickItemIndexOffsets[i];
                if (tryIndex < 0 || tryIndex >= alters.Length)
                    continue;
                if (alters[tryIndex].equippedRune == null)
                    continue;
                if (filter.RunFilter(alters[tryIndex].equippedRune.tags) == false)
                    continue;
                selectedAlters.Add(alters[tryIndex]);
            }
        }

        if (kickFilters.Length == 0)
        {
            for (int i = 0; i < kickItemIndexOffsets.Length; i++)
            {
                int tryIndex = alterIndex + kickItemIndexOffsets[i];

                if (tryIndex < 0 || tryIndex >= alters.Length)
                    continue;
                if (alters[tryIndex].equippedRune == null)
                    continue;
                selectedAlters.Add(alters[tryIndex]);
            }
        }
        return selectedAlters;
    }
}
