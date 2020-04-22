using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using UnityEngine.Networking;
using System.Linq;
using Photon.Pun;

/// <summary>
/// This controller class  holds the methods responsible for the player's movements, score and life updates in the game.
/// </summary>
public class PlayerController : MonoBehaviour
{

    /// <summary>
    /// A variable that contains the glow effect game object that will be casted on the player.
    /// </summary>
    public GameObject glow;

    /// <summary>
    /// A variable that contains the dead effect game object that will be casted on the player.
    /// </summary>
    public GameObject deadEffect;

    /// <summary>
    /// A variable that contains an array of heart game objects representing the number of lives the player has in the game.
    /// </summary>
    private GameObject[] heartArr;

    /// <summary>
    /// A variable that contains boolean to check if player is glowing, this is used to check if player can kill the minions.
    /// </summary>
    public bool isGlowing = false;

    /// <summary>
    /// A variable that contains a time limit to the how long the player can hold the glow effect.
    /// </summary>
    public float glowTime = 10.0f;
    /// <summary>
    /// A variable that contains a boolean to check if the player has collided with minions or answered a question wrongly, this is used to decrease the player's lives accordingly.
    /// </summary>
    public bool isInjured = false;

    /// <summary>
    /// A variable that contains unity defined explosion effect game object.
    /// </summary>
    private UnityEngine.Object explosionRef;

    /// <summary>
    /// A variable that contains an array of minion gameobjects.
    /// </summary>
    private GameObject[] minions;

    /// <summary>
    /// A delegate variable used to detect player events. 
    /// </summary>
    public delegate void PlayerDelegate(int value);

    /// <summary>
    /// A event variable that will receive the delegate notification when player dies.
    /// </summary>
    public static event PlayerDelegate OnPlayerDied;

    /// <summary>
    /// A variable that contains player's lives count initialized to 3.
    /// </summary>
    private int countLife = 3;

    /// <summary>
    /// A variable that contains the border UI object that surrounds the life game objects.
    /// </summary>
    private SpriteRenderer lifeBox;

    /// <summary>
    /// A variable that contains life text.
    /// </summary>
    private Text lifeText;
    /// <summary>
    /// A variable that contains score text
    /// </summary>
    private Text scoreText;

    /// <summary>
    /// A variable that contains the player game object.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// A variable that contains the correct answer id.
    /// </summary>
    private int correctAns;

    /// <summary>
    /// A variable that contains the level text.
    /// </summary>
    private Text levelText;
    /// <summary>
    /// A variable that contains the level border around the text.
    /// </summary>
    private SpriteRenderer levelBox;

    /// <summary>
    /// A variable that contains the player's rigidbody2D component used to detect collisions.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// A variable that contains the player's move velocity.
    /// </summary>
    private Vector2 moveVelocity;
    /// <summary>
    /// A variable that contains a boolean to check if the game is paused, it is used to restrict the player's movement is the game is paused.
    /// </summary>
    public bool isPause = false;

    /// <summary>
    /// A variable that contains player's speed.
    /// </summary>
    public float speed;

    /// <summary>
    /// A variable that contains player's score accumulated in the one game run.
    /// </summary>
    int score = 0;

    /// <summary>
    /// A variable that contains player's facing direction.
    /// </summary>
    private int direction;

    /// <summary>
    /// A variable that contains the joy stick move input.
    /// </summary>
    private Vector2 moveInput;

    /// <summary>
    /// A variable that contains joy stick position.
    /// </summary>
    int nsew = 0;

    /// <summary>
    /// A variable that contains the count of coins accumulated.
    /// </summary>
    private int coinsCnt;

    /// <summary>
    /// A variable that contains boolean to check if player is freezed to restrict the player's movement in the maze.
    /// </summary>
    bool isFreeze;

    /// <summary>
    /// This method is called to initialize all of the parameters in the player controller to set up the initial game play environment.
    /// </summary>
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

    /// <summary>
    /// This method is called once per frame to update the player's moveInput according to the input received from the joystick manipulation.
    /// </summary>
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

    /// <summary>
    /// This method is called one per frame to move the player rigidbody2D component according to the moveInput
    /// </summary>
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

    /// <summary>
    /// <para>
    /// This method is called on collision and it will check if the collided game object is a coin or minion and does the manipulation accordingly. 
    /// Coin will increase the player's points and minion will decrease the player's lives.
    /// </para>
    /// </summary>
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


    /// <summary>
    /// This method is called once the player runs out of lives and the dead effect will appear on the player's avatar.
    /// </summary>
    IEnumerator activateEffect()
    {
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.FindWithTag("deadEffect"));
    }

    /// <summary>
    /// This method is called once the player touches a minion, it will display the hurt effect.
    /// </summary>
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

    /// <summary>
    /// This method is called when player is to receive a power up, the startglow() method is called.
    /// </summary>
    public void IStartGlow()
    {
        StartCoroutine(startGlow());
    }

    /// <summary>
    /// This method is called to place a glow on the player.
    /// </summary>
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

    /// <summary>
    /// This method is called to push players away from chest boxes.
    /// </summary>
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

    /// <summary>
    /// This method is called to update the player's score on coin accumulated as well as the number of questions answered correctly.
    /// </summary>
    public void ScoreUpdate(int point=10)
    {
        score += point;
        scoreText.text = "Coin: " + score;
    }

    /// <summary>
    /// This method is called to update the player's lives remaining in the game.
    /// </summary>
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

    /// <summary>
    /// This method is called to restart the environment on game end.
    /// </summary>
    /// <param name="win">A boolean to indicate whether the player win or lose the game.</param>
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

    /// <summary>
    /// This method is called to resume game play. ALlow minions and player to move again.
    /// </summary>
    public void ResumeGame()
    {
        foreach (GameObject minion in minions)
        {

            minion.GetComponent<MinionController>().isPause = false;

        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = false;
        lifeBox.enabled = false;
    }

    /// <summary>
    /// This method is called to pause game play, restricting minions and player's movement.
    /// </summary>
    public void PauseGame()
    {
        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().isPause = true;
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = true;
    }

    /// <summary>
    /// This method is called when player answers 3 questions and a new level will be unlocked.
    /// </summary>
    public void openNewLevel()
    {
        int newLevel = LevelController.Instance.getChosenLevel() + 1;
        levelText.text = "Level " + newLevel + " is opened";
        levelText.enabled = true;
        levelBox.enabled = true;
        Invoke("hideLevelBox", 2f);

        LevelController.Instance.unlockNewLevel = true;
    }

    /// <summary>
    /// This method is called whenever a player answers a question correctly.
    /// </summary>
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

    /// <summary>
    /// This method is called to hide the new level box.
    /// </summary>
    public void hideLevelBox()
    {
        Debug.Log("aha");
        levelText.enabled = false;
        levelBox.enabled = false;
    }

    /// <summary>
    /// This method is called to set the direction = 1, pointing to left.
    /// </summary>
    public void moveLeft()
    {
        direction = 1;
    }

    /// <summary>
    /// This method is called to set the direction = 0, pointing to right.
    /// </summary>
    public void moveRight()
    {
        direction = 0;
    }

    /// <summary>
    /// This method is called to set the direction = 2, pointing to up.
    /// </summary>
    public void moveUp()
    {
        direction = 2;
    }

    /// <summary>
    /// This method is called to set the direction = 3, pointing to down.
    /// </summary>
    public void moveDown()
    {
        direction = 3;
    }

    /// <summary>
    /// This method is called to set the direction = -1, pointing to no directions.
    /// </summary>
    public void stopMove()
    {
        direction = -1;
    }
}