using UnityEngine;
using System;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class Journal : MonoBehaviour, IDataPersitiens
{
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
    // public enum HintState
    // {
    //     NotAdded,
    //     Added,
    //     Removed,
    // }

    [System.Serializable]
    public struct Hint
    {
        [NonSerialized] public int state;
        public TextMeshProUGUI textElement;
        [TextArea(1, 3)] public string[] esay;
        [TextArea(1, 3)] public string[] normal;
        [TextArea(1, 3)] public string[] hard;
    }
    [System.Serializable]
    public struct ReplaceStringWithSpecial
    {
        [Tooltip("The text that will be replaced by the special character")] public string targetPart;
        [Tooltip("The special Character that will replace the targetPart")] public SpecialCharactersUI.Character character;
        [Tooltip("The width of the special character. This is text will replace the targetPart text so witdh should be specified with normal character exemple: \"___\" or with spaces like this \"   \" ")] public string withOfSpecialCharacter;
    }

    int hintOrderIndex;
    [SerializeField] HintType[] hintOrder;
    [SerializedDictionary("HintType", "Hint")] public SerializedDictionary<HintType, Hint> hints;
    Hint[] hintsArray;
    [SerializeField] ReplaceStringWithSpecial[] replaceStringsWithSpecialCharacters;

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

        hint.textElement.text = textArray[arrayIndex];
        SpecialCharactersUI.Instance.Destory(hintType.ToString());
        ReplaceWithSpecialCharacters(hint.textElement, hintType.ToString());
        return true;
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
            Vector3 spawnOffset = data.replace.withOfSpecialCharacter.Length % 2 == 0 ? Vector3.zero : Vector3.right * targetCharacterWidth / 2;

            SpecialCharactersUI.Instance.Create(data.replace.character, data.text, data.index, spawnOffset, data.size, data.id);
        }

    }
    void ReplaceWithSpecialCharacters(TextMeshProUGUI text, string id)
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
                    text.text = text.text.Insert(beginingOfCorrectString, replace.withOfSpecialCharacter);

                    foreach (CreateSpecialCharactersNextFrameData data in specialCreate)
                    {
                        if (data.index < i)
                            continue;
                        data.index += replace.withOfSpecialCharacter.Length - replace.targetPart.Length;
                    }

                    int targetSpawnIndex = beginingOfCorrectString + replace.withOfSpecialCharacter.Length / 2;
                    specialCreate.Add(new CreateSpecialCharactersNextFrameData
                    {
                        index = targetSpawnIndex,
                        text = text,
                        character = replace.character,
                        size = 1,
                        id = id,
                        replace = replace,
                    });

                    currentLookIndex = 0;
                    i = beginingOfCorrectString + replace.withOfSpecialCharacter.Length;
                    continue;
                }
                currentLookIndex++;
            }
        }
        StartCoroutine(CreateSpecialCharactersNextFrame(specialCreate));
    }

    void Awake()
    {
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
        if (data.journal.hintStates == null || hintsArray == null)
            return;

        for (int i = 0; i < hintsArray.Length && i < data.journal.hintStates.Length; i++)
        {
            if (hintsArray[i].state == data.journal.hintStates[i])
                continue;
            hintsArray[i].state = data.journal.hintStates[i];
            switch (hintsArray[i].state)
            {
                case 1:
                    TryTriggerHint((HintType)i);
                    break;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.journal.hintStates = new int[hintsArray.Length];
        for (int i = 0; i < hintsArray.Length; i++)
        {
            data.journal.hintStates[i] = hintsArray[i].state;
        }
    }
}
