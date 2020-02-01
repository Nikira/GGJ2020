using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour
{
    Rigidbody2D FrogBody;

    // Start is called before the first frame update
    void Start()
    {
        FrogBody = GetComponent<Rigidbody2D>();
        StartCoroutine(FrogHopInterval());
    }

    public void FrogHop()
    {
        FrogBody.AddForce(new Vector2(Random.Range(-2, 2), Random.Range(0, 5)), ForceMode2D.Impulse);
    }

    IEnumerator FrogHopInterval()
    {
        FrogHop();
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        StartCoroutine(FrogHopInterval());
    }
}
