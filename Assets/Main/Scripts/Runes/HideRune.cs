using UnityEngine;

public class HideRune : Rune
{
    [Tooltip("The object that will be shown instead of the normal render of the rune when hiding is acivated")][SerializeField] GameObject hidePrefab;
    [Tooltip("This are the objects that will be SetActive to false. When selecting runes select the object that render the rune and not the whole rune")][SerializeField] GameObject[] selectedRunesRenderObjectsToHide;
    GameObject[] createdHideObjects;

    protected override void Awake()
    {
        base.Awake();

        createdHideObjects = new GameObject[selectedRunesRenderObjectsToHide.Length];
        for (int i = 0; i < selectedRunesRenderObjectsToHide.Length; i++)
        {
            GameObject hideObject = selectedRunesRenderObjectsToHide[i];
            createdHideObjects[i] = Instantiate(hidePrefab, hideObject.transform.position, hideObject.transform.rotation, hideObject.transform.parent);
            createdHideObjects[i].name = "HideRender";
            createdHideObjects[i].SetActive(false);
        }
    }

    void SetObjectActive(bool hideObj, bool renderObj)
    {
        for (int i = 0; i < createdHideObjects.Length; i++)
        {
            createdHideObjects[i].SetActive(hideObj);
            selectedRunesRenderObjectsToHide[i].SetActive(renderObj);
        }
    }

    public void HideObjects() => SetObjectActive(true, false);
    public void ShowObjects() => SetObjectActive(false, true);
}
