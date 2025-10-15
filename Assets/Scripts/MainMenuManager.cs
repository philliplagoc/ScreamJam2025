using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic("MainMenuBackgroundMusic");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DayPhase"); 
    }
}
