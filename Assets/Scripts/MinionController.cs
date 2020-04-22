using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This controller class contains all the methods manipulate the minions. The controller will be attached onto the minions in order to control them.
/// </summary>
public class MinionController : MonoBehaviour
{
    /// <summary>
    /// A variable to hold the rigidbody2D component on the minion.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// A variable hold and manipulate the movement speed of minions.
    /// </summary>
    public float speed;

    /// <summary>
    /// A variable to hold and control the direction the minion is facing.
    /// </summary>
    private Vector2 moveInput;

    /// <summary>
    /// A variable to hold a boolean value to check if the game is paused, this is used to restrict movements of minions during a game pause.
    /// </summary>
    public bool isPause = false;

    /// <summary>
    /// A variable to hold the delay to regulate the movement calculation interval for the minions. To ensure a steady flow of movements shown the GUI.
    /// </summary>
    float delay = 0;

    /// <summary>
    /// A variable to hold the attack effect game object.
    /// </summary>
    public GameObject attackEffect;

    /// <summary>
    /// A variable to hold the fire ball game object.
    /// </summary>
    public GameObject fireball;

    /// <summary>
    /// A variable to hold the freeze effect game object.
    /// </summary>
    public GameObject freeze;

    /// <summary>
    /// A variable to hold the alert effect game object.
    /// </summary>
    public GameObject alert;

    /// <summary>
    /// A boolean variable to control the firing, the firing will stop when it is true.
    /// </summary>
    public bool stopFiring = false;

    /// <summary>
    /// This method is called at the start of the game, to get the minion's rigidbody and set the minion's movement.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveInput = new Vector2(1, 1);
    }

    /// <summary>
    /// This method is called once per frame to move the minions throughout the entire game.
    /// </summary>
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

    /// <summary>
    /// This method is used to generate fire balls shooting out of minions.
    /// </summary>
    public void IFire()
    {
        StartCoroutine(Fire());
    }

    /// <summary>
    /// This method is used to call FreezeMinion() method to freeze the minions' movement.
    /// </summary>
    public void IFreezeMinions()
    {
        StartCoroutine(FreezeMinions());
    }

    /// <summary>
    /// This method is used to call SlowDownMinion() method to slow down the minions' movement.
    /// </summary>
    public void ISlowDownMinion()
    {
        StartCoroutine(SlowDownMinion());
    }

    /// <summary>
    /// This method is used to call SpeedUpMinion() method to speed up the minions' movement.
    /// </summary>
    public void ISpeedUpMinion()
    {
        StartCoroutine(SpeedUpMinion());
    }

    /// <summary>
    /// This method performs the actual slowing down of minions for 10 seconds.
    /// </summary>
    IEnumerator SlowDownMinion()
    {
        speed = 0.6f;
        yield return new WaitForSeconds(10.0f);
        speed = 1.5f;
    }

    /// <summary>
    /// This method performs the actual speeding up of minions for 10 seconds.
    /// </summary>
    IEnumerator SpeedUpMinion()
    {
        speed = 3f;
        yield return new WaitForSeconds(10.0f);
        speed = 1.5f;
    }

    /// <summary>
    /// This method performs the actual freezing of minions for 5 seconds then unfreeze them.
    /// </summary>
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

    /// <summary>
    /// This method holds the logic to create the fireball attacks.
    /// </summary>
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

                GameObject fb1 = Instantiate(fireball, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                GameObject fb2 = Instantiate(fireball, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
                GameObject fb3 = Instantiate(fireball, transform.position, Quaternion.Euler(0, 0, 180)) as GameObject;
                GameObject fb4 = Instantiate(fireball, transform.position, Quaternion.Euler(0, 0, 270)) as GameObject;
                
                yield return new WaitForSeconds(3.0f);
            }
        }

        //yield return null;
    }

    /// <summary>
    /// This method performs effect generated from the fireball attack.
    /// </summary>
    IEnumerator activateEffect2()
    {
        Vector3 offset = new Vector3(0, 0.2f, 0);
        Instantiate(attackEffect, transform.position + offset, Quaternion.identity, transform);
        yield return new WaitForSeconds(9f);

        GameObject[] aE = GameObject.FindGameObjectsWithTag("attackEffect");
        foreach (GameObject a in aE)
            GameObject.Destroy(a);
    }

    /// <summary>
    /// This method is responsible for manipulating minions' movement in cases where the minion touches a wall, to prevent the minion from moving out of the maze.
    /// </summary>
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
                if (absY > 0)
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
            }

            else if (absY < absX)
            {
                if (absX > 0)
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
                    moveInput.x = -moveInput.x;
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
