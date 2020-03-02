﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinionController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float speed;
    private Vector2 moveInput;
    public bool isPause = false;
    float delay = 0;

    public GameObject fireball;
    public bool stopFiring;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveInput = new Vector2(1, 1);

        InvokeRepeating("Fire", 0.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Freeze minions
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isPause)
            {
                isPause = false;

                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }

            else
            {
                isPause = true;

                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
            }
        }
    }
    private void FixedUpdate()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }

        if (moveInput.x == 1)  //move left
        {
            this.transform.localScale = new Vector3(Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (moveInput.x == -1)  //move right
        {
            this.transform.localScale = new Vector3(-Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        Vector2 moveVelocity = moveInput.normalized * speed;
        if (isPause)
        {
            rb.MovePosition(rb.position);
        }
        else
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
        
    }
    private void Fire()
    {
        if (!isPause && !stopFiring)
        {
            Debug.Log(stopFiring);
            GameObject fb = Instantiate(fireball, transform.position, Quaternion.identity) as GameObject;
        }
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
