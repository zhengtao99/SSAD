using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float speed;
    private Vector2 moveInput;
    float delay = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveInput = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (delay > 0)
        { 
            delay -= Time.deltaTime;
        }
        if (moveInput.x == 1)
        {
            this.transform.localScale = new Vector3(0.5f, 0.51f, 0.5f);
        }
        else if (moveInput.x == -1)
        {
            this.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        Vector2 moveVelocity = moveInput.normalized * speed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (delay > 0)
                return;
            delay = 0.00001f;

            float absX = Mathf.Abs(this.transform.position.x - other.transform.position.x);
            float absY = Mathf.Abs(this.transform.position.y - other.transform.position.y);

            if (absY > absX)
            {
                if (moveInput.y == 1)
                {
                    moveInput.y = -1;
                }
                else
                {
                    moveInput.y = 1;
                }
            }
            else if (absY < absX)
            {
                if (moveInput.x == 1)
                {
                    moveInput.x = -1;
                }
                else
                {
                    moveInput.x = 1;
                }
            }
            else
            {
                moveInput.x = 0;
                moveInput.y = 0;
            }
           
        }
    }

}
