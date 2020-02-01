using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobSplitInteractible : MonoBehaviour, IBlobInteractible
{
    public float timer = 0f;

    public void OnAction(BlobController blob)
    {
    }

    public void OnCollideWithBlob(BlobController other)
    {
        if (!enabled) return;
        var blob = GetComponent<BlobController>();

        if (blob.root != other.root && timer > 3f)
        {
            enabled = false;
            
            foreach (var split in blob.root.GetComponentsInChildren<BlobSplitInteractible>())
            {
                split.enabled = false;
            }
            Debug.Log("JOIN");
        }
    }

    public void OnHoldAction(BlobController blob)
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
}
