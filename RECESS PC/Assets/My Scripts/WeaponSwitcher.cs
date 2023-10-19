using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;
    [SerializeField] InputActionReference switchKey;
    [SerializeField] int currentWeapon = 0;
    [SerializeField] TextMeshProUGUI WeaponWheelDisplayCurrentWeaponName;

    public void Start()
    {
        GameObject.Find("Glock").SetActive(true);
        switchKey.action.performed += changeGun;
    }

    public void Update()
    {
        DisplayWeaponName();
    }

    public void DisplayWeaponName()
    {
        if (currentWeapon == 0)
        {
            WeaponWheelDisplayCurrentWeaponName.text = "Pistol";
        }
        else if (currentWeapon == 1)
        {
            WeaponWheelDisplayCurrentWeaponName.text = "Shotgun";
        }
        else if (currentWeapon == 2)
        {
            WeaponWheelDisplayCurrentWeaponName.text = "Grenade Launcher";
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
}