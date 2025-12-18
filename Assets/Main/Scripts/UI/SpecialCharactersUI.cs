using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SpecialCharactersUI : MonoBehaviour
{
    public static SpecialCharactersUI Instance { get; private set; }
    public enum Character
    {
        BasicRune,
        KickRune,
        WallRune,
        LockRune,
    }
    [SerializeField] Transform characterHolder;
    [SerializedDictionary("Character", "Prefab")] public SerializedDictionary<Character, GameObject> prefabs;

    public GameObject Create(Character character, TMPro.TextMeshProUGUI textElement, int textIndex, Vector3 offset, float scale = 1, string id = "None")
    {
        Vector3 position = textElement.textInfo.characterInfo[textIndex].bottomLeft + offset;
        position = textElement.transform.TransformPoint(position);
        return Create(character, position, scale, id);

    }
    public GameObject Create(Character character, Vector3 position, float scale = 1, string id = "None")
    {
        GameObject prefab;
        if (prefabs.TryGetValue(character, out prefab) == false)
            return null;

        GameObject createdGameObject = Instantiate(prefab, position, Quaternion.identity, characterHolder);
        createdGameObject.transform.localScale *= scale;
        createdGameObject.name = id;
        return createdGameObject;
    }
    public void Destory(string id)
    {
        foreach (Transform child in characterHolder.GetComponentInChildren<Transform>())
        {
            if (child == null || child == characterHolder || child.name != id)
                continue;
            Destroy(child.gameObject);
        }
    }
    public GameObject GetPrefab(Character character)
    {
        GameObject prefab;
        if (prefabs.TryGetValue(character, out prefab) == false)
            return null;
        return prefab;

    }
    void Awake()
    {
        Instance = this;
    }
}
