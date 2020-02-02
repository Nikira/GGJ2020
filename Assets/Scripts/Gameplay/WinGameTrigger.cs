using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameTrigger : MonoBehaviour
{
    public static string winner = "";
    bool entered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var blob = collision.GetComponent<BlobController>();
        if (blob != null)
        {
            Initiate.Fade("EndScene", Color.black, 1f);
            entered = true;
            winner = blob.name;
        }
    }
}
