using System;
using UnityEngine;
using UnityEngine.Events;
public class SceneLoaderExtra : MonoBehaviour
{
    [Tooltip("When you press the escape button")] public UnityEvent OnESCPressed;
    [Tooltip("When you press the J button")] public UnityEvent OnJPressed;
    [Tooltip("Trigges of gameobject is enabled when difficulty is set to hard")] public UnityEvent OnHardDifficulty;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnESCPressed.Invoke();
        if (GameData.difficulty != GameData.Difficulty.Hard && Input.GetKeyDown(KeyCode.J))
            OnJPressed.Invoke();
    }
    public void ChangeDifficulty(int value) => GameData.difficulty = (GameData.Difficulty)value;

    void OnEnable()
    {
        if (GameData.difficulty == GameData.Difficulty.Hard)
            OnHardDifficulty.Invoke();
    }

    //public void ContinuesGame(MenuScritablleObject menuScritablleObject) { MenuScritablleObject.continueGame = true; }
}
