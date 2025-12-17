using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  
 [SerializeField] Animator transitionAnimator;



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
