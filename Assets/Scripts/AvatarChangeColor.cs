using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The controller class that contains a method that changes the player's avatar color.
/// </summary>
public class AvatarChangeColor : MonoBehaviour
{
    // Start is called before the first frame update
    /// <summary>
    /// This method that is called before the first frame update to randomly set the color of the avatar.
    /// </summary>
    void Start()
    {
        Image renderer = GetComponent<Image>();

        renderer.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

}
