using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviourScript : MonoBehaviour, IBlobInteractible
{
    public BirdControllerScript parentController; // Initialized by controller

    public GameObject pickUp;
    public bool pickedUp = false;

    public bool isFlying = false;

    public Sprite restSprite;
    public Sprite flapSprite;

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

    public void OnCollideWithBlob(BlobController blob)
    {
        if (pickedUp) return;
        var pickup = Instantiate(pickUp, transform);
        pickup.GetComponent<WingInteractible>().OnCollideWithBlob(blob);

        Destroy(gameObject);
        pickedUp = true;
    }

    public void ChangeSprite(Sprite _inputSprite)
    {
        GetComponent<SpriteRenderer>().sprite = _inputSprite;
    }

    public void OnAction(BlobController blob)
    {

    }

    public void OnHoldAction(BlobController blob)
    {

    }
}
