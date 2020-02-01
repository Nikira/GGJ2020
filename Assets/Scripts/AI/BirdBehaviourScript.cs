using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviourScript : MonoBehaviour
{
    public BirdControllerScript parentController; // Initialized by controller

    public bool isFlying = false;

    public void OnDestroy()
    {
        parentController.birds.Remove(this.gameObject);
    }

    public void EmitSound(AudioClip _sounds, AudioSource _birdSource)
    {
        _birdSource.PlayOneShot(_sounds);
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
        if (isFlying == true)
        {
            AudioClip clipToPlay = parentController.audioThump[(int)Random.Range (0, parentController.audioThump.Length -1)];
            Debug.Log(clipToPlay);
            EmitSound(clipToPlay, GetComponent<AudioSource>());
            isFlying = false;
        }
    }
}
