using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debris : MonoBehaviour
{

    [SerializeField] SceneSwitcher changer;

    private void Start()
    {
        changer.enabled = false;
    }

    public void Destruction()
    {
        changer.enabled = true;
        Destroy(gameObject);
    }
}
