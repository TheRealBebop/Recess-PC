using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{

    [SerializeField] AudioClip[] zombieSounds;
    [SerializeField] AudioSource source;
    ZombombHealth zombombHealth;
    EnemyHealth enemyHealth;
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
        if(!zombombHealth.IsDead() || !enemyHealth.IsDead())
        {
            source.PlayOneShot(clip);
        }
    }
}
