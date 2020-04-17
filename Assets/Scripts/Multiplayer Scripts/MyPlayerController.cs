using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using UnityEngine.Networking;
using System.Linq;
using Photon.Pun;

public class MyPlayerController : MonoBehaviour
{
    private PhotonView PV;

    public delegate void PlayerDelegate(int value);
    public static event PlayerDelegate OnPlayerDied;

    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    public float speed;

    public int Score = 0;
    private Text myScoreText;
    int score = 0;

    private GameObject[] heartArr;
    private int countLife = 3;
    private UnityEngine.Object explosionRef;
    public bool isInjured = false;
    private GameObject[] minions;
    bool isFreeze;

    public bool isPause = false;
    private int direction;
    //public Text txt;
    // Update is called once per frame
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

    public void ScoreUpdate(int point = 10)
    {
        score += point;
        myScoreText.text = PhotonNetwork.NickName + "\n" + score;
        RoomController.Instance.ScoreUpdateOtherSide(score);
    }

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

    public void ResumeGame()
    {
        foreach (GameObject minion in minions)
        {

            minion.GetComponent<MinionController>().isPause = false;

        }
        if (PV.IsMine)
            isPause = false;
    }

    void PauseGame()
    {
        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().isPause = true;
        }
        if (PV.IsMine)
            isPause = true;
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