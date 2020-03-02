
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
    private int countLife = 3;
    private SpriteRenderer lifeBox;
    private Text lifeText;
    private Text scoreText;
    private GameObject player;
    private int correctAns;
    private Text levelText;
    private SpriteRenderer levelBox;
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

        var mazePage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("maze")).First();
        lifeBox = mazePage.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "LoseLife").First();
        lifeBox.enabled = false;

        lifeText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "LifeText").First();
        lifeText.text = "Life: " + countLife;

        scoreText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText").First();

        levelText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "LevelText").First();
        levelBox = mazePage.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "OpenLevel").First();
        minions = GameObject.FindGameObjectsWithTag("Minion");
        player = GameObject.FindGameObjectWithTag("Player");
        correctAns = 0;
        levelText.enabled = false;
        levelBox.enabled = false;
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
            FindObjectOfType<SoundManager>().Play("CoinCollection");
            Destroy(other.gameObject);
           ScoreUpdate(); 
        }
        if (other.gameObject.tag == "Minion")
        {
            LifeUpdate();

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
        scoreText.text = "Coin: " + score;

    }

    public void LifeUpdate()
    {
        countLife -= 1;
        lifeText.text = "Life: " + countLife;
        if (countLife == 0)
        {
            OnPlayerDied(score); //event sent to GameManager
        }
        else
        {
            lifeBox.enabled = true;
            PauseGame();
            Invoke("ResumeGame", 0.5f);
        }
    }

    public void ResumeGame()
    {
        foreach (GameObject minion in minions)
        {

            minion.GetComponent<MinionController>().isPause = false;

        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = false;
        lifeBox.enabled = false;
    }

    void PauseGame()
    {
        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().isPause = true;
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = true;
    }

    public void openNewLevel()
    {
        levelText.text = "Level 3 is opened"; 
        levelText.enabled = true;
        levelBox.enabled = true;
        Invoke("hideLevelBox", 2f);
    }

    public void countCorrectAns()
    {
        correctAns += 1;
        if(correctAns == 3)
        {
            openNewLevel();
        }
    }

    public void hideLevelBox()
    {
        Debug.Log("aha");
        levelText.enabled = false;
        levelBox.enabled = false;
    }
}
