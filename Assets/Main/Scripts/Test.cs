using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [SerializeField] private bool[] bools;

    public void TestLog(string input = "TEST")
    {
        Debug.Log(input);
    }

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
