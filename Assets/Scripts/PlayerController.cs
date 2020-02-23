
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using UnityEngine.Networking;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public GameObject projectile;

    public delegate void PlayerDelegate(int value);
    public static event PlayerDelegate OnPlayerDied;


    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    public bool isPause = false;
    public float speed;
    int score = 0;

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
            //this.transform.localScale = new Vector3(23.74183f, 23.74183f, 1f);  //localScale
            this.transform.localScale = new Vector3(Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else if(Input.GetAxisRaw("Horizontal") == -1)
        {
            //this.transform.localScale = new Vector3(-23.74183f, 23.74183f, 1f);  //localScale
            this.transform.localScale = new Vector3(-Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        moveVelocity = moveInput.normalized * speed;


        //Press Z to fire projectile (testing)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector2 moveVelocity = new Vector2(-1, 0);

            if (moveInput.x == -1)
            {
                Debug.Log("x-1");
                GameObject pro = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
                //rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
            }

            else if (moveInput.x == 1)
            {
                Debug.Log("x1");
            }

            else if (moveInput.y == 1)
            {
                Debug.Log("y1");
            }

            else if (moveInput.y == -1)
            {
                Debug.Log("y-1");
            }
                
        }

    }
    private void FixedUpdate()
    {
        if (isPause)
        {
            rb.MovePosition(rb.position);
        }
        else
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {

            Destroy(other.gameObject);
           ScoreUpdate(); 
        }
        if (other.gameObject.tag == "Minion")
        {
            //Destroy(other.gameObject);
            //Score += 10;
            //txt.text = "Score: Hit";
            OnPlayerDied(score); //event sent to GameManager
        }
    }
    void ScoreUpdate()
    {
        score += 1;
        var mazePage = GameObject.FindGameObjectsWithTag("Page").First();
        var textBox = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText").First();

        textBox.text = "Coin: " + score;

    }
}
