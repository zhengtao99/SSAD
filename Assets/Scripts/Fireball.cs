using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This entity class models a Fire ball game object, the class contains the attributes and methods to model the behaviour of this object.
/// </summary>
public class Fireball : MonoBehaviour
{

    /// <summary>
    /// A variable that contains the fire ball speed attribute.
    /// </summary>
    public float fireballSpeed;

    /// <summary>
    /// A variable that contains the fire ball rigidbody2D attribute.
    /// </summary>
    Rigidbody2D rb;

    /// <summary>
    /// This method is called once before the first frame update, it will initialize the fire ball's rigidbody2d attribute
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// This method is called once per frame and it will move the fire ball to the left.
    /// </summary>
    void Update()
    {
        transform.Translate(Vector3.left * fireballSpeed * Time.deltaTime);
    }
}
