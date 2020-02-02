using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour, IBlobInteractible
{
    Rigidbody2D FrogBody;

    public GameObject pickUp;
    public bool pickedUp = false;

    private AudioSource frogSource;
    public AudioClip[] croakAudio;
    public AudioClip[] hopAudio;

    // Start is called before the first frame update
    void Start()
    {
        FrogBody = GetComponent<Rigidbody2D>();
        frogSource = GetComponent<AudioSource>();
        StartCoroutine(FrogHopInterval());
        StartCoroutine(FrogCroakInterval());
    }

    public void FrogHop()
    {
        FrogBody.AddForce(new Vector2(Random.Range(-2, 2), Random.Range(0, 5)), ForceMode2D.Impulse);
        frogSource.PlayOneShot(hopAudio[(int)Random.Range (0, hopAudio.Length -1)]);
    }

    IEnumerator FrogHopInterval()
    {
        FrogHop();
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        StartCoroutine(FrogHopInterval());
    }

    IEnumerator FrogCroakInterval()
    {
        yield return new WaitForSeconds(Random.Range(5f, 12f));
        frogSource.PlayOneShot(croakAudio[(int)Random.Range (0, croakAudio.Length -1)]);
        StartCoroutine(FrogCroakInterval());
    }

    public void OnCollideWithBlob(BlobController blob)
    {
        if (pickedUp) return;
        var pickup = Instantiate(pickUp, transform);
        pickup.GetComponent<LegInteractible>().OnCollideWithBlob(blob);

        Destroy(gameObject);
        pickedUp = true;
    }

    public void OnAction(BlobController blob)
    {

    }

    public void OnHoldAction(BlobController blob)
    {

    }
}
