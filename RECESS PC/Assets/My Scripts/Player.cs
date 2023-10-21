using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] public float playerHealth = 100f;
    [SerializeField] public int score = 0;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] GameObject toppledWall;
    [SerializeField] GameObject horde;
    [SerializeField] ParticleSystem smoke;
    [SerializeField] AudioSource wallExplosionSound;
    [SerializeField] TextMeshProUGUI finalScoreText;
    DeathHandler loadGameOver;
    public bool isDead = false;
    public bool generatorTurnedOn;
    private void Start()
    {
        healthText.text = playerHealth.ToString();
        scoreText.text = "Score: " + score.ToString();
    }
    public void TakeDamage(float enemyDamage)
    {
        playerHealth -= enemyDamage;
        if (playerHealth <= 0)
        {
            isDead = true;
            loadGameOver = FindObjectOfType<DeathHandler>();
            loadGameOver.HandleDeath();
            Debug.Log("Yer dead");
            // Debug.Log("Number of TRIANGLES IN THE SCENE:" + UnityEditor.UnityStats.triangles);
        }
    }

    public void Heal()
    {
        playerHealth = 100f;
    }

    public void BreakWall()
    {
        if (generatorTurnedOn == true)
        {
            toppledWall.GetComponent<Animator>().SetTrigger("WALL FALL");
            horde.SetActive(true);
            Debug.Log("Horde ACTIVE");
            smoke.Play();
            wallExplosionSound.Play();
            Debug.Log("WALL FALLEN SOUND");
            generatorTurnedOn = false;
        }
    }

    public void DeleteDuplicates()
    {
        int numPlayer = FindObjectsOfType<Player>().Length;
        if (numPlayer > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Update()
    {
        healthText.text = "Health: " + playerHealth.ToString();
        scoreText.text = "Score: " + score.ToString();
        finalScoreText.text = scoreText.text;
    }

    public void AddToScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
}
