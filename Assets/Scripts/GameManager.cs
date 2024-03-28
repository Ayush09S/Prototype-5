using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI livesText;

    public TextMeshProUGUI volumeText;
    public Slider volumeSlider;
    public AudioSource backgroundMusic;

    public Button restartButton;
    public GameObject titleScreen;

    public GameObject pausedGameScreen;

    public bool isGameActive;
    public int lives = 3;

    public bool isGamePaused = false;

    private float spawnRate = 1.0f;
    private int score;

    private void Update()
    {
        volumeText.text = "Volume:" + VolumeSlider(volumeSlider.value) + "%";
        backgroundMusic.volume = volumeSlider.value;
        PauseGame();
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);

        }
    }

    public void UpdateScore(int scoretToAdd)
    {
        score += scoretToAdd;
        scoreText.text = "Score: " + score;
    }

    public int VolumeSlider(float value) => Mathf.RoundToInt(value * 100);

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            isGamePaused = !isGamePaused;
        }

        if (isGamePaused)
        {
            Time.timeScale = 0;
            pausedGameScreen.gameObject.SetActive(true);

        }
        else if (!isGamePaused)
        {
            Time.timeScale = 1;
            pausedGameScreen.gameObject.SetActive(false);
        }
    }

    public void UpdateLives(int liveCount)
    {
        lives += liveCount;
        livesText.text = "Lives: " + lives;
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        score = 0;
        spawnRate /= difficulty;

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        UpdateLives(3);
        titleScreen.gameObject.SetActive(false);
    }
}
