using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAudioController : MonoBehaviour
{
private AudioSource spikeSource;

    // Start is called before the first frame update
    void Start()
    {
        spikeSource = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !spikeSource.isPlaying)
        {
            spikeSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
