using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerScript : MonoBehaviour
{

    public GameObject birdPrefab;
    public int amountOfBirds;
    public List<GameObject> birds;

    public enum State { resting, fleeing, hovering, returning }
    public State birdState = State.resting;

    [Header("Forces applied during different states")]
    public float fleeForce = 1.8f;
    public float returnForce = 2f;
    public float hoverForce = 5f;

    Vector2 fleeDirection;
    Vector2 returnDirection;

    public Sprite spriteRest;
    public Sprite spriteFlap;
    public Sprite spriteFly;

    public AudioClip[] tweets;
    public AudioClip[] audioFlutter;
    public AudioClip[] audioflap;
    public AudioClip[] audioThump;

    // Start is called before the first frame update
    void Start()
    {

        fleeDirection = new Vector2(0, 5);
        birds = new List<GameObject>();

        
        for(int i = 0; i < amountOfBirds; i++)
        {
            birds.Add(Instantiate(birdPrefab, new Vector2(transform.position.x + i * 0.2f, transform.position.y), Quaternion.identity));
            birds[i].GetComponent<BirdBehaviourScript>().parentController = this;
        }

        StartCoroutine(RandomTweets());
    }

    // Update is called once per frame
    void Update()
    {
        switch(birdState)
        {
            case State.resting:
                BirdsReturn(); // I put this here to have them "walk" back after they land
                LoadSprite(spriteRest);
                break;
            case State.fleeing:
                BirdsFlee();
                StartCoroutine(FlapTimer());
                birdState++;
                StartCoroutine(StateIterator());// Rotate through the other state
                break;
            case State.hovering:
                break;
            case State.returning:
                BirdsReturn();
                break;
        }

        StateToSprite(); // Don't think this is optimal at all, comment this out if it causes any problems with framerate
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BlobController>() != null)
        {
            birdState = State.fleeing;
        }
    }

    // Apply force towards origin
    public void BirdsReturn()
    {
        foreach (GameObject bird in birds)
        {
            Rigidbody2D birdBody = bird.GetComponent<Rigidbody2D>();
            // Return vector for regrouping
            returnDirection = new Vector2(transform.position.x - bird.transform.position.x, 0);
            returnDirection.Normalize();
            Debug.DrawLine(bird.transform.position, returnDirection);
            birdBody.AddForce(returnDirection * returnForce * (DistanceToController(bird.transform.position)*0.5f));
            bird.GetComponent<BirdBehaviourScript>().ChangeSprite(spriteRest);
        }
    }


    public void BirdsFlee()
    {
        foreach (GameObject bird in birds)
        {
            Rigidbody2D birdBody = bird.GetComponent<Rigidbody2D>();
            // Apply random physics force
            fleeDirection.x = Random.Range(-6, 6);

            BirdBehaviourScript tempRef = bird.GetComponent<BirdBehaviourScript>();

            birdBody.AddForce(fleeDirection * fleeForce, ForceMode2D.Impulse);
            tempRef.EmitSound(audioFlutter[(int)Random.Range (0, audioFlutter.Length -1)], bird.GetComponent<AudioSource>());
        }
    }

    // Apply upwards force to birds
    public void BirdsFlap()
    {
        Debug.Log("Flap!");
        foreach (GameObject bird in birds)
        {
            Rigidbody2D birdBody = bird.GetComponent<Rigidbody2D>();
            birdBody.AddForce(Vector2.up * hoverForce, ForceMode2D.Impulse);
            bird.GetComponent<BirdBehaviourScript>().EmitSound(audioflap[(int)Random.Range (0, audioflap.Length -1)], bird.GetComponent<AudioSource>());
        }
    }

    public float DistanceToController(Vector2 _birdLocation)
    {
        return Vector2.Distance(transform.position, _birdLocation);
    }

    IEnumerator StateIterator()
    {
        if(birdState > State.returning)
        {
            birdState = State.resting;
        
            foreach (GameObject bird in birds)
            {
                bird.GetComponent<BirdBehaviourScript>().isFlying = true;
            }
        }

        yield return new WaitForSeconds(3f);
        if (birdState != State.resting)
        {
            birdState++;
            StartCoroutine(StateIterator());
        }
        //Debug.Log("birdState:" + birdState);


    }

    // Apply upwards force at given interval
    IEnumerator FlapTimer()
    {
        Debug.Log("Starting flaptimer");
        yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
        if (birdState != State.resting)
        {
            BirdsFlap();
            StartCoroutine(FlapTimer());
        }
    }

    IEnumerator RandomTweets()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 4));
        if (birdState == State.resting)
        GetComponent<AudioSource>().PlayOneShot(tweets[(int)Random.Range (0, tweets.Length -1)]);
        StartCoroutine(RandomTweets());
    }

    public void LoadSprite(Sprite _inputSprite)
    {
        foreach(GameObject bird in birds)
        {
            bird.GetComponent<BirdBehaviourScript>().ChangeSprite(_inputSprite);
        }
    }

    public void StateToSprite()
    {
        switch (birdState)
        {
            case State.resting:
                LoadSprite(spriteRest);
                break;
            case State.fleeing:
                break;
            case State.hovering:
                LoadSprite(spriteFlap);
                break;
            case State.returning:
                LoadSprite(spriteFly);
                break;
        }
    }
}
