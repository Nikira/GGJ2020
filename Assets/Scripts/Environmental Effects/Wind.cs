using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    public enum WindDirection { north, northeast, east, southeast, south, southwest, west, northwest }
    public WindDirection selectedDirection = WindDirection.north;
    Vector2 direction;
    public float windForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        WindBlow();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D otherBody))
        {
            otherBody.AddForce(direction * windForce * 2, ForceMode2D.Force);
        }
    }

    public void WindBlow()
    {
        switch(selectedDirection)
        {
            case WindDirection.north:
                direction.x = 0;
                direction.y = 1;
                break;
            case WindDirection.northeast:
                direction.x = 1;
                direction.y = 1;
                break;
            case WindDirection.east:
                direction.x = 1;
                direction.y = 0;
                break;
            case WindDirection.southeast:
                direction.x = 1;
                direction.y = -1;
                break;
            case WindDirection.south:
                direction.x = 0;
                direction.y = -1;
                break;
            case WindDirection.southwest:
                direction.x = -1;
                direction.y =-1;
                break;
            case WindDirection.west:
                direction.x = -1;
                direction.y = 0;
                break;
            case WindDirection.northwest:
                direction.x = -1;
                direction.y = 1;
                break;
        }
    }
}
