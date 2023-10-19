using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchCanvas : MonoBehaviour
{

    [SerializeField] public Canvas buttonPromptCanvas;

    private void Start()
    {
        buttonPromptCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            buttonPromptCanvas.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        buttonPromptCanvas.enabled = false;
    }
}