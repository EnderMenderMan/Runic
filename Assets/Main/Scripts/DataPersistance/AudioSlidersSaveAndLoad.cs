using UnityEngine;
using UnityEngine.UI;

public class AudioSlidersSaveAndLoad : MonoBehaviour, IDataPersitiens
{
    [SerializeField] private Slider[] audioSliders;

    public void LoadData(GameData data)
    {
        if (data.soundsVolume.Length != audioSliders.Length)
            return;
        for (int i = 0; i < audioSliders.Length; i++)
            audioSliders[i].value = data.soundsVolume[i];
    }

    public void SaveData(ref GameData data)
    {
        data.soundsVolume = new float[audioSliders.Length];
        for (int i = 0; i < audioSliders.Length; i++)
            data.soundsVolume[i] = audioSliders[i].value;
    }
}
