using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GamePhase { Day, Night }

    [Header("Phase Settings")]
    [Tooltip("Set the starting phase for the scene.")]
    public GamePhase CurrentPhase;
    [SerializeField] private int m_maxDaySteps;

    [Header("Game State UI")]
    [SerializeField] private GameObject m_winScreen;
    [SerializeField] private GameObject m_loseScreen;
    [SerializeField] private TextMeshProUGUI m_stepsRemainingText;

    private int m_currentSteps;
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
        m_isGameOver = false;

        if (CurrentPhase == GamePhase.Day)
        {
            m_currentSteps = m_maxDaySteps;
        }
        else if (CurrentPhase == GamePhase.Night)
        {
            // TODO Setup Night Phase (i.e. show fire animation)
        }
        
        // Update the UI to show the starting steps.
        UpdateStepsUI();
    }

    /// <summary>
    /// Call this method from your player/camera controller every time it moves one step.
    /// </summary>
    public void UseStep()
    {
        if (m_isGameOver || m_currentSteps <= 0) return;

        m_currentSteps--;
        UpdateStepsUI();

        if (m_currentSteps <= 0)
        {
            // Player has run out of steps.
            TransitionToNextPhase();
        }
    }
    
    private void TransitionToNextPhase()
    {
        if (CurrentPhase == GamePhase.Day)
        {
            Debug.Log("Day has ended. Ran out of steps.");
            CurrentPhase = GamePhase.Night;
        }
        else if (CurrentPhase == GamePhase.Night)
        {
            Debug.Log("Night has ended.");
        }
    }
    
    /// <summary>
    /// Checks if the number of collected gun parts meets the win condition.
    /// </summary>
    public void CheckWinCondition(int currentGunPartCount)
    {
        if (currentGunPartCount >= GUN_PARTS_TO_WIN)
        {
            WinGame();
        }
    }

    // This method now updates the step counter UI.
    private void UpdateStepsUI()
    {
        if (m_stepsRemainingText != null)
        {
            m_stepsRemainingText.text = $"Steps Remaining: {m_currentSteps}";
        }
    }

    private void WinGame()
    {
        if (m_isGameOver) return;

        m_isGameOver = true;
        m_winScreen.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("You Win! All gun parts collected.");
    }

    private void LoseGame()
    {
        if (m_isGameOver) return;

        m_isGameOver = true;
        m_loseScreen.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("You Lose!");
    }
}