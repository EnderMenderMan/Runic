[System.Serializable]
public class GameData
{
    // music
    public float[] soundsVolume;

    public void NewLevelDataReset()
    {
    }

    public GameData()
    {
        soundsVolume = new float[0];
    }
    public GameData(float[] volumes)
    {
        soundsVolume = volumes;
    }
}
