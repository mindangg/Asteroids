using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player player;
    public ParticleSystem explosion;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI livesDisplay;
    public GameObject menu;
    private bool isPaused = false;
    public int lives = 3;
    private float score;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
                Resume();
            else if(!isPaused)
                Pause();
        }
    }

    public void AsteroidsDestroy(Asteroids asteroids)
    {
        explosion.transform.position = asteroids.transform.position;
        explosion.Play();

        if(asteroids.size < 0.75f)
            AddScore(100);
        else if(asteroids.size < 1.2f)
            AddScore(50);
        else
            AddScore(25);
    }
    
    public void PlayerDied()
    {
        explosion.transform.position = player.transform.position;
        explosion.Play();

        lives--;
        if(lives <= 0)
            GameOver();
        else
            Invoke(nameof(Respawn), 3);
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), 3);
    }

    private void TurnOnCollisions()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        lives = 3;
        score = 0;

        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }

    public void AddScore(float scoreToAdd)
    {
        score += scoreToAdd;
        scoreDisplay.text = score.ToString();
    }

    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
