[System.Serializable]
public class GameData
{
    public enum Difficulty
    {
        Cissi,
        Easy,
        Normal,
        Hard,
    }
    public struct JournalSaveData
    {
        public int[] hintStates;
    }

    public (float x, float y) playerPosition;
    public int loadedSceneIndex;
    public int loadedLevelIndex;
    public static Difficulty difficulty = Difficulty.Normal;
    public JournalSaveData journal;
    // music
    public float[] soundsVolume;

    public bool isSavingGameData;

    public void NewLevelDataReset()
    {
        loadedSceneIndex = -1;
        loadedLevelIndex = -1;
        journal.hintStates = new int[0];
    }

    public GameData()
    {
        NewLevelDataReset();
        journal.hintStates = new int[0];
        soundsVolume = new float[0];
    }
    public GameData(float[] volumes, int loadedLevelIndex, int loadedSceneIndex)
    {
        this.loadedSceneIndex = loadedSceneIndex;
        this.loadedLevelIndex = loadedLevelIndex;
        soundsVolume = volumes;
    }
}
