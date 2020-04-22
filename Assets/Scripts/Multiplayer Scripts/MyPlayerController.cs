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
public class MyPlayerController : MonoBehaviour
{
    /// <summary>
    /// A PhotonView variable that will hold the other image of the maze. It is used in multiplayer gameplay mode.
    /// </summary>
    private PhotonView PV;

    /// <summary>
    /// A delegate variable used to detect player events. 
    /// </summary>
    public delegate void PlayerDelegate(int value);

    /// <summary>
    /// A event variable that will receive the delegate notification when player dies.
    /// </summary>
    public static event PlayerDelegate OnPlayerDied;

    /// <summary>
    /// A variable that contains the player's rigidbody2D component used to detect collisions.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// A variable that contains the player's move velocity.
    /// </summary>
    private Vector2 moveVelocity;

    /// <summary>
    /// A variable that contains player's speed.
    /// </summary>
    public float speed;

    /// <summary>
    /// A variable that contains player's score accumulated in the one game run.
    /// </summary>
    public int Score = 0;

    /// <summary>
    /// A variable that contains my score text.
    /// </summary>
    private Text myScoreText;

    /// <summary>
    /// A variable that contains score.
    /// </summary>
    int score = 0;

    /// <summary>
    /// A variable that contains the heart array.
    /// </summary>
    private GameObject[] heartArr;

    /// <summary>
    /// A variable that contains player's lives count initialized to 3.
    /// </summary>
    private int countLife = 3;
    /// <summary>
    /// A variable that contains unity defined explosion effect game object.
    /// </summary>
    private UnityEngine.Object explosionRef;

    /// <summary>
    /// A variable that contains a boolean to check if the player has collided with minions or answered a question wrongly, this is used to decrease the player's lives accordingly.
    /// </summary>
    public bool isInjured = false;

    /// <summary>
    /// A variable that contains an array of minion gameobjects.
    /// </summary>
    private GameObject[] minions;

    /// <summary>
    /// A variable that contains boolean to check if player is freezed to restrict the player's movement in the maze.
    /// </summary>
    bool isFreeze;

    /// <summary>
    /// A variable that contains a boolean to check if the game is paused, it is used to restrict the player's movement is the game is paused.
    /// </summary>
    public bool isPause = false;

    /// <summary>
    /// A variable that contains player's facing direction.
    /// </summary>
    private int direction;

    /// <summary>
    /// This method is called to initialize all of the parameters in the myplayer controller to set up the initial game play environment.
    /// </summary>
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        
        var mazePage = MultiplayerSceneManager.Instance.playPage;
        if (mazePage == null)
            return;

        if (!PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("player 1");
            myScoreText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText_Player1").First();
            heartArr = GameObject.FindGameObjectsWithTag("Heart_player1");
        }
        else
        {
            //Debug.Log("player 2");
            myScoreText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText_Player2").First();
            heartArr = GameObject.FindGameObjectsWithTag("Heart_player2");
        }
        score = 0;
        myScoreText.text = PhotonNetwork.NickName + "\n" + score;
        RoomController.Instance.ScoreUpdateOtherSide(score);

        explosionRef = Resources.Load("PlayerExplosion");

        direction = 4;
    }

    /// <summary>
    /// This method is called once per frame to update the player's moveInput according to the input received from the joystick manipulation.
    /// </summary>
    void Update()
    {
        if (PV.IsMine)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (direction == 0)
            {
                //this.transform.localScale = new Vector3(23.74183f, 23.74183f, 1f);  //localScale
                this.transform.localScale = new Vector3(Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
                moveVelocity = new Vector2(5, 0);
            }
            else if (direction == 1)
            {
                //this.transform.localScale = new Vector3(-23.74183f, 23.74183f, 1f);  //localScale
                this.transform.localScale = new Vector3(-Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
                moveVelocity = new Vector2(-5, 0);
            }
            else if (direction == 2)
            {
                moveVelocity = new Vector2(0, 5);
            }
            else if (direction == 3)
            {
                moveVelocity = new Vector2(0, -5);
            }
            else
            {
                moveVelocity = new Vector2(0, 0);
            }

            //Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            //if (Input.GetAxisRaw("Horizontal") == 1)
            //{
            //    //this.transform.localScale = new Vector3(23.74183f, 23.74183f, 1f);  //localScale
            //    this.transform.localScale = new Vector3(Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            //}
            //else if (Input.GetAxisRaw("Horizontal") == -1)
            //{
            //    //this.transform.localScale = new Vector3(-23.74183f, 23.74183f, 1f);  //localScale
            //    this.transform.localScale = new Vector3(-Math.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            //}
            //moveVelocity = moveInput.normalized * speed;
        }
    }


    /// <summary>
    /// This method is called one per frame to move the player rigidbody2D component according to the moveInput
    /// </summary>
    private void FixedUpdate()
    {
        if (PV.IsMine)
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
            Destroy(other.gameObject);
            if (PV.IsMine)
            {
                FindObjectOfType<SoundManager>().Play("CoinCollection");
                ScoreUpdate();
            }
        }


        if (other.gameObject.tag == "Minion" && !isInjured && PhotonNetwork.IsMasterClient)
        {
            minions = GameObject.FindGameObjectsWithTag("Minion");

            foreach (GameObject minion in minions)
            {
                MinionController mcs = minion.GetComponent<MinionController>();
                isFreeze = mcs.isPause;
            }

            if (!isFreeze)
            {
                LifeUpdate();
                Debug.Log(countLife);
            }
        }

        Debug.Log(PhotonNetwork.IsMasterClient + other.gameObject.tag);

        if (other.gameObject.tag == "Red" && !isInjured && !PhotonNetwork.IsMasterClient)
        {

            minions = GameObject.FindGameObjectsWithTag("Red");

            foreach (GameObject minion in minions)
            {
                MinionController mcs = minion.GetComponent<MinionController>();
                isFreeze = mcs.isPause;
            }

            if (!isFreeze)
            {
                LifeUpdate();
                Debug.Log(countLife);
            }
        }

    }

    /// <summary>
    /// This method is called to update the player's score on coin accumulated as well as the number of questions answered correctly.
    /// </summary>
    public void ScoreUpdate(int point = 10)
    {
        score += point;
        myScoreText.text = PhotonNetwork.NickName + "\n" + score;
        RoomController.Instance.ScoreUpdateOtherSide(score);
    }

    /// <summary>
    /// This method is called to update the player's lives remaining in the game.
    /// </summary>
    public void LifeUpdate()
    {
        countLife -= 1;
        if (PV.IsMine)
        {
            Destroy(heartArr[countLife]);
            RoomController.Instance.LifeUpdateOtherSide(countLife);
        }

        if (countLife == 0)
        {
            GameObject explosion = (GameObject)Instantiate(explosionRef);
            explosion.transform.position = new Vector2(transform.position.x, transform.position.y + .3f);
            Destroy(gameObject);
            FindObjectOfType<SoundManager>().Play("CharacterDeath");
            PauseGame();
            RoomController.Instance.GameOverPopUp();
        }
        else
        {
            StartCoroutine(hit());
        }
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
    /// This method is called to resume game play. ALlow minions and player to move again.
    /// </summary>
    public void ResumeGame()
    {
        foreach (GameObject minion in minions)
        {

            minion.GetComponent<MinionController>().isPause = false;

        }
        if (PV.IsMine)
            isPause = false;
    }

    /// <summary>
    /// This method is called to pause game play, restricting minions and player's movement.
    /// </summary>
    void PauseGame()
    {
        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().isPause = true;
        }
        if (PV.IsMine)
            isPause = true;
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