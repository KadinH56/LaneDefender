using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int score;

    [SerializeField] private TMP_Text livesText;
    [SerializeField] private int lives;

    [SerializeField] private TMP_Text highScoreText;
    private int highScore;

    [SerializeField] private AudioClip lifeLostClip;
    private AudioSource audioSource;

    private void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();

        lives = 3;
        livesText.text = "Lives:" + lives.ToString();

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        topScore();

        audioSource = GetComponent<AudioSource>();
    }

    private void topScore()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    public void loseALife()
    {
        lives--;
        livesText.text = "Lives: " + lives.ToString();
        audioSource.PlayOneShot(lifeLostClip);

        if (lives <= 0)
        {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        Debug.Log("Enemy passed boundary, losing a life!");
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();

        //checks if it's a high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); // writes it to disk
        }

    }
}
