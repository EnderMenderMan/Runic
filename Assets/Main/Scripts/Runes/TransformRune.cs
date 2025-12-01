using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TransformRune : Rune
{
    [SerializeField][Tooltip("The gameObject that displays this transformRunes graphics")] private GameObject renderObject;
    [SerializeField][Tooltip("When transformed: Dont delete this TransformRune originals tags")] private bool alwaysKeepOriginalTags;
    [SerializeField][Tooltip("When transformed: Gets the tags form the targeted transformed rune")] private bool getTagsFromTransform;
    Rune selectedChild;
    [SerializeField][Tooltip("Replaces all runes in transformToRunes array with all runes that are a child of this TransformRune")] private AutoGetRunes autoGetRunes;
    [SerializeField][Tooltip("Runes that this TransformRune can transform to. The runes in this array needs to be a child of this TransformRune")] Rune[] transformToRunes;
    int selectedRuneIndex = -1;
    private HashSet<string> ogTags = new HashSet<string>();

    [Serializable]
    struct AutoGetRunes
    {
        [Tooltip("Runes that this TransformRune can transform to. The runes in this array needs to be a child of this TransformRune")] public bool enable;
        [Tooltip("Make sure this TransformRune is not part of the transformToRunes array")] public bool ignoreThisTransformRune;
    }

    protected override void Awake()
    {
        GetTransformToRunes();
        SetToCorrectStartingIndex();

        foreach (var rune in transformToRunes)
            if (rune.gameObject != gameObject)
                rune.gameObject.SetActive(false);

        base.Awake();
        ogTags.UnionWith(tags.ids);
    }

    void SetToCorrectStartingIndex()
    {
        if (transformToRunes.Length == 0)
            return;
        if (transformToRunes[0].gameObject == gameObject)
            selectedRuneIndex = 0;
        else
            selectedRuneIndex = -1;
    }
    void GetTransformToRunes()
    {
        if (autoGetRunes.enable == false)
            return;

        Rune[] runes = GetComponentsInChildren<Rune>();
        if (autoGetRunes.ignoreThisTransformRune == false)
        {
            transformToRunes = runes;
            return;
        }

        transformToRunes = new Rune[runes.Length - 1];
        for (int i = 0; i < transformToRunes.Length; i++)
            transformToRunes[i] = runes[i + 1];
    }

    public void Reset()
    {
        tags.ids.Clear();
        tags.ids.UnionWith(ogTags);
        SetRuneActive(selectedChild, false);
        renderObject.SetActive(true);
        selectedChild = null;
        SetToCorrectStartingIndex();
    }
    public void SwapSelectedRune() => SwapSelectedRune((selectedRuneIndex + 1) % transformToRunes.Length);
    public void SwapSelectedRune(int index)
    {
        if (transformToRunes.Length == 0)
            return;

        selectedRuneIndex = index;
        if (selectedChild == null)
        {
            renderObject.SetActive(false);
            selectedChild = transformToRunes[selectedRuneIndex];
        }

        SetRuneActive(selectedChild, false);
        HandleTagsSwap();
        selectedChild = transformToRunes[selectedRuneIndex];
        SetRuneActive(selectedChild, true);
    }

    void SetRuneActive(Rune rune, bool value)
    {
        if (rune.gameObject == gameObject)
            renderObject.SetActive(value);
        else
            selectedChild.gameObject.SetActive(value);
    }

    void HandleTagsSwap()
    {
        if (getTagsFromTransform == false)
            return;
        tags.ids.Clear();
        tags.ids.UnionWith(transformToRunes[selectedRuneIndex].tags.ids);

        if (alwaysKeepOriginalTags == false)
            return;
        tags.ids.UnionWith(ogTags);
    }


    void TryRunSelectedChildFunction(Action childFunction, Action backupFunction)
    {
        if (selectedChild == null || selectedChild.gameObject == gameObject)
            backupFunction();
        else
            childFunction();
    }
    public override void OnPickUp() => TryRunSelectedChildFunction(() => selectedChild.OnPickUp(), base.OnPickUp);
    public override void OnGroundPickUp() => TryRunSelectedChildFunction(() => selectedChild.OnGroundPickUp(), base.OnGroundPickUp);
    public override void OnAlterPickUp() => TryRunSelectedChildFunction(() => selectedChild.OnAlterPickUp(), base.OnAlterPickUp);
    public override void OnDropped() => TryRunSelectedChildFunction(() => selectedChild.OnDropped(), base.OnDropped);
    public override void OnKicked() => TryRunSelectedChildFunction(() => selectedChild.OnKicked(), base.OnDropped);

    public override void AfterDropped() => TryRunSelectedChildFunction(() => selectedChild.AfterDropped(), base.AfterDropped);
    public override void AfterKicked() => TryRunSelectedChildFunction(() => selectedChild.AfterKicked(), base.AfterKicked);
    public override void AfterAlterPlace() => TryRunSelectedChildFunction(() => selectedChild.AfterAlterPlace(), base.AfterAlterPlace);



    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
        if (selectedChild == null || selectedChild.gameObject == gameObject)
            base.TriggerRunePlacement(itemIndex, alters);
        else
            selectedChild.TriggerRunePlacement(itemIndex, alters);
    }

    public override bool TryBePlaced(int alterIndex, Alter[] alters, AlterCluster cluster)
    {
        if (selectedChild == null || selectedChild.gameObject == gameObject)
            return base.TryBePlaced(alterIndex, alters, cluster);

        return selectedChild.TryBePlaced(alterIndex, alters, cluster);
    }
}
