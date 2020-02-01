using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractible : MonoBehaviour, IBlobInteractible
{
    public bool picked = false;
    public BlobController parent;

    public Vector3 offsetPos = Vector3.zero;
    public Vector3 offsetAngle = Vector3.zero;

    public void OnAction(BlobController blob)
    {
        Debug.Log("Test");
    }

    public void OnCollideWithBlob(BlobController other)
    {
        if (picked) return;
        other.ParentObject(gameObject);
        parent = other;
        picked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (picked && parent != null)
        {
            transform.localPosition = offsetPos;
            transform.LookAt(parent.root.transform);
        }
    }
}
