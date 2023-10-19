using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightTurnOffinator : MonoBehaviour
{
    [SerializeField] GameObject lights;
    [SerializeField] InputActionReference enterKey;
    SceneSwitcher switcher;

    private void Start()
    {
        enterKey.action.performed += TurnOff;
        switcher = GetComponent<SceneSwitcher>();
    }
    public void TurnOff(InputAction.CallbackContext context)
    {
        if (switcher.sceneChange && switcher.unlockedButtonPromptCanvas == true)
            lights.SetActive(false);
    }
}
