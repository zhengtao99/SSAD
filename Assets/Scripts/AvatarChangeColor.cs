using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarChangeColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image renderer = GetComponent<Image>();

        renderer.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
