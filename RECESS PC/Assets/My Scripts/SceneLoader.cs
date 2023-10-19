using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] InputActionReference buttonPressedReference;
    public bool buttonPressed = false;
    // DeathHandler playerIsDead;

    private void Start()
    {
        buttonPressedReference.action.performed += onClick;
        // playerIsDead = GetComponent<DeathHandler>();
    }

    public void onClick(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
        {
            buttonPressed = true;
            LoadScene();
        }
    }

    public void LoadScene()
    {
        // enemiesPersist.enabled = false;
        SceneManager.LoadScene(1);
        buttonPressed = false;
        Debug.Log("Scene Reloaded");
        Time.timeScale = 1;
    }

}
