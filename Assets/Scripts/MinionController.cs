using System.Collections;
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

    public GameObject attackEffect;
    public GameObject fireball;
    public GameObject freeze;
    public GameObject alert;
    public bool stopFiring = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveInput = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Minion fire FOR TESTING
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(Fire());
        }

        //Freeze minions FOR TESTING
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(FreezeMinions());
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

    public void IFire()
    {
        StartCoroutine(Fire());
    }

    public void IFreezeMinions()
    {
        StartCoroutine(FreezeMinions());
    }

    public void ISlowDownMinion()
    {
        StartCoroutine(SlowDownMinion());
    }

    public void ISpeedUpMinion()
    {
        StartCoroutine(SpeedUpMinion());
    }

    IEnumerator SlowDownMinion()
    {
        speed = 1;
        yield return new WaitForSeconds(10.0f);
        speed = 3;
    }

    IEnumerator SpeedUpMinion()
    {
        speed = 5;
        yield return new WaitForSeconds(10.0f);
        speed = 3;
    }
    IEnumerator FreezeMinions()
    {
        FindObjectOfType<SoundManager>().Play("Freeze");

        Animator anim = this.gameObject.GetComponent<Animator>();

        isPause = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        anim.enabled = false;
        Instantiate(freeze, transform.position, Quaternion.identity, transform);

        yield return new WaitForSeconds(5.0f);

        isPause = false;
        renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        anim.enabled = true;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    IEnumerator Fire()
    {
        Vector3 offset = new Vector3(0.4f, 1, 0);
        Instantiate(alert, transform.position + offset, Quaternion.identity, transform);

        yield return new WaitForSeconds(3f);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(activateEffect2());

        for (int i = 0; i < 4; i++)
        {
            if (!isPause && !stopFiring)
            {
                FindObjectOfType<SoundManager>().Play("Fireball");
                GameObject fb = Instantiate(fireball, transform.position, Quaternion.identity) as GameObject;
                yield return new WaitForSeconds(3.0f);
            }
        }

        //yield return null;
    }

    IEnumerator activateEffect2()
    {
        Vector3 offset = new Vector3(0, 0.2f, 0);
        Instantiate(attackEffect, transform.position + offset, Quaternion.identity, transform);
        yield return new WaitForSeconds(9f);

        GameObject[] aE = GameObject.FindGameObjectsWithTag("attackEffect");
        foreach (GameObject a in aE)
            GameObject.Destroy(a);
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
