using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneSwitcher : MonoBehaviour
{
    // [SerializeField] int nextSceneIndex;
    [SerializeField] Transform teleportPos;
    [SerializeField] public bool sceneChange = false;
    [SerializeField] InputActionReference enterKey;
    // [SerializeField] GameObject firstFloorLights;
    [SerializeField] public Canvas unlockedButtonPromptCanvas;
    [SerializeField] public Canvas lockedButtonPromptCanvas;
    public int hasBeenTraversed = 0;
    public bool switched = false;
    public bool locked = false;
    GameObject player;
    int currentSceneIndex;
    Scene scene;
    Player playerscript;
    Vector3 verticalPos;

    private void Start()
    {
        verticalPos = new Vector3(0f, 0.9f, 0f);
        sceneChange = false;
        unlockedButtonPromptCanvas.enabled = false;
        lockedButtonPromptCanvas.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
        playerscript = FindObjectOfType<Player>();
        scene = SceneManager.GetActiveScene();
        currentSceneIndex = scene.buildIndex;
        //enterKey.action.performed += Teleport;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Teleport();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (locked == false)
        {
            if (other.tag == "Player")
            {
                sceneChange = true;
                unlockedButtonPromptCanvas.enabled = true;
            }
        }
        else if (locked == true)
        {
            if (other.tag == "Player")
            {
                sceneChange = false;
                lockedButtonPromptCanvas.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (locked == false)
        {
            if (other.tag == "Player")
            {
                sceneChange = false;
                unlockedButtonPromptCanvas.enabled = false;
            }
        }
        else if (locked == true)
        {
            if (other.tag == "Player")
            {
                sceneChange = false;
                lockedButtonPromptCanvas.enabled = false;
            }
        }
    }

    public void Teleport()
    {
        if (sceneChange && unlockedButtonPromptCanvas == true)
        {
            // firstFloorLights.SetActive(false);
            player.transform.position = teleportPos.position + verticalPos;
            hasBeenTraversed += 1;
            switched = true;
        }
    }

    public bool HasSwitched()
    {
        if (hasBeenTraversed == 1 && switched == true)
        {
            switched = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}