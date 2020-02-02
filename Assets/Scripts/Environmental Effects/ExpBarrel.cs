using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosion))]
public class ExpBarrel : MonoBehaviour
{
    Explosion exploderReference;
    public AudioClip explosionSound; // Initialized in inspector
    AudioSource source;
    public bool destroyOnCollision = true;

    // Start is called before the first frame update
    void Start()
    {
        exploderReference = GetComponent<Explosion>();
        source = GetComponent<AudioSource>();
    }

    // Explode if the blob touches it
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out BlobController blobController))
        {
            blobController.Bisect();
            source.PlayOneShot(explosionSound);
            exploderReference.Explode();
            if (destroyOnCollision == true)
            {
                StartCoroutine(DestroyOnDelay());
            }
        }
    }

    IEnumerator DestroyOnDelay()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
