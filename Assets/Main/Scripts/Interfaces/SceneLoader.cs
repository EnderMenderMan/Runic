using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
      LoadScene(0);
  }

  public void TestLog(string input = "TEST")
  {
    Debug.Log(input);
  }

  public void LoadScene(int buildIndex)
  {
    SceneManager.LoadScene(buildIndex);
  }
  public void QuitGame()
  {
    Application.Quit();
    Debug.Log("Quit Game");
  }
}
