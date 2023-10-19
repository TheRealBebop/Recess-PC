using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MyWeaponWheel : MonoBehaviour
{

    // [SerializeField] Canvas handMenu;
    [SerializeField] InputActionReference activateWeaponWheel;
    Weapon gun;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        activateWeaponWheel.action.performed += OnPressed;
        activateWeaponWheel.action.canceled += OnReleased;
        gun = GetComponent<Weapon>();
    }

    public void OnPressed(InputAction.CallbackContext context)
    {
        // handMenu.enabled = true;
        this.gameObject.SetActive(true);
    }

    public void OnReleased(InputAction.CallbackContext context)
    {
        // handMenu.enabled = false;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
