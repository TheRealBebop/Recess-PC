using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Endgame : MonoBehaviour
{
    [SerializeField] GameObject locomotionSystem;
    [SerializeField] Canvas endgameScreen;
    [SerializeField] TextMeshProUGUI scoreNumberText;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] Player gameSession;
    public bool gameOver = false;
    void Start()
    {
        endgameScreen.enabled = false;
        gameSession = FindObjectOfType<Player>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            scoreNumberText.text = gameSession.score.ToString();
            locomotionSystem.SetActive(false);
            //Time.timeScale = 0;
            endgameScreen.enabled = true;
            gameOver = true;
        }
    }

    private void Update()
    {
        if(endgameScreen.enabled && gameOver == true)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            else if(Input.GetKeyDown(KeyCode.Return))
            {
                sceneLoader.LoadScene();
            }
        }
    }
}
