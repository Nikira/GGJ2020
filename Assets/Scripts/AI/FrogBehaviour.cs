using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour, IBlobInteractible
{
    Rigidbody2D FrogBody;

    public GameObject pickUp;
    public bool pickedUp = false;

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
