using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameQuitter : MonoBehaviour
{
    //     ScenePersistForEnemiesAndPickups enemiesPersist;

    //     private void Start()
    //     {
    //         enemiesPersist = FindObjectOfType<ScenePersistForEnemiesAndPickups>().GetComponent<ScenePersistForEnemiesAndPickups>();
    //     }
    //[SerializeField] InputActionReference buttonPressedReference;
    public bool buttonPressed = false;
    [SerializeField] Player player;


    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            buttonPressed = true;
            onPressed();
        }
    }

    public void onPressed()
    {
        if (buttonPressed == true && player.IsPlayerDead() == true)
        {
            buttonPressed = true;
            QuitGame();
        }
        else
        {
            Debug.Log("Not quitting");
            buttonPressed = false;
        }
    }

    public void QuitGame()
    {
        // enemiesPersist.enabled = false;
        Application.Quit();
        buttonPressed = false;
        // Debug.Log("Scene Reloaded");
        // Time.timeScale = 1;
    }
}
