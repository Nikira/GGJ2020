﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviourScript : MonoBehaviour
{
    public BirdControllerScript parentController; // Initialized by controller

    public void OnDestroy()
    {
        parentController.birds.Remove(this.gameObject);
    }

    public void EmitSound(AudioClip _tweets, AudioSource _birdSource)
    {
        _birdSource.PlayOneShot(_tweets);
    }
}
