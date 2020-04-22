using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// This entity class is used to model dissolving materials, the class contains attributes and behaviour for a given game object to dissolve.
/// </summary>
public class Dissolve : MonoBehaviour
{
    /// <summary>
    /// A variable contain the initial object's material and the text material.
    /// </summary>
    Material material, textMaterial;

    /// <summary>
    /// A variable that contains a boolean to check if the object is dissolving.
    /// </summary>
    bool isDissolving = false;

    /// <summary>
    /// A variable that contains a boolean to check if the object has dissolved
    /// </summary>
    bool dissolved;

    /// <summary>
    /// A variable that contains the visibility of the object.
    /// </summary>
    float visible;


    /// <summary>
    /// Tis method will be called when the dissolve material is enabled and the object will starts to dissolve.
    /// </summary>
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

    /// <summary>
    /// This method will be called when the dissolve material is disabled and the object recover to it's original state.
    /// </summary>
    private void OnDisable()
    {
        dissolved = false;
        visible = 0f;
    }

    /// <summary>
    /// This method is called once per frame and it will constantly check if the object is full dissolved. If so, the object will be disabled.
    /// </summary>
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
