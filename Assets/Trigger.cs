using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    public int Score = 0;

    public Text txt;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        Score += 10;
        txt.text = "Score: " + Score;
    }
}
