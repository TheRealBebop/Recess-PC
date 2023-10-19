using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightTurnOninator : MonoBehaviour
{
    [SerializeField] GameObject lights;
    [SerializeField] InputActionReference enterKey;
    SceneSwitcher switcher;

    private void Start()
    {
        enterKey.action.performed += TurnOn;
        switcher = GetComponent<SceneSwitcher>();
    }
    public void TurnOn(InputAction.CallbackContext context)
    {
        if (switcher.sceneChange && switcher.unlockedButtonPromptCanvas == true)
            lights.SetActive(true);
    }
}
