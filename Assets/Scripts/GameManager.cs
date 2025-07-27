using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text scoreText;

    private int score = 0;
    private int missedRedEggs = 0;
    public int maxMisses = 3;

    public float eggFallSpeed = 2f;

    private float timer = 0f;
    public GameObject pausePanel;
    public GameObject failPanel; //  added this

    public GameObject redEggSplashPrefab;
    public Transform canvasTransform; // Drag your Canvas here in Inspector
    public bool isGameOver = false;

    public Image[] hearts;
    private int lives = 3;
    bool isPaused = false;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        UpdateScoreUI();
        Time.timeScale = 1f; // Just to be safe
        pausePanel.SetActive(false);
        failPanel.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 60f)
        {
            eggFallSpeed = 5f;
            EggSpawner.Instance.UpdateSpawnInterval(1.5f / eggFallSpeed);
        }
        else if (timer > 40f)
        {
            eggFallSpeed = 4f;
            EggSpawner.Instance.UpdateSpawnInterval(1.5f / eggFallSpeed);
        }
        else if (timer > 20f)
        {
            eggFallSpeed = 3f;
            EggSpawner.Instance.UpdateSpawnInterval(1.5f / eggFallSpeed);
        }
        else
            eggFallSpeed = 2f;
    }

    public void RedEggPopped()
    {
        score += 1;
        UpdateScoreUI();
    }

    public void RedEggMissed()
    {
        if (missedRedEggs < hearts.Length)
            hearts[missedRedEggs].enabled = false; // Hide current heart

        missedRedEggs++;
        if (missedRedEggs >= maxMisses)
            GameOver();
    }

    public void GameOver()
    {
        if (isGameOver) return; // prevent double-triggering

        isGameOver = true;
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        failPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    public void PauseGame() // added this
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}
