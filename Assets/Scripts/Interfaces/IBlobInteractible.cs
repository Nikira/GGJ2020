using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlobInteractible
{
    void OnCollideWithBlob(BlobController blob);

    void OnAction(BlobController blob);

    void OnHoldAction(BlobController blob);
}
