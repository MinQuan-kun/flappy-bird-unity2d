using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Panels UI")]
    public GameObject startPanel;
    public GameObject gameOverPanel;

    [Header("Score Sprites (0 -> 9)")]
    public Sprite[] digitSprites;
    public Image[] digitImages;

    [Header("Gameplay")]
    public BirdController bird;
    public MonoBehaviour pipeSpawner;

    [Header("Âm thanh GameManager")]
    public AudioSource audioSource;
    public AudioClip scoreClip;
    public AudioClip dieClip;
    public float dieSoundDelay = 0.15f;

    private int score = 0;
    private bool isGameStarted = false;
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    void Start()
    {
        isGameStarted = false;
        isGameOver = false;
        score = 0;

        UpdateScoreUI(0);
        SetPanelState(showStart: true, showGameOver: false);

        if (pipeSpawner != null) pipeSpawner.enabled = false;
    }

    public void StartGame()
    {
        if (isGameStarted || isGameOver) return;

        isGameStarted = true;
        SetPanelState(showStart: false, showGameOver: false);

        if (pipeSpawner != null) pipeSpawner.enabled = true;
        if (bird != null) bird.BeginFlight();
    }

    public void AddScore()
    {
        if (isGameOver || !isGameStarted) return;

        score++;
        UpdateScoreUI(score);

        if (audioSource != null && scoreClip != null)
        {
            audioSource.PlayOneShot(scoreClip);
        }
    }

    private void UpdateScoreUI(int value)
    {
        if (digitSprites == null || digitSprites.Length < 10 || digitImages == null) return;

        string scoreStr = value.ToString();

        for (int i = 0; i < digitImages.Length; i++)
        {
            if (digitImages[i] == null) continue;

            if (i < scoreStr.Length)
            {
                int digit = scoreStr[i] - '0';
                digitImages[i].sprite = digitSprites[digit];
                digitImages[i].gameObject.SetActive(true);
            }
            else
            {
                digitImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        SetPanelState(showStart: false, showGameOver: true);

        StartCoroutine(PlayDieSoundAndPause());
    }

    private IEnumerator PlayDieSoundAndPause()
    {
        if (dieClip != null && audioSource != null)
        {
            yield return new WaitForSecondsRealtime(dieSoundDelay);
            audioSource.PlayOneShot(dieClip);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetPanelState(bool showStart, bool showGameOver)
    {
        if (startPanel != null) startPanel.SetActive(showStart);
        if (gameOverPanel != null) gameOverPanel.SetActive(showGameOver);
    }
}