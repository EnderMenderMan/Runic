using UnityEngine;
using System;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Journal : MonoBehaviour, IDataPersitiens
{
    private static readonly int IsNotifying = Animator.StringToHash("IsNotifying");
    public static Journal Instance { get; private set; }
    public enum HintType
    {
        BasicRune,
        GrowRune,
        HideRune,
        KickRune,
        LockRune,
        SeeRune,
        RunRune,
        ShootRune,
        SpeedRune,
        SwapRune,
        TransformRune,
        WallRune,
        LastElementUsedOnlyForCode,
    }
    [System.Serializable]
    public struct Hint
    {
        [NonSerialized] public int state;
        [NonSerialized] public Coroutine displayTextCoroutune;
        public TextMeshProUGUI textElement;
        [TextArea(1, 6)] public string[] esay;
        [TextArea(1, 6)] public string[] normal;
        [TextArea(1, 6)] public string[] hard;
    }
    [System.Serializable]
    public struct ReplaceStringWithSpecial
    {
        [Tooltip("The text that will be replaced by the special character")] public string targetPart;
        [Tooltip("The special Character that will replace the targetPart")] public SpecialCharactersUI.Character character;
        [Tooltip("The width of the special character. This is text will replace the targetPart text so width should be specified with normal character example: \"___\" or with spaces like this \"   \" ")] public string widthOfSpecialCharacter;
    }

    int hintOrderIndex;
    [SerializeField] HintType[] hintOrder;
    [SerializedDictionary("HintType", "Hint")] public SerializedDictionary<HintType, Hint> hints;
    Hint[] hintsArray;
    [SerializeField] ReplaceStringWithSpecial[] replaceStringsWithSpecialCharacters;
    [SerializeField] private Animator journalAnimator;
    [SerializeField] private Transform bookPart;
    [Header("Special Characters")]
    [SerializeField] float specialCharacterSize;
    [SerializeField] bool specialCharactersScaleWithText;
    private Vector3 bookPartOriginalPosition;

    [Header("Debug")]
    [SerializeField] bool fillAllText;
    [SerializeField] bool clearAllText;

    AudioSource audioSource;

    public void BookClose()
    {
        audioSource.Stop();
        bookPart.position += Vector3.right * 10000;
        SoundManager.instance.PlaySound(SoundManager.SoundType.JournalClose, SoundManager.MixerType.SFX);
    }
    public void BookOpen()
    {
        audioSource.Play();
        bookPart.position = bookPartOriginalPosition;
        SoundManager.instance.PlaySound(SoundManager.SoundType.JournalOpen, SoundManager.MixerType.SFX);
    }

    public void CancelJournalNotifyAnimation() => journalAnimator.SetBool(IsNotifying, false);

    public void TriggerNextHint()
    {

        if (hintOrderIndex >= hintOrder.Length)
            return;


        TryTriggerHint(hintOrder[hintOrderIndex]);
        hintOrderIndex++;

        return;
    }
    public bool TryTriggerHint(HintType hintType)
    {
        if (hintsArray[(int)hintType].state < 0)
            return false;
        if (hintsArray[(int)hintType].textElement == null)
            return true;

        Hint targetHint = hintsArray[(int)hintType];
        switch (GameData.difficulty)
        {
            case GameData.Difficulty.Cissi:
                TrySetTextElementHint(targetHint, hintType, targetHint.esay);
                break;
            case GameData.Difficulty.Easy:
                TrySetTextElementHint(targetHint, hintType, targetHint.esay);
                break;
            case GameData.Difficulty.Normal:
                TrySetTextElementHint(targetHint, hintType, targetHint.normal);
                break;
            case GameData.Difficulty.Hard:
                TrySetTextElementHint(targetHint, hintType, targetHint.hard);
                break;
        }

        hintsArray[(int)hintType].state++;
        return true;
    }
    bool TrySetTextElementHint(Hint hint, HintType hintType, string[] textArray)
    {
        int arrayIndex = hint.state;
        if (textArray.Length <= arrayIndex)
            return false;

        if (GameData.difficulty != GameData.Difficulty.Cissi)
            journalAnimator.SetBool(IsNotifying, true);
        if (hasStartPlayingEntrySound == false && GameData.difficulty != GameData.Difficulty.Cissi)
        {
            hasStartPlayingEntrySound = true;
            StartCoroutine(PlayNewEntrySound());
        }


        hint.textElement.text = textArray[arrayIndex];
        SpecialCharactersUI.Instance.Destory(hintType.ToString());
        ReplaceWithSpecialCharacters(hint.textElement, hint.displayTextCoroutune, hintType.ToString());
        return true;
    }

    bool hasStartPlayingEntrySound = false;
    IEnumerator PlayNewEntrySound()
    {
        yield return null;
        SoundManager.instance.PlaySound(SoundManager.SoundType.JournalNewEntry, SoundManager.MixerType.SFX);
        hasStartPlayingEntrySound = false;

    }

    class CreateSpecialCharactersNextFrameData
    {
        public int index; public TextMeshProUGUI text; public SpecialCharactersUI.Character character; public float size; public string id; public ReplaceStringWithSpecial replace;
    }
    IEnumerator CreateSpecialCharactersNextFrame(List<CreateSpecialCharactersNextFrameData> datas)
    {
        yield return null;
        foreach (var data in datas)
        {
            TMP_CharacterInfo targetSpawnCharacter = data.text.textInfo.characterInfo[data.index];
            float targetCharacterWidth = Mathf.Abs(targetSpawnCharacter.bottomRight.x - targetSpawnCharacter.bottomLeft.x);
            Vector3 spawnOffset = data.replace.widthOfSpecialCharacter.Length % 2 == 0 ? Vector3.zero : Vector3.right * targetCharacterWidth / 2;
            float size = specialCharactersScaleWithText ? data.text.fontSize * 0.05f * data.size : data.size;

            SpecialCharactersUI.Instance.Create(data.replace.character, data.text, data.index, spawnOffset, size, data.id);
        }
    }
    void ReplaceWithSpecialCharacters(TextMeshProUGUI text, Coroutine createSpecialCharactersNextFrameCoroutine, string id)
    {
        List<CreateSpecialCharactersNextFrameData> specialCreate = new List<CreateSpecialCharactersNextFrameData>();
        foreach (ReplaceStringWithSpecial replace in replaceStringsWithSpecialCharacters)
        {
            int currentLookIndex = 0;
            int beginingOfCorrectString = 0;
            for (int i = 0; i < text.text.Length; i++)
            {
                bool foundCurrentLookCharacter = text.text[i] == replace.targetPart[currentLookIndex];
                if (foundCurrentLookCharacter == false && currentLookIndex == 0)
                    continue;

                if (foundCurrentLookCharacter == false)
                {
                    currentLookIndex = 0;
                    i = beginingOfCorrectString;
                    continue;
                }

                if (currentLookIndex == 0)
                {
                    beginingOfCorrectString = i;
                }

                if (currentLookIndex == replace.targetPart.Length - 1)
                {
                    text.text = text.text.Remove(beginingOfCorrectString, i - beginingOfCorrectString + 1);
                    text.text = text.text.Insert(beginingOfCorrectString, replace.widthOfSpecialCharacter);

                    foreach (CreateSpecialCharactersNextFrameData data in specialCreate)
                    {
                        if (data.index < i)
                            continue;
                        data.index += replace.widthOfSpecialCharacter.Length - replace.targetPart.Length;
                    }

                    int targetSpawnIndex = beginingOfCorrectString + replace.widthOfSpecialCharacter.Length / 2;
                    specialCreate.Add(new CreateSpecialCharactersNextFrameData
                    {
                        index = targetSpawnIndex,
                        text = text,
                        character = replace.character,
                        size = specialCharacterSize,
                        id = id,
                        replace = replace,
                    });

                    currentLookIndex = 0;
                    i = beginingOfCorrectString + replace.widthOfSpecialCharacter.Length;
                    continue;
                }
                currentLookIndex++;
            }
        }
        if (createSpecialCharactersNextFrameCoroutine != null)
            StopCoroutine(createSpecialCharactersNextFrameCoroutine);
        createSpecialCharactersNextFrameCoroutine = StartCoroutine(CreateSpecialCharactersNextFrame(specialCreate));
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        bookPartOriginalPosition = bookPart.position;
        bookPart.position += Vector3.right * 10000;

        Instance = this;
        hintsArray = new Hint[(int)HintType.LastElementUsedOnlyForCode];
        foreach (KeyValuePair<HintType, Hint> keyValuePair in hints)
            hintsArray[(int)keyValuePair.Key] = keyValuePair.Value;
        hints.Clear();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadData(GameData data)
    {
        if (GameData.difficulty == GameData.Difficulty.Hard)
            journalAnimator.gameObject.SetActive(false);
        else if (GameData.difficulty == GameData.Difficulty.Cissi)
            ShowAllHintsWithoutTrigger();

        if (data.journal.hintStates == null || hintsArray == null)
            return;

        hasStartPlayingEntrySound = true;
        for (int i = 0; i < hintsArray.Length && i < data.journal.hintStates.Length; i++)
        {
            if (hintsArray[i].state == data.journal.hintStates[i])
                continue;
            for (int j = 0; j < data.journal.hintStates[i] - hintsArray[i].state; j++)
                TryTriggerHint((HintType)i);
        }
        hasStartPlayingEntrySound = false;
        hintOrderIndex = data.journal.currentHintIndex;
        if (data.journal.isPlayingNotifyAnimation == false || GameData.difficulty == GameData.Difficulty.Cissi)
            journalAnimator.SetBool(IsNotifying, false);
    }

    public void SaveData(ref GameData data)
    {
        data.journal.isPlayingNotifyAnimation = journalAnimator.GetBool(IsNotifying);

        if (data.isSavingGameData == false)
            return;

        data.journal.hintStates = new int[hintsArray.Length];
        for (int i = 0; i < hintsArray.Length; i++)
        {
            data.journal.hintStates[i] = hintsArray[i].state;
        }

        data.journal.currentHintIndex = hintOrderIndex;
    }
    public int GetLoadPriority() => 10;

    public void ShowAllHintsWithoutTrigger()
    {
        int[] tempStatesArray = new int[hintsArray.Length];
        for (int i = 0; i < hintsArray.Length; i++)
            tempStatesArray[i] = hintsArray[i].state;

        for (int i = hintOrderIndex; i < hintOrder.Length; i++)
            TryTriggerHint(hintOrder[i]);

        for (int i = 0; i < hintsArray.Length; i++)
            hintsArray[i].state = tempStatesArray[i];
    }

    void OnValidate()
    {
        if (fillAllText)
        {
            fillAllText = false;
            foreach (var pair in hints)
            {
                pair.Value.textElement.text = pair.Value.normal[^1];
            }

        }
        if (clearAllText)
        {
            clearAllText = false;
            foreach (var pair in hints)
                pair.Value.textElement.text = "";
        }
    }
}
