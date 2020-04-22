using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// This controller class contains all methods required to perform the Chest animation when the player touches a chest in the game.
/// </summary>
public class ChestPopUpController : MonoBehaviour
{
    public bool isMultiplayerMode;

    /// <summary>
    /// A variable that stores the controller class instance, it will allow other GameObjects' scripts to access the methods here.
    /// </summary>
    public static ChestPopUpController Instance;

    /// <summary>
    /// A variable to store all minion gameobjects that will be manipulated to perform the chest animation during gameplay.
    /// </summary>
    private GameObject[] minions;

    /// <summary>
    /// A variable to store all minion gameobjects that will be used in the multiplayer scene.
    /// </summary>
    private GameObject[] redMinions;

    /// <summary>
    /// A variable that stores the rigidbody component of the current chest gameobject to detect gameobject collisions, the chest animation's activation point.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// A boolean variable used to control the closing of all the chest in cases where players dies and game has to be replayed.
    /// </summary>
    private bool isOpened = false;


    /// <summary>
    /// Start is called before the first frame update, it is used to instantiate this controller class instance.
    /// </summary>
    void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// This method is used to pause the game when the player is answering a question, freezes minions and player movements
    /// </summary>
    void PauseGame()
    {
        if (!isMultiplayerMode)
        {
            minions = GameObject.FindGameObjectsWithTag("Minion");

            foreach (GameObject minion in minions)
            {

                minion.GetComponent<MinionController>().isPause = true;

            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = true;
            GameManager.Instance.QuestionPopUp();
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                minions = GameObject.FindGameObjectsWithTag("Minion");
                foreach (GameObject minion in minions)
                {

                    minion.GetComponent<MinionController>().isPause = true;

                }
            }
            else
            {
                redMinions = GameObject.FindGameObjectsWithTag("Red");
                foreach (GameObject red in redMinions)
                {
                    red.GetComponent<MinionController>().isPause = true;
                    Debug.Log(red.GetComponent<MinionController>().isPause);
                }
            }

            MultiplayerSceneManager.Instance.myPlayer.GetComponent<MyPlayerController>().isPause = true;
            MultiplayerSceneManager.Instance.QuestionPopUp();
        }
    }

    /// <summary>
    /// This method is used to freeze the minions in multiplayer gameplay when the player is answering a question.
    /// </summary>
    void PauseMinions()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            minions = GameObject.FindGameObjectsWithTag("Minion");
            foreach (GameObject minion in minions)
            {

                minion.GetComponent<MinionController>().isPause = true;

            }
        }
        else
        {
            redMinions = GameObject.FindGameObjectsWithTag("Red");
            foreach (GameObject red in redMinions)
            {
                red.GetComponent<MinionController>().isPause = true;
                Debug.Log(red.GetComponent<MinionController>().isPause);
            }
        }
    }
    /// <summary>
    /// This method is used to resume the game when the player is done answering a question, unfreezes minions and player movements
    /// </summary>
    public void ResumeGame()
    {
        if (!isMultiplayerMode)
        {
            minions = GameObject.FindGameObjectsWithTag("Minion");

            foreach (GameObject minion in minions)
            {

                minion.GetComponent<MinionController>().isPause = false;

            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = false;

            //invoke attack
            int correct = PlayerPrefs.GetInt("correct");
            if (correct == 1)
            {
                //Debug.Log("check correct is true, check actual correct:" + correct);
                PAttack();
            }

            else if (correct == 0)
            {
                //Debug.Log("check correct is false, check actual correct:" + correct);
                MAttack();
            }

            GameManager.Instance.SetPageState(GameManager.PageState.Play);
        }
        else
        {
            minions = GameObject.FindGameObjectsWithTag("Minion");
            foreach (GameObject minion in minions)
            {

                minion.GetComponent<MinionController>().isPause = false;

            }

            redMinions = GameObject.FindGameObjectsWithTag("Red");
            foreach (GameObject red in redMinions)
            {
                red.GetComponent<MinionController>().isPause = false;
            }
            MultiplayerSceneManager.Instance.myPlayer.GetComponent<MyPlayerController>().isPause = false;

            //invoke attack
            int correct = PlayerPrefs.GetInt("correct");
            if (correct == 1)
            {
                //Debug.Log("check correct is true, check actual correct:" + correct);
                PAttack();
            }
            else if (correct == 0)
            {
                //Debug.Log("check correct is false, check actual correct:" + correct);
                MAttack();
            }

            MultiplayerSceneManager.Instance.SetPageState(MultiplayerSceneManager.PageState.Play);
        }
    }

    /// <summary>
    /// This method is used to detect collisions between player and chestbox which activates chest animation and Question pop-ups.
    /// </summary>
    /// <param name="other">The collided game object</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (!isMultiplayerMode)
        {
            if (other.tag == "Player" && !isOpened)
            {
                PauseGame();
                FindObjectOfType<SoundManager>().Play("ChestOpening");
                MiniChestController.OpenedChestInstance.CreateOpenedChests(transform.position.x,
                transform.position.y, transform.position.z);
                gameObject.SetActive(false);
                Invoke("delayPopUp", 1.5f);
                //Invoke("ResumeGame",1f);
                isOpened = true;
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().Push();
            }
        }

        else
        {
            if (other.tag == "Player" && !isOpened)
            {
                if (other.gameObject == MultiplayerSceneManager.Instance.myPlayer && !isOpened)
                {
                    PauseGame();
                    FindObjectOfType<SoundManager>().Play("ChestOpening");
                    MiniChestController.OpenedChestInstance.CreateOpenedChests(transform.position.x,
                    transform.position.y, transform.position.z);
                    RoomController.Instance.MiniChestUpdateOtherSide(transform.position.x,
                    transform.position.y, transform.position.z);

                    Invoke("delayPopUp", 1.5f);
                    //Invoke("ResumeGame",1f);
                    isOpened = true;

                }
                else if (!isOpened)
                {
                    PauseMinions();
                }
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// This method is used to delay the question pop ups and allow the chest to finishing animating.
    /// </summary>
    void delayPopUp()
    {
        if (!isMultiplayerMode)
            GameManager.Instance.QuestionPopUp();
        else
            MultiplayerSceneManager.Instance.QuestionPopUp();
    }

    /// <summary>
    /// Ths method is used to give the player a buff on resume game, when the player answered a question correctly.
    /// </summary>
    public void PAttack()
    {
        if (!isMultiplayerMode)
            GameObject.FindWithTag("Player").GetComponent<AttackController>().PlayerAttack();
    }

    /// <summary>
    /// This method is used to give the minions a buff on resume game, when the playe answered a question wrongly.
    /// </summary>
    public void MAttack()
    {
        if (!isMultiplayerMode)
            GameObject.FindWithTag("Player").GetComponent<AttackController>().MinionAttack();
    }
}
