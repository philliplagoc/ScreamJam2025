using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Required for using TextMeshPro UI elements

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GamePhase { Day, Night }

    [Header("Phase Settings")] 
    [Tooltip("Set the starting phase for the scene.")]
    public GamePhase CurrentPhase;
    [SerializeField] private float m_dayDuration;
    [SerializeField] private float m_nightDuration;
    
    [Header("Game State UI")]
    [SerializeField] private GameObject m_winScreen; 
    [SerializeField] private GameObject m_loseScreen; 
    [SerializeField] private TextMeshProUGUI m_timerText;
    

    private float m_currentTime;
    private bool m_isGameOver = false;
    private const int GUN_PARTS_TO_WIN = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupPhase();
    }

    private void SetupPhase()
    {
        Time.timeScale = 1f;

        if (CurrentPhase == GamePhase.Day)
        {
            m_currentTime = m_dayDuration;
        }
        else if (CurrentPhase == GamePhase.Night)
        {
            m_currentTime = m_nightDuration;
        }
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
            TransitionToNextPhase();
        }
    }

    private void TransitionToNextPhase()
    {
        if (CurrentPhase == GamePhase.Day)
        {
            Debug.Log("Day has just ended.");
            CurrentPhase = GamePhase.Night;
            SceneManager.LoadScene("NightPhase");
        }
        else if (CurrentPhase == GamePhase.Night)
        {
            Debug.Log("Night has just ended.");
            CurrentPhase = GamePhase.Day;
            SceneManager.LoadScene("DayPhase");
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