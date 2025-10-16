using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayMainMenuMusic();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DayPhase"); 
    }
}
