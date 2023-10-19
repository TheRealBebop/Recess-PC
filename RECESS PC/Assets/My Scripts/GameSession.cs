using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] float playerHealth = 100f;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scoreText;
    DeathHandler loadGameOver;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        healthText.text = playerHealth.ToString();
        scoreText.text = "Kill Count: " + score.ToString();
    }

    private void Update()
    {
        healthText.text = playerHealth.ToString();
        scoreText.text = "Kill Count: " + score.ToString();
    }

    public void AddToScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
