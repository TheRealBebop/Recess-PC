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
    [SerializeField] InputActionReference buttonPressedReference;
    public bool buttonPressed = false;

    private void Start()
    {
        buttonPressedReference.action.performed += onPressed;
    }

    public void onPressed(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
        {
            buttonPressed = true;
            QuitGame();
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
