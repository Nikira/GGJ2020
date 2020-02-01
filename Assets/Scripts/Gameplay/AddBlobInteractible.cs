using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBlobInteractible : MonoBehaviour, IBlobInteractible
{
    [HideInInspector]
    public bool picked = false;

    public void OnAction(BlobController blob)
    {

    }

    public void OnCollideWithBlob(BlobController other)
    {
        if (picked) return;
        other.CreateBlob();

        picked = true;
        Destroy(gameObject);
    }

    public void OnHoldAction(BlobController blob)
    {
        
    }
}
