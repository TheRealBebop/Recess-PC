using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    // [SerializeField] Camera FPCamera;
    [SerializeField] public bool equippedByDefault = false;
    [SerializeField] public bool pickedUp = false;
    [SerializeField] float range = 100f;
    [SerializeField] float weaponDamage = 50f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Ammo ammoScript;
    [SerializeField] AmmoType ammoType;
    public int currentAmmo;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] InputActionReference fireActionReference;
    public bool canShoot = true;
    public AudioSource gunshotSound;
    public bool triggerPressed = false;
    private void OnEnable()
    {
        canShoot = true;
    }

    private void Start()
    {
        fireActionReference.action.performed += OnFired;
        gunshotSound = GetComponent<AudioSource>();
        // fire.performed += OnFired;
        // fire.canceled += OnFired;
        // fire.Enable();
        // ammoSlot = GetComponent<Ammo>();
    }
    public void OnFired(InputAction.CallbackContext context)
    {
        triggerPressed = true; /*context.ReadValueAsButton();*/
        Debug.Log("trigger pressed");
        if (gameObject.activeSelf)
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        Debug.Log("coroutine running");
        canShoot = true;
        if (ammoScript.GetAmmoAmount(ammoType) > 0 && canShoot == true)
        {
            Debug.Log("muzzle flash");
            PlayMuzzleFlash();
            gunshotSound.Play();
            ProcessRayCast();
            ammoScript.ReduceCurrentAmmo(ammoType);
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = false;
    }
    void Update()
    {
        currentAmmo = ammoScript.GetAmmoAmount(ammoType);
        DisplayAmmo();
        //     if (triggerPressed == true && canShoot == true)
        //     {
        //         StartCoroutine(Shoot());
        //     }
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoScript.GetAmmoAmount(ammoType);
        ammoText.text = currentAmmo.ToString();
    }
    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }


    public void EquipWeapon()
    {
        pickedUp = true;
    }

    private void ProcessRayCast()
    {
        RaycastHit hit;
        // if (hit.collider.gameObject.tag == "Enemy")
        // {
        //     Debug.Log("Blood splatter");
        // }
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name + " has been hit");
            CreateHitImpact(hit);
            if (hit.transform.CompareTag("Enemy"))
            {
                EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
                if (target == null)
                {
                    return;
                }
                target.TakeDamage(weaponDamage);
            }
            else if (hit.transform.CompareTag("Zombomb"))
            {
                ZombombHealth target2 = hit.transform.GetComponent<ZombombHealth>();
                if (target2 == null)
                {
                    return;
                }
                target2.TakeDamage(weaponDamage);
            }
        }
        // if (Physics.Raycast(transform.position, transform.forward, out hit2, range))
        // {
        //     Debug.Log("DICKKKKKKK");
        //     Debug.Log(hit2.transform.name + " has been hit");
        //     CreateHitImpact(hit2);
        //     // EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
        //     ZombombHealth target2 = hit2.transform.GetComponent<ZombombHealth>();
        //     if (target2 == null)
        //     {
        //         return;
        //     }
        //     target2.TakeDamage(weaponDamage);
        // }
        else
        {
            return;
        }
    }
    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }
}


