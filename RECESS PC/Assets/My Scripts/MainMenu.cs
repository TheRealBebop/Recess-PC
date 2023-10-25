using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenuCanvas;
    [SerializeField] LocomotionSystem locomotionSystem;
    [SerializeField] SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        locomotionSystem.enabled = false;
        sceneLoader = GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainMenuCanvas.enabled == true)
        {
            Debug.Log("MAINMENU");
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.Return)) 
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
