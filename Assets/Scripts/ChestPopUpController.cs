using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all methods required to perform the Chest animation when the player touches a chest in the game.
/// </summary>
public class ChestPopUpController : MonoBehaviour
{
    /// <summary>
    /// Creates the instance to allow other GameObjects' scripts to access the methods here.
    /// </summary>
    public static ChestPopUpController Instance;

    /// <summary>
    /// A variable to store all minion gameobjects that will be manipulated perform chest animation.
    /// </summary>
    private GameObject[] minions;

    /// <summary>
    /// A variable to get the rigidbody component of the current chest gameobject to detect gameobject collisions, the chest animation's activation point.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// A variable to control the closing of all the chest in cases where players dies and game has to be replayed.
    /// </summary>
    private bool isOpened = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// To pause the game when the player is answering a question, freezes minions and player movements
    /// </summary>
    void PauseGame()
    {
        minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {

            minion.GetComponent<MinionController>().isPause = true;

        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = true;
        GameManager.Instance.QuestionPopUp();
    }

    /// <summary>
    /// To resume the game when the player is done answering a question, unfreezes minions and player movements
    /// </summary>
    public void ResumeGame()
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

    /// <summary>
    /// To detect collisions between player and chestbox which activates chest animation and Question pop-ups
    /// </summary>
    /// <param name="other">The collided game object</param>
    void OnTriggerStay2D(Collider2D other)
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

    /// <summary>
    /// To delay the questions from showing up before the chest animation completes
    /// </summary>
    void delayPopUp()
    {
        GameManager.Instance.QuestionPopUp();
    }

    /// <summary>
    /// The Method gives a player a buff on resume game, when the player answered a question correctly.
    /// </summary>
    public void PAttack()
    {
        //Debug.Log("activate PAttack");
        GameObject.FindWithTag("Player").GetComponent<AttackController>().PlayerAttack();
    }

    /// <summary>
    /// The Method gives a the minions buff on resume game, when the playe answered a question wrongly.
    /// </summary>
    public void MAttack()
    {
        //Debug.Log("activate MAttack");
        GameObject.FindWithTag("Player").GetComponent<AttackController>().MinionAttack();
    }
}
