using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public AudioClip spikeSound; // Initialized in inspector
    AudioSource source;

    // Split blob if touched
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BlobController blobController))
        {
            Debug.Log("collided with a spike");
            blobController.Bisect();

        }
    }
}
