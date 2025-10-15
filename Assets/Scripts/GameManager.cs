using System.Collections;
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
    [SerializeField] private int m_woodCostPerNight;
    [SerializeField] private float m_phaseTransitionDelay;

    [Header("Cycle Tracking")] 
    [SerializeField] private TextMeshProUGUI m_dayCountText;
    private int m_dayCount = 1;

    [Header("Game State UI")]
    [SerializeField] private GameObject m_winScreen;
    [SerializeField] private GameObject m_loseScreen;
    [SerializeField] private TextMeshProUGUI m_stepsRemainingText;

    [Header("Scene Visuals")] 
    [SerializeField] private GameObject m_dayBackground;
    [SerializeField] private GameObject m_nightBackground;
    [SerializeField] private GameObject m_firePrefab;
    [SerializeField] private GameObject m_firePit;  // The fire pit that we see in the Day Phase
    private GameObject m_fireInstance;

    [Header("Night Settings")] 
    [SerializeField] private int m_nightStartCol;
    [SerializeField] private int m_nightStartRow;

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
        // Initial setup for the first day
        SetupDayPhase();
    }

    private void SetupDayPhase()
    {
        CurrentPhase = GamePhase.Day;
        Time.timeScale = 1f;
        m_isGameOver = false;

        // Visuals
        if (m_dayBackground != null) m_dayBackground.SetActive(true);
        if (m_nightBackground != null) m_nightBackground.SetActive(false);
        if (m_fireInstance != null) Destroy(m_fireInstance);  // Clean up fire from previous night
        if (m_firePit != null) m_firePit.SetActive(true);
        
        
        // Reset steps for the new day
        m_currentSteps = m_maxDaySteps;
        UpdateStepsUI();
        UpdateDayCountUI();
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
            StartCoroutine(NightSequence());
        }
    }

    private IEnumerator NightSequence()
    {
        Debug.Log("Day has ended. Night begins...");
        CurrentPhase = GamePhase.Night;
        
        // Move camera to where the fire pit is
        if (CameraController.Instance != null)
        {
            CameraController.Instance.SetGridPosition(m_nightStartCol, m_nightStartRow);
        }
        
        // Transition visuals to night
        if (m_dayBackground != null) m_dayBackground.SetActive(false);
        if (m_nightBackground != null) m_nightBackground.SetActive(true);
        if (m_firePrefab != null)
            m_fireInstance = Instantiate(m_firePrefab, new Vector3(-4.58f, -4.02f, 0f), Quaternion.identity);
        if (m_firePit != null) m_firePit.SetActive(false);
        
        // Wait for a moment
        yield return new WaitForSeconds(m_phaseTransitionDelay);
        
        // TODO Check for survival
        Debug.Log($"Checking for wood. Cost: {m_woodCostPerNight}");
        bool survived = InventoryManager.Instance.RemoveItem(ItemType.Wood, m_woodCostPerNight);

        if (survived)
        {
            Debug.Log("Survived the night!");
            yield return new WaitForSeconds(m_phaseTransitionDelay);

            m_dayCount++;
            SetupDayPhase();
        }
        else
        {
            Debug.Log("Not enough wood to survive the night...");
            LoseGame();
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
    private void UpdateStepsUI()
    {
        if (m_stepsRemainingText != null)
        {
            m_stepsRemainingText.text = $"Steps Remaining: {m_currentSteps}";
        }
    }
    
    private void UpdateDayCountUI()
    {
        if (m_dayCountText != null)
        {
            m_dayCountText.text = $"Day: {m_dayCount}";
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