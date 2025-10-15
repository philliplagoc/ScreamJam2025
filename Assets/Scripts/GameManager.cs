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
    // [SerializeField] private int m_maxDaySteps;
    // [SerializeField] private int m_woodCostPerNight;
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

    [Header("Movement Buttons")] 
    [SerializeField] private GameObject m_moveNorthButton;
    [SerializeField] private GameObject m_moveSouthButton;
    [SerializeField] private GameObject m_moveEastButton;
    [SerializeField] private GameObject m_moveWestButton;

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
        // Call CameraController to advance day and update respawn timers
        if (CameraController.Instance != null && m_dayCount > 1)
        {
            CameraController.Instance.AdvanceDay();
        }
        
        CurrentPhase = GamePhase.Day;
        Time.timeScale = 1f;
        m_isGameOver = false;

        // Visuals
        if (m_dayBackground != null) m_dayBackground.SetActive(true);
        if (m_nightBackground != null) m_nightBackground.SetActive(false);
        if (m_fireInstance != null) Destroy(m_fireInstance);  // Clean up fire from previous night
        if (m_firePit != null) m_firePit.SetActive(true);
        if (m_dayCountText != null) m_dayCountText.gameObject.SetActive(true);
        if (m_stepsRemainingText != null) m_stepsRemainingText.gameObject.SetActive(true);
        
        // Movement buttons
        if (m_moveNorthButton != null) m_moveNorthButton.SetActive(true);
        if (m_moveSouthButton != null) m_moveSouthButton.SetActive(true);
        if (m_moveEastButton != null) m_moveEastButton.SetActive(true);
        if (m_moveWestButton != null) m_moveWestButton.SetActive(true);
        
        
        // Reset steps for the new day
        m_currentSteps = CalculateNumberOfStepsForThisDay(m_dayCount);
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
        
        // Hide the 4 cardinal buttons so player can't move during the NightPhase
        if (m_moveNorthButton != null) m_moveNorthButton.SetActive(false);
        if (m_moveSouthButton != null) m_moveSouthButton.SetActive(false);
        if (m_moveEastButton != null) m_moveEastButton.SetActive(false);
        if (m_moveWestButton != null) m_moveWestButton.SetActive(false);
        
        // Transition visuals to night
        if (m_dayBackground != null) m_dayBackground.SetActive(false);
        if (m_nightBackground != null) m_nightBackground.SetActive(true);
        if (m_firePrefab != null)
        {
            // These coordinates correspond to where the fire pit is located in the day scene
            m_fireInstance = Instantiate(m_firePrefab, new Vector3(-4.58f, -4.02f, 0f), Quaternion.identity);
        }
        if (m_firePit != null) m_firePit.SetActive(false);
        if (m_dayCountText != null) m_dayCountText.gameObject.SetActive(false);
        if (m_stepsRemainingText != null) m_stepsRemainingText.gameObject.SetActive(false);
        
        // Wait for a moment
        yield return new WaitForSeconds(m_phaseTransitionDelay);
        
        // Check for survival
        Debug.Log($"Checking for wood. Cost: {CalculateWoodNeededForThisNight(m_dayCount)}");
        bool survived =
            InventoryManager.Instance.RemoveItem(ItemType.Wood, CalculateWoodNeededForThisNight(m_dayCount));

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

    /// <summary>
    /// These are the number of steps needed for the given day
    /// </summary>
    private int CalculateNumberOfStepsForThisDay(int n)
    {
        // Steps = StartingSteps + (StepsIncrease * (n - 1))
        return 8 + (3 * (n - 1));
    }

    private int CalculateWoodNeededForThisNight(int n)
    {
        // WoodNeeded = round(StartingWood * (GrowthRate ^ (n - 1)))
        return Mathf.RoundToInt(6 * Mathf.Pow(1.35f, (n - 1)));
    }    
}