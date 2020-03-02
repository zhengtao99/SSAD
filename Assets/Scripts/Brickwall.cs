using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brickwall : MonoBehaviour
{
    public GameObject flame;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Fireball")
        {
            Destroy(other.gameObject);
            GameObject fl = Instantiate(flame, other.transform.position, Quaternion.identity) as GameObject;
            Destroy(fl, 0.5f);
        }
    }
}
