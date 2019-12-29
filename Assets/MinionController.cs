using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float speed;
    Vector2 moveInput;
    Vector2 previousVector;
 
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
        ChangeOnColliding();
        Vector2 moveVelocity = moveInput.normalized * speed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Wall")
        {
            float absX = Mathf.Abs(this.transform.position.x - other.transform.position.x);
            float absY = Mathf.Abs(this.transform.position.y - other.transform.position.y);

            Debug.Log("X= " + absX + ", vecX= " + moveInput.x);
            Debug.Log("Y= " + absY + ", vecY= " + moveInput.y);
            Debug.Log(other.GetInstanceID());
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
    private void  ChangeOnColliding()
    {
     
    }

}
