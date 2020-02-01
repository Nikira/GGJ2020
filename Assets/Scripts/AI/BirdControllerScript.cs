using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerScript : MonoBehaviour
{

    public GameObject birdPrefab;
    public int amountOfBirds;
    public GameObject[] birds;

    public enum state { resting, fleeing, hovering, returning }
    public state birdState = state.resting;


    [Header("Forces applied during different states")]
    public float fleeForce = 2f;
    public float returnForce = 2f;
    public float hoverForce = 5f;

    Vector2 fleeDirection;
    Vector2 returnDirection;

    // Start is called before the first frame update
    void Start()
    {
        fleeDirection = new Vector2(0, 5);
        birds = new GameObject[amountOfBirds];

        for(int i = 0; i < amountOfBirds; i++)
        {
            birds[i] = Instantiate(birdPrefab, new Vector2(transform.position.x + i * 0.2f, transform.position.y), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(birdState)
        {
            case state.resting:
                break;
            case state.fleeing:
                BirdsFlee();
                StartCoroutine(FlapTimer());
                birdState++;
                StartCoroutine(StateIterator());// Rotate through the other state
                break;
            case state.hovering:
                break;
            case state.returning:
                BirdsReturn();
                break;
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
            birdBody.AddForce(returnDirection * returnForce /** DistanceToController(bird.transform.position)*/);
        }
    }


    public void BirdsFlee()
    {
        foreach (GameObject bird in birds)
        {
            Rigidbody2D birdBody = bird.GetComponent<Rigidbody2D>();
            // Apply random physics force
            fleeDirection.x = Random.Range(-6, 6);

            birdBody.AddForce(fleeDirection * fleeForce, ForceMode2D.Impulse);
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
        }
    }

    public float DistanceToController(Vector2 _birdLocation)
    {
        return Vector2.Distance(transform.position, _birdLocation);
    }

    IEnumerator StateIterator()
    {
        if(birdState > state.returning)
        {
            birdState = state.resting;
        }

        yield return new WaitForSeconds(3f);
        if (birdState != state.resting)
        {
            birdState++;
            StartCoroutine(StateIterator());
        }
        Debug.Log("birdState:" + birdState);
    }

    // Apply upwards force at given interval
    IEnumerator FlapTimer()
    {
        Debug.Log("Starting flaptimer");
        yield return new WaitForSeconds(0.5f);
        if (birdState != state.resting)
        {
            BirdsFlap();
            StartCoroutine(FlapTimer());
        }
    }
}
