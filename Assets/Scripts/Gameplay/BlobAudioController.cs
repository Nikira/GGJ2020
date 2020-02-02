using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAudioController : MonoBehaviour
{
    public Transform player1;

    //public Input left;
    //public Input right;
    //public Input jump;

    private AudioSource blobSource;
    public AudioClip jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        blobSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left"))
        {
            if (!blobSource.isPlaying)
                blobSource.Play();
        }

        if (Input.GetKeyUp("left"))
        {
            if (blobSource.isPlaying)
                blobSource.Stop();
        }

        if (Input.GetKeyDown("right"))
        {
            if (!blobSource.isPlaying)
                blobSource.Play();
        }

        if (Input.GetKeyUp("right"))
        {
            if (blobSource.isPlaying)
                blobSource.Stop();
        }

        if (Input.GetKeyDown("left ctrl"))
        {
            Debug.Log("Control Pressed");
            blobSource.PlayOneShot(jumpSound);
        }

        transform.position = player1.position;
    }
}
