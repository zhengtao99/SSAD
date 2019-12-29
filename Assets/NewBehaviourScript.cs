
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    public float speed;

    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            this.transform.localScale = new Vector3(0.5f, 0.51f, 0.5f);
        }
        else if(Input.GetAxisRaw("Horizontal") == -1)
        {
            this.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        moveVelocity = moveInput.normalized * speed;
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
