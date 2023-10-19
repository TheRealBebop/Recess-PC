using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{

    [SerializeField] AudioClip[] zombieSounds;
    [SerializeField] AudioSource source;
    [SerializeField] ZombombHealth zombombHealth;
    [SerializeField] EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        zombombHealth = GetComponent<ZombombHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void PlayZombieSounds()
    {
        AudioClip clip = zombieSounds[UnityEngine.Random.Range(0, zombieSounds.Length)];
        if(enemyHealth == null)
        {
            if(!zombombHealth.IsDead())
            {
                source.PlayOneShot(clip);
            }
        }
        else if (zombombHealth == null)
        {
            if (!enemyHealth.IsDead())
            {
                source.PlayOneShot(clip);
            }
        }
    }
}
