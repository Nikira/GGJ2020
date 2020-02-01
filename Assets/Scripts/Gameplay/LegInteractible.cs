using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegInteractible : MonoBehaviour, IBlobInteractible
{
    public bool picked = false;
    public BlobController parent;
    public Transform chaseTransform;

    public float force = 10f;

    public Vector3 offsetPos = Vector3.zero;
    public Vector3 offsetAngle = Vector3.zero;

    public void OnAction(BlobController blob)
    {
        if (!IsGrounded) return;
        var vec = -(transform.localRotation.eulerAngles.normalized).normalized * force;
        blob.GetComponent<Rigidbody2D>().AddForce(vec);
        Debug.Log($"Applying force {vec.ToString()}");
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
        var collision = GetComponent<BoxCollider2D>();
        if (collision != null)
        {
            Destroy(collision);
        }
        parent = other;
        picked = true;
    }

    public void OnHoldAction(BlobController blob)
    {

    }

    bool IsGrounded
    {
        get
        {
            var collider = GetComponent<Collider2D>();
            return Physics2D.Raycast(transform.position - new Vector3(0f, collider.bounds.extents.y, 0f), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        }
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
            chaseTransform.LookAt(parent.root.transform);

            var diff = (transform.position - parent.root.transform.position).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            chaseTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rot_z - 90) + offsetAngle);
        }
    }
}
