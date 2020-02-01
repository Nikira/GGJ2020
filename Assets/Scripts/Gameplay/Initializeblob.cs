using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializeblob : MonoBehaviour
{
    public int blobs = 1;
    public BlobController controller;
    public BlobController controller2;

    // Start is called before the first frame update
    void Start()
    {
        for (var i=0; i<blobs; i++)
        {
            controller.CreateBlob(false);
            controller2.CreateBlob(false);
        }
        controller.CalcJoints();
        controller2.CalcJoints();
    }

    void Update()
    {
        
    }

}
