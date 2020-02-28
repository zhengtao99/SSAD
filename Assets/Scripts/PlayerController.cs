
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
    //public Sprite Player_Dead;
    //MinionController mc;
    private GameObject[] minions;

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
        Debug.Log("AAA");

        minions = GameObject.FindGameObjectsWithTag("Minion");
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

            foreach (GameObject minion in minions)
            {

                minion.GetComponent<MinionController>().stopFiring = true;

            }
            OnPlayerDied(score); //event sent to GameManager    
            
        }
        if (other.gameObject.tag == "Fireball")
        {
            //Animator animator = this.gameObject.GetComponent<Animator>();
            //animator.runtimeAnimatorController = Resources.Load("New Sprite") as RuntimeAnimatorController;
            //animator.enabled = false;
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = Player_Dead;

            Destroy(other.gameObject);

            StartCoroutine(hit());
        }
    }
    IEnumerator hit()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 8; i++)
        {
            renderer.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    void ScoreUpdate()
    {
        score += 10;
        var mazePage = GameObject.FindGameObjectsWithTag("Page").Where(z=>z.name.ToLower().Contains("maze")).First();

        var textBox = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText").First();

        textBox.text = "Coin: " + score;

    }
}
