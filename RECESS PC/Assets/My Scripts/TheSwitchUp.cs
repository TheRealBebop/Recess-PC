using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TheSwitchUp : MonoBehaviour
{
    // [SerializeField] GameObject mesh;
    [SerializeField] GameObject circuitBreakerOn;
    [SerializeField] InputActionReference enterKey;
    [SerializeField] GameObject redLights;

    // [SerializeField] GameObject firstFloorLights;
    GameObject player;
    Player playerscript;
    Vector3 verticalPos;
    LightSwitchCanvas lightSwitchCanvas;

    private void Start()
    {
        lightSwitchCanvas = FindObjectOfType<LightSwitchCanvas>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerscript = FindObjectOfType<Player>();
        enterKey.action.performed += TurnOn;
    }

    public void TurnOn(InputAction.CallbackContext context)
    {
        if (lightSwitchCanvas.buttonPromptCanvas.enabled == true)
        {
            gameObject.SetActive(false);
            circuitBreakerOn.SetActive(true);
            redLights.SetActive(true);
            playerscript.generatorTurnedOn = true;
        }
    }

}
