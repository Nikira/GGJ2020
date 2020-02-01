using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float ExplosionForce = 5f;
    public float ExplosionRadius = 10f;

    public bool exploding = false;

    public void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);
        foreach(Collider2D collider in colliders)
        {
            //Debug.Log(collider.name);
            if (collider.TryGetComponent(out Rigidbody2D otherBody))
            {
                Debug.Log(otherBody.name);
                Vector2 direction = collider.transform.position - transform.position;
                otherBody.AddForce((direction.normalized * ExplosionForce) + Vector2.up * ExplosionForce * 2, ForceMode2D.Impulse);
            }
        }
    }

    public void Update()
    {
        if(exploding)
        {
            Explode();
            exploding = false;
        }
    }
}
