using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour, IBlobInteractible
{
    Rigidbody2D FrogBody;

    public GameObject pickUp;
    public bool pickedUp = false;

    public Sprite spriteJumpRight;
    public Sprite spriteStillRight;
    public Sprite spriteJumpLeft;
    public Sprite spriteStillLeft;

    private AudioSource frogSource;
    public AudioClip[] croakAudio;
    public AudioClip[] hopAudio;

    SpriteRenderer renderer;

    bool flipped = false;

    // Start is called before the first frame update
    void Start()
    {
        FrogBody = GetComponent<Rigidbody2D>();
        frogSource = GetComponent<AudioSource>();
        StartCoroutine(FrogHopInterval());
        StartCoroutine(FrogCroakInterval());

        renderer = GetComponent<SpriteRenderer>();
    }

    public void FrogHop()
    {
        float horizontalForce = Random.Range(-2, 2);
        // Flip sprite depending on direction of jump
        if(horizontalForce > 0)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
        FrogBody.AddForce(new Vector2(horizontalForce, Random.Range(2, 5)), ForceMode2D.Impulse);
        frogSource.PlayOneShot(hopAudio[(int)Random.Range (0, hopAudio.Length -1)]);
    }

    IEnumerator FrogHopInterval()
    {
        FrogHop();
        StartCoroutine(HopSprite());
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        StartCoroutine(FrogHopInterval());
    }

    IEnumerator FrogCroakInterval()
    {
        yield return new WaitForSeconds(Random.Range(3f, 12f));
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


    IEnumerator HopSprite()
    {
        if(flipped)
        {
            ChangeSprite(spriteJumpRight);
        }
        else
        {
            ChangeSprite(spriteJumpLeft);
        }

        yield return new WaitForSeconds(0.8f);
        ChangeSprite(spriteStillRight);
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
