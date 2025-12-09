using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  [Tooltip("When you press th escape button")] public UnityEvent OnESCPressed;
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape)) 
      OnESCPressed.Invoke();

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
