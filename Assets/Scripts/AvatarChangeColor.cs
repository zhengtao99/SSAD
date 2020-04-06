using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarChangeColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Color background = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = background;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
