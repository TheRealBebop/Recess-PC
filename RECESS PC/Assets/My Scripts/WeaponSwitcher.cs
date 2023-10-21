using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Security.Cryptography;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeapon;
    [SerializeField] public int numberOfWeapons = 1;
    //[SerializeField] GameObject[] weapons;

    private void Start()
    {
        SetWeaponActive();
    }

    private void Update()
    {
        int previousWeapon = currentWeapon;
        KeyInput();
        ScrollWheel();

        if(previousWeapon != currentWeapon)
        {
            SetWeaponActive();
        }
    }

    private void KeyInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && numberOfWeapons >= 1)
        {
            currentWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && numberOfWeapons > 1)
        {
            currentWeapon = 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3) && numberOfWeapons > 2)
        {
            currentWeapon = 2;
        }
    }

    private void ScrollWheel()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(currentWeapon > numberOfWeapons - 1)
            {
                currentWeapon = 0;
            }
            else
            {
                if(numberOfWeapons > 1)
                {
                    currentWeapon++;
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeapon <= 0 && numberOfWeapons > 1)
            {
                currentWeapon = numberOfWeapons - 1;
            }
            else if (currentWeapon > 0 && numberOfWeapons > 1)
            {
                currentWeapon--;
            }
        }
    }

    private void SetWeaponActive()
    {
        int weaponIndex = 0;

        foreach (Transform weapon in transform)
        {
            if(weaponIndex == currentWeapon && weapon.GetComponent<WeaponIdentifier>().identifiedAsAWeapon == true ||
               weaponIndex == currentWeapon && weapon.GetComponent<WeaponIdentifier>().equippedByDef == true)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }

    /*
    [SerializeField] GameObject[] weapons;
    //[SerializeField] InputActionReference switchKey;
    [SerializeField] int currentWeapon = 0;
    //[SerializeField] TextMeshProUGUI WeaponWheelDisplayCurrentWeaponName;

    public void Start()
    {
        GameObject.Find("Glock").SetActive(true);
        //switchKey.action.performed += changeGun;
    }

    public void Update()
    {
        //DisplayWeaponName();
    }

    public void DisplayWeaponName()
    {
        if (currentWeapon == 0)
        {
           // WeaponWheelDisplayCurrentWeaponName.text = "Pistol";
        }
        else if (currentWeapon == 1)
        {
            //WeaponWheelDisplayCurrentWeaponName.text = "Shotgun";
        }
        else if (currentWeapon == 2)
        {
            //WeaponWheelDisplayCurrentWeaponName.text = "Grenade Launcher";
        }
    }

    public void changeGun(InputAction.CallbackContext context)
    {
        if (currentWeapon == weapons.Length - 1 || currentWeapon == 1 && weapons[2].GetComponent<GrenadeLauncher>().pickedUp == false)
        {
            if (weapons[0].GetComponent<Weapon>().equippedByDefault == true && weapons[0].GetComponent<Weapon>().pickedUp == false)
            {
                currentWeapon = 0;
                foreach (GameObject x in weapons)
                {
                    if (x != weapons[currentWeapon])
                    {
                        x.SetActive(false);
                    }
                }
                weapons[currentWeapon].SetActive(true);
            }
        }
        else if (currentWeapon == 0)
        {
            if (weapons[1].GetComponent<Weapon>().pickedUp == true)
            {
                currentWeapon = 1;
                foreach (GameObject x in weapons)
                {
                    if (x != weapons[currentWeapon])
                    {
                        x.SetActive(false);
                    }
                }
                weapons[currentWeapon].SetActive(true);
            }
        }
        else if (currentWeapon == 1)
        {
            if (weapons[2].GetComponent<GrenadeLauncher>().pickedUp == true)
            {
                currentWeapon = 2;
                foreach (GameObject x in weapons)
                {
                    if (x != weapons[currentWeapon])
                    {
                        x.SetActive(false);
                    }
                }
                weapons[currentWeapon].SetActive(true);
            }
        }
    }
    */
}