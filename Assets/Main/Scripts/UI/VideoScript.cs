
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class videoscript : MonoBehaviour
{

    [SerializeField] VideoPlayer video;
    [SerializeField] int buildIndex;

    private void Awake()
    {
        video.Play();
        video.loopPointReached += CheckOver;
    }
    
    private void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(buildIndex);
    }
}