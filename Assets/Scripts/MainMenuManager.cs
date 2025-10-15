using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// This public method will be called by the Start Game button.
    /// </summary>
    public void StartGame()
    {
        // Loads the scene that contains your main game.
        // Make sure the scene name matches your game scene file exactly.
        SceneManager.LoadScene("DayPhase"); 
    }
}
