using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using UnityEngine.Networking;
using System.Linq;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{

    public GameObject glow;
    public GameObject deadEffect;
    private GameObject[] heartArr;
    public bool isGlowing = false;
    public float glowTime = 5.0f;
    public bool isInjured = false;
    private UnityEngine.Object explosionRef;

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

    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    public bool isPause = false;
    public float speed;
    int score = 0;
    private int direction;
    private Vector2 moveInput;
    int nsew = 0;
    private int coinsCnt;
    bool isFreeze;

    private void Start()
    {

        explosionRef = Resources.Load("PlayerExplosion");

        coinsCnt = 0;
        foreach (Transform child in GameManager.Instance.playPage.transform)
        {
            if (child.tag == "Coin")
                coinsCnt++;
        }

        rb = GetComponent<Rigidbody2D>();
        Debug.Log("rb: " + rb);
        

        //var mazePage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("maze")).First();
        var mazePage = GameManager.Instance.playPage;
        /*
        var x = mazePage.GetComponentsInChildren<SpriteRenderer>().ToList();
        foreach(var y in x)
        {
            Debug.Log(y.name);
        }
        */
        lifeBox = mazePage.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "LoseLife").First();
        lifeBox.enabled = false;

        //lifeText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "LifeText").First();
        //lifeText.text = "Life: " + countLife;

        scoreText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText").First();

        levelText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "LevelText").First();
        levelBox = mazePage.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "OpenLevel").First();
        minions = GameObject.FindGameObjectsWithTag("Minion");
        player = GameObject.FindGameObjectWithTag("Player");
        heartArr = GameObject.FindGameObjectsWithTag("Heart");
        correctAns = 0;
        levelText.enabled = false;
        levelBox.enabled = false;

        int chosenLevel = PlayerPrefs.GetInt("chosenLevel");
        Debug.Log("Level is: " + chosenLevel);

        moveInput = new Vector2(1, 1);

        direction = 4;
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (direction == 0)
        {
            //this.transform.localScale = new Vector3(23.74183f, 23.74183f, 1f);  //localScale
            this.transform.localScale = new Vector3(Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            moveVelocity = new Vector2(5, 0);
            nsew = 2;
        }
        else if (direction == 1)
        {
            //this.transform.localScale = new Vector3(-23.74183f, 23.74183f, 1f);  //localScale
            this.transform.localScale = new Vector3(-Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            moveVelocity = new Vector2(-5, 0);
            nsew = 4;
        }
        else if(direction == 2)
        {
            moveVelocity = new Vector2(0, 5);
            nsew = 1;
        }
        else if(direction == 3)
        {
            moveVelocity = new Vector2(0, -5);
            nsew = 3;
        }
        else
        {
            moveVelocity = new Vector2(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(startGlow());
        }

        //do not remove
        if (moveInput.x == 1)
        {
            nsew = 2;
        }

        else if (moveInput.x == -1)
        {
            nsew = 4;
        }

        else if (moveInput.y == 1)
        {
            nsew = 1;
        }

        else if (moveInput.y == -1)
        {
            nsew = 3;
        }

        int openedChestCnt = GameObject.FindGameObjectsWithTag("OpenedChest").Length;

        //Debug.Log("coinsCnt: " + coinsCnt);
        //Debug.Log("openedChestCnt: " + openedChestCnt);

        if (openedChestCnt == 6 && coinsCnt == 0)
        {
            Debug.Log("Finish all chestboxs and coins");
            EndGame(true);
        }

        //Move by keyboard
        /*
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            //this.transform.localScale = new Vector3(23.74183f, 23.74183f, 1f);  //localScale
            this.transform.localScale = new Vector3(Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            //this.transform.localScale = new Vector3(-23.74183f, 23.74183f, 1f);  //localScale
            this.transform.localScale = new Vector3(-Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        moveVelocity = moveInput.normalized * speed;
        */


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
            coinsCnt--;
            ScoreUpdate();
        }

        if (other.gameObject.tag == "Minion" && !isGlowing && !isInjured)
        {
            minions = GameObject.FindGameObjectsWithTag("Minion");

            foreach (GameObject minion in minions)
            {
                MinionController mcs = minion.GetComponent<MinionController>();
                isFreeze = mcs.isPause;
            }

            if (!isFreeze)
                LifeUpdate();
        }

        else if ((other.gameObject.tag == "Minion" || other.gameObject.tag == "Fireball") && isGlowing)
        {
            Destroy(other.gameObject);
            FindObjectOfType<SoundManager>().Play("MinionDeath");
            Vector3 offset = new Vector3(-0.2f, 0.1f, 0);
            Instantiate(deadEffect, other.transform.position + offset, Quaternion.identity);
            StartCoroutine(activateEffect());
        }

        if (other.gameObject.tag == "Fireball" && !isGlowing && !isInjured)
        {
            Destroy(other.gameObject);
            LifeUpdate();
        }
    }

    IEnumerator activateEffect()
    {
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.FindWithTag("deadEffect"));
    }

    IEnumerator hit()
    {
        FindObjectOfType<SoundManager>().Play("CharacterHit");

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 8; i++)
        {
            isInjured = true;
            renderer.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            yield return new WaitForSeconds(0.1f);
        }

        isInjured = false;
        yield return null;
    }

    public void IStartGlow()
    {
        StartCoroutine(startGlow());
    }

    IEnumerator startGlow()
    {
        isGlowing = true;
        Vector3 offset = new Vector3(0, -0.1f, 0);
        Instantiate(glow, transform.position + offset, Quaternion.identity, transform);

        yield return new WaitForSeconds(glowTime);

        isGlowing = false;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        yield return null;
    }

    public void Push()
    {
        switch (nsew)
        {
            case 1:
                rb.AddForce(-transform.up * 12);
                break;
            case 2:
                rb.AddForce(-transform.right * 12);
                break;
            case 3:
                rb.AddForce(transform.up * 12);
                break;
            case 4:
                rb.AddForce(transform.right * 12);
                break;
        }
    }

    public void ScoreUpdate(int point=10)
    {
        score += point;
        scoreText.text = "Coin: " + score;
    }

    public void LifeUpdate()
    {
        countLife -= 1;
        Destroy(heartArr[countLife]);
        //lifeText.text = "Life: " + countLife;
        bool win;

        if (countLife == 0)
        {
            //PauseGame();
            //OnPlayerDied(score); //event sent to LevelController

            GameObject explosion = (GameObject)Instantiate(explosionRef);
            explosion.transform.position = new Vector2(transform.position.x, transform.position.y + .3f);
            Destroy(gameObject);
            FindObjectOfType<SoundManager>().Play("CharacterDeath");

            if (correctAns >= 4)  //Answer correctly 4 questions to win
            {
                win = true;
            }

            else
            {
                win = false;
            }

            minions = GameObject.FindGameObjectsWithTag("Minion");

            if (minions[0] != null) {
                foreach (GameObject minion in minions)
                {
                    minion.GetComponent<MinionController>().stopFiring = true;
                }
            }


            //EndGame(LevelController.Instance.getWin());
            EndGame(win);

            //StartCoroutine(passWin(win));
        }

        else
        {
            StartCoroutine(hit());
            //lifeBox.enabled = true;
            //PauseGame();
            //Invoke("ResumeGame", 0.5f);
        }
    }

    //IEnumerator passWin(bool win)
    //{
    //    Debug.Log("passWin called");
    //    yield return new WaitForSeconds(2f);
    //    EndGame(win);
    //    Debug.Log("passWin22 called");
    //    yield return null;
    //}

    private void EndGame(bool win)
    {
        MiniChestController.OpenedChestInstance.CloseOpenedChest();
        PauseGame();

        var topic = SectionController.currentTopic;
        var user = ConnectionManager.user;
        var level = LevelController.chosenLevel;

        int chosenLevel = LevelController.Instance.getChosenLevel();
        int lastCompletedLevel = LevelController.Instance.getLastCompletedLevel();

        if (chosenLevel == lastCompletedLevel + 1)
        {
            LevelController.Instance.unlockNewLevel = win;
            StartCoroutine(ConnectionManager.UpdateHighscore(user.Id, topic.Id, level, score, win));
        }
        else
        {
            StartCoroutine(ConnectionManager.UpdateHighscore(user.Id, topic.Id, level, score, true));
        }

        if (win)
        {
            LevelController.Instance.WinPopUp(score);
        }
        else
        {
            LevelController.Instance.GameOverPopUp(score);
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

    public void PauseGame()
    {
        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().isPause = true;
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = true;
    }

    public void openNewLevel()
    {
        int newLevel = LevelController.Instance.getChosenLevel() + 1;
        levelText.text = "Level " + newLevel + " is opened";
        levelText.enabled = true;
        levelBox.enabled = true;
        Invoke("hideLevelBox", 2f);

        LevelController.Instance.unlockNewLevel = true;
    }

    
    public void increaseCorrectAns()
    {
        int chosenLevel = LevelController.Instance.getChosenLevel();
        int lastCompletedLevel = LevelController.Instance.getLastCompletedLevel();

        correctAns += 1;
        if (correctAns == 4 && chosenLevel == lastCompletedLevel + 1)
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

    public void moveLeft()
    {
        direction = 1;
    }

    public void moveRight()
    {
        direction = 0;
    }

    public void moveUp()
    {
        direction = 2;
    }

    public void moveDown()
    {
        direction = 3;
    }

    public void stopMove()
    {
        direction = -1;
    }
}