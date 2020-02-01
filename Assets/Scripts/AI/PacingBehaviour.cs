using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacingBehaviour : MonoBehaviour
{
    Rigidbody2D PacingBody;
    public float paceFrequency = 1f;
    public float directionFlipInterval = 8f;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        PacingBody = GetComponent<Rigidbody2D>();
        direction = new Vector2(2f, 3f);
        StartCoroutine(PaceInterval());
        StartCoroutine(DirectionFlip());        
    }

    public void Pace()
    {
        PacingBody.AddForce(direction, ForceMode2D.Impulse);
    }
    
    IEnumerator PaceInterval()
    {
        Pace();
        yield return new WaitForSeconds(paceFrequency);
        StartCoroutine(PaceInterval());
    }

    IEnumerator DirectionFlip()
    {
        yield return new WaitForSeconds(directionFlipInterval);
        direction.x = direction.x * -1;
        StartCoroutine(DirectionFlip());
    }
}
