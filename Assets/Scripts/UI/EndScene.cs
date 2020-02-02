using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour
{
    public TextMeshProUGUI text; 

    public void Start()
    {
        text.text = $"{WinGameTrigger.winner} goes to Hollywood!";
    }

    public void EndGame()
    {
        Initiate.Fade("Main Scene", Color.black, 2f);
    }
    
}
