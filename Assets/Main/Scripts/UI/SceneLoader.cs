using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  
 [SerializeField] Animator transitionAnimator;

 public void LoadWithDelay(int buildIndex)
 {
     StartCoroutine(LoadWithDela(buildIndex));
 }

 IEnumerator LoadWithDela(int buildIndex)
 {
     transitionAnimator.SetTrigger("End");
     yield return new WaitForSeconds(1);
     SceneManager.LoadScene(buildIndex);
     transitionAnimator.SetTrigger("Start");
 }
  public void LoadScene(int buildIndex)
  {
      SceneManager.LoadScene(buildIndex);
      Debug.Log("Scene load finished");
  }
  public void QuitGame()
  {
    Application.Quit();
    Debug.Log("Quit Game");
  }
}
