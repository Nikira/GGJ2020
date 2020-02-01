using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingInteractible : MonoBehaviour, IBlobInteractible
{
    [HideInInspector]
    public bool picked = false;
    public BlobController parent;
    public Transform chaseTransform;

    public float force = 10f;

    public Vector3 offsetPos = Vector3.zero;
    public Vector3 offsetAngle = Vector3.zero;

    public void OnAction(BlobController blob)
    {
        var dir = new Vector3(Input.GetAxis("Horizontal") / 2f, 1f, 0f);
        var vec = (dir).normalized * force;
        blob.GetComponent<Rigidbody2D>().AddForce(vec);
    }

    public void OnCollideWithBlob(BlobController other)
    {
        if (picked) return;
        other.ParentObject(gameObject);
        var rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody)
        {
            Destroy(rigidBody);
        }
        parent = other;
        picked = true;
    }

    public void OnHoldAction(BlobController blob)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (picked && parent != null)
        {
            transform.localPosition = offsetPos;
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            var diff = (transform.position - parent.root.transform.position).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            chaseTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rot_z - 90) + offsetAngle);
        }
    }
}
