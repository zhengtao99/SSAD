using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This entity class holds the methods to model the brick wall's behaviour.
/// </summary>
public class Brickwall : MonoBehaviour
{
    /// <summary>
    /// A variable containing the fire ball game object in the maze.
    /// </summary>
    public GameObject flame;

    /// <summary>
    /// This method is used to destroy fire ball objects when the fire ball collides with this brick wall game object.
    /// </summary>
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
