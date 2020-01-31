using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerScript : MonoBehaviour
{

    public GameObject birdPrefab;
    public int amountOfBirds;
    public GameObject[] birds;

    public bool flee;

    public float hoverTimer = 3f;

    [Header("Forces applied during different states")]
    public float fleeForce = 2f;
    public float returnForce = 2f;
    public float hoverForce = 5f;

    bool returning;    

    Vector2 fleeDirection;
    Vector2 returnDirection;

    // Start is called before the first frame update
    void Start()
    {
        fleeDirection = new Vector2(0, 5);
        flee = false;

        returning = false;

        birds = new GameObject[amountOfBirds];

        for(int i = 0; i < amountOfBirds; i++)
        {
            birds[i] = Instantiate(birdPrefab, new Vector2(transform.position.x + i * 0.2f, transform.position.y), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(flee)
        {
            if(!returning)
            {
                StartCoroutine(ReturnTimer());
                BirdsFlee();
            }
        }
        else
        {
            BirdsReturn();
        }

        flee = false;
    }

    public void BirdsReturn()
    {
        if (returning)
        {
            foreach (GameObject bird in birds)
            {
                Rigidbody2D birdBody = bird.GetComponent<Rigidbody2D>();
                // Return vector for regrouping
                returnDirection = new Vector2(transform.position.x - bird.transform.position.x, 0);
                returnDirection.Normalize();
                Debug.DrawLine(bird.transform.position, returnDirection);
                birdBody.AddForce(returnDirection * returnForce + (Vector2.up * hoverForce));
            }
        }
    }

    public void BirdsFlee()
    {
        foreach (GameObject bird in birds)
        {
            Rigidbody2D birdBody = bird.GetComponent<Rigidbody2D>();
            // Apply random physics force
            fleeDirection.x = Random.Range(-10, 10);

            birdBody.AddForce(fleeDirection * fleeForce, ForceMode2D.Impulse);
        }
    }



    IEnumerator ReturnTimer()
    {
        Debug.Log("Returning start");
        returning = true;
        yield return new WaitForSeconds(hoverTimer);
        returning = false;
        Debug.Log("Returning stop");
    }
}
