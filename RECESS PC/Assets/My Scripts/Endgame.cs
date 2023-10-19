using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Endgame : MonoBehaviour
{
    [SerializeField] GameObject locomotionSystem;
    [SerializeField] Canvas endgameScreen;
    [SerializeField] TextMeshProUGUI scoreNumberText;
    Player player;
    void Start()
    {
        endgameScreen.enabled = false;
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // scoreNumberText = player.scoreText;
            locomotionSystem.SetActive(false);
            Time.timeScale = 0;
            endgameScreen.enabled = true;
        }
    }
}
