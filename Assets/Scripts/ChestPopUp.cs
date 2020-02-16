using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPopUp : MonoBehaviour
{

    private GameObject[] minions;
    private GameObject player;
    private Rigidbody2D rb;
    public Vector2 moveInput;



    // Start is called before the first frame update
    void Start()
    {
        minions = GameObject.FindGameObjectsWithTag("Minion");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PauseGame()
    {
        foreach (GameObject minion in minions)
        {

            minion.GetComponent<MinionController>().isPause = true;

        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<NewBehaviourScript>().isPause = true;
    }

    void ResumeGame()
    {
        foreach (GameObject minion in minions)
        {

            minion.GetComponent<MinionController>().isPause = false;

        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<NewBehaviourScript>().isPause = false;
        GameManager.Instance.SetPageState(GameManager.PageState.Play);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            PauseGame();
            GameManager.Instance.ChestPopUp();
            Invoke("ResumeGame", 1.0f);
        }
    }
}
