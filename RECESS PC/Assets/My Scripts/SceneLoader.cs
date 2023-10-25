using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SceneLoader : MonoBehaviour
{
    //[SerializeField] InputActionReference buttonPressedReference;
    public bool buttonPressed = false;
    [SerializeField] Player player
        ;

    // DeathHandler playerIsDead;

    private void Start()
    {
        //buttonPressedReference.action.performed += onClick;
        // playerIsDead = GetComponent<DeathHandler>();
        //player = GetComponent<Player>();
    }

    /*
    public void onClick(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
        {
            buttonPressed = true;
            LoadScene();
        }
    }

    */

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            buttonPressed = true;
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if(buttonPressed == true && player.IsPlayerDead() == true)
        {
            // enemiesPersist.enabled = false;
            SceneManager.LoadScene(1);
            buttonPressed = false;
            Debug.Log("Scene Reloaded");
            Time.timeScale = 1;
        }

        else
        {
            Debug.Log("Not Reloading");
        }
    }

}
