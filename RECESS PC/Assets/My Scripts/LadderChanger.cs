using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderChanger : MonoBehaviour
{
    [SerializeField] GameObject groundLadder;
    [SerializeField] GameObject upLadder;
    [SerializeField] GameObject enterSchoolSceneChanger;
    [SerializeField] public bool ladderChange = false;
    [SerializeField] public Canvas placeLadderButtonPromptCanvas;
    [SerializeField] InputActionReference enterKey;
    // [SerializeField] GameObject firstFloorLights;
    private void Start()
    {
        enterSchoolSceneChanger.SetActive(false);
        ladderChange = false;
        upLadder.SetActive(false);
        placeLadderButtonPromptCanvas.enabled = false;
        enterKey.action.performed += PlaceLadder;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ladderChange = true;
            placeLadderButtonPromptCanvas.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ladderChange = false;
        placeLadderButtonPromptCanvas.enabled = false;
    }

    public void PlaceLadder(InputAction.CallbackContext context)
    {
        if (ladderChange && placeLadderButtonPromptCanvas == true)
        {
            placeLadderButtonPromptCanvas.enabled = false;
            groundLadder.SetActive(false);
            upLadder.SetActive(true);
            enterSchoolSceneChanger.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
