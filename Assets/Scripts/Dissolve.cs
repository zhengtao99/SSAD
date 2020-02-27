using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Dissolve : MonoBehaviour
{
    Material material;

    bool isDissolving = false;
    bool dissolved = false;
    float fade = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpriteRenderer>() !=null)
            material = GetComponent<SpriteRenderer>().material;
        if (GetComponent<UnityEngine.UI.Image>() != null)
            material = GetComponent<UnityEngine.UI.Image>().material;
        if (GetComponent<Text>() != null)
        {
            material = GetComponent<Text>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dissolved)
        {
            isDissolving = true;
            dissolved = true;
        }

        if (isDissolving)
        {
            fade += Time.deltaTime;


            if (fade >= 1f)
            {
                fade = 1f;
                isDissolving = false;
            }
            material.SetFloat("_Fade", fade);
        }
    }
}
