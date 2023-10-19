using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDamage : MonoBehaviour
{
    [SerializeField] public Canvas impactCanvas;
    [SerializeField] float impactTime = 0.3f;

    Player target;

    // Start is called before the first frame update
    void Start()
    {
        impactCanvas.enabled = false;
        target = FindObjectOfType<Player>();
    }

    public void ShowDamageImpact()
    {
        StartCoroutine(ShowSplatter());
    }

    IEnumerator ShowSplatter()
    {
        impactCanvas.enabled = true;
        yield return new WaitForSeconds(impactTime);
        impactCanvas.enabled = false;
        if (target.isDead == true)
            impactCanvas.enabled = false;
    }
}
