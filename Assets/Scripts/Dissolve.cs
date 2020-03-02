using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Dissolve : MonoBehaviour
{
    Material material, textMaterial;

    bool isDissolving = false;
    bool dissolved;
    float visible;
    // Start is called before the first frame update
    void OnEnable()
    {
        //Finds all the material of the object to manipulate with animation
        dissolved = false;
        visible = 0f;
        if (GetComponent<SpriteRenderer>() != null)
            material = GetComponent<SpriteRenderer>().material;
        else if (GetComponent<UnityEngine.UI.Image>() != null) 
        { 
            if (GetComponentInChildren<Text>() != null)
            {
                textMaterial = GetComponentInChildren<Text>().material;
            }
            material = GetComponent<UnityEngine.UI.Image>().material;
        }
        else if (GetComponent<Text>() != null)
        {
            material = GetComponent<Text>().material;
        }
    }

    private void OnDisable()
    {
        dissolved = false;
        visible = 0f;
    }

    // Update is called once per frame
    void Update() 
        {
        //Animate till fade >= 1f (Means object is fully visible)
        if (!dissolved)
        {
            isDissolving = true;
            dissolved = true;
        }

        if (isDissolving)
        {
            visible += Time.deltaTime/2; // Slows down the animation
            if (visible >= 1f)
             {
                visible = 1f;
                isDissolving = false;
            }
            material.SetFloat("_Fade", visible);
            if (textMaterial != null)
            {
                textMaterial.SetFloat("_Fade", visible);
            }
        }
    }
}
