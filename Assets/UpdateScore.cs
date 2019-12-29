using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    public TextMeshPro txt;
    private void Update()
    {
        Debug.Log(txt.text);
    }
}
