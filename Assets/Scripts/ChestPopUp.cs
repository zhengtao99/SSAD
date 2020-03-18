using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPopUp : MonoBehaviour
{
    public static ChestPopUp Instance;
    private GameObject[] minions;
    private GameObject player;
    private Rigidbody2D rb;

    private bool isOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;        
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

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

    void OnTriggerStay2D(Collider2D other)
    { 
        if (other.tag == "Player" && !isOpened)
        {
            PauseGame();
            FindObjectOfType<SoundManager>().Play("ChestOpening");
            CreateOpenedChest.OpenedChestInstance.CreateOpenedChests(transform.position.x,
            transform.position.y, transform.position.z);
            gameObject.SetActive(false);
            Invoke("delayPopUp",1.5f);
            //Invoke("ResumeGame",1f);
            isOpened = true;

            GameObject.FindWithTag("Player").GetComponent<PlayerController>().Push();
        }
    }

    void delayPopUp(){
        GameManager.Instance.QuestionPopUp();
    }

    public void PAttack()
    {
        //Debug.Log("activate PAttack");
        GameObject.FindWithTag("Player").GetComponent<AttackController>().PlayerAttack();
    }

    public void MAttack()
    {
        //Debug.Log("activate MAttack");
        GameObject.FindWithTag("Player").GetComponent<AttackController>().MinionAttack();
    }
}
