using UnityEngine;
using TMPro; // Required for using TextMeshPro UI elements

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Timer Settings")]
    [Tooltip("The total time for the game in seconds.")]
    [SerializeField] private float m_gameDuration;
    [SerializeField] private TextMeshProUGUI m_timerText;

    [Header("Game State UI")]
    [SerializeField] private GameObject m_winScreen; 
    [SerializeField] private GameObject m_loseScreen; 

    private float m_currentTime;
    private bool m_isGameOver = false;
    private const int GUN_PARTS_TO_WIN = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_currentTime = m_gameDuration;
        m_winScreen.SetActive(false);
        m_loseScreen.SetActive(false);
        UpdateTimeUI();
    }

    private void Update()
    {
        // If the game is over, do nothing.
        if (m_isGameOver) return;
        
        // --- Countdown Timer Logic ---
        if (m_currentTime > 0)
        {
            m_currentTime -= Time.deltaTime;
            UpdateTimeUI();
        }
        else
        {
            // Timer has run out
            m_currentTime = 0;
            LoseGame();
        }
    }
    
    /// <summary>
    /// Checks if the number of collected gun parts meets the win condition.
    /// This should be called by the InventoryManager whenever a gun part is collected.
    /// </summary>
    public void CheckWinCondition(int currentGunPartCount)
    {
        if (currentGunPartCount >= GUN_PARTS_TO_WIN)
        {
            WinGame();
        }
    }

    private void UpdateTimeUI()
    {
        // Format the time into minutes and seconds for display
        string timerText = "Time: 00:00";
        if (m_currentTime > 0)
        {
            int minutes = Mathf.FloorToInt(m_currentTime / 60);
            int seconds = Mathf.FloorToInt(m_currentTime % 60);
            timerText = $"Time: {minutes:00}:{seconds:00}";    
        }
        
        m_timerText.text = timerText;
    }

    private void WinGame()
    {
        if (m_isGameOver) return; // Prevent multiple triggers

        m_isGameOver = true;
        m_winScreen.SetActive(true);
        Debug.Log("You Win! All gun parts collected.");
        
        // Optional: Freeze game time
        Time.timeScale = 0f;
    }

    private void LoseGame()
    {
        if (m_isGameOver) return; // Prevent multiple triggers

        m_isGameOver = true;
        m_loseScreen.SetActive(true);
        Debug.Log("You Lose! Time ran out.");
        
        // Optional: Freeze game time
        Time.timeScale = 0f;
    }
}