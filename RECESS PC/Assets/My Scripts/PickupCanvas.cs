using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupCanvas : MonoBehaviour
{
    [SerializeField] public Canvas pickupCanvas;
    float displayTime = 1.3f;
    // Start is called before the first frame update
    public void Start()
    {
        pickupCanvas.enabled = false;
    }

    public void DisplayCanvas()
    {
        StartCoroutine(ShowPickedupText());
    }

    IEnumerator ShowPickedupText()
    {
        pickupCanvas.enabled = true;
        yield return new WaitForSeconds(displayTime);
        pickupCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
