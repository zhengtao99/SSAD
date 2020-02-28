using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float fireballSpeed;
    private int RN;

    Rigidbody2D rb;
    PlayerController target;
    Vector2 moveDirection;

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        //target = GameObject.FindObjectOfType<PlayerController>();
        //moveDirection = (target.transform.position - transform.position).normalized * fireballSpeed;
        //rb.velocity = new Vector2(moveDirection.x, moveDirection.y);        
        
        RN = Random.Range(1, 3);
    }

    void Update()
    {
        if (RN == 1)
        {
            transform.Translate(Vector3.left * fireballSpeed * Time.deltaTime);
        }

        else if (RN == 2)
        {
            transform.Translate(Vector3.right * fireballSpeed * Time.deltaTime);
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.flipX = true;   
        }
    }
}
