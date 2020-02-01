using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviourScript : MonoBehaviour
{
    public BirdControllerScript parentController; // Initialized by controller
    public const string LAYER_NAME = "Foreground";
    public int sortingOrder = 0;
    private SpriteRenderer sprite;

    public void OnDestroy()
    {
        parentController.birds.Remove(this.gameObject);
    }

    public void SetLayer()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = sortingOrder;
        sprite.sortingLayerName = LAYER_NAME;
    }
}
