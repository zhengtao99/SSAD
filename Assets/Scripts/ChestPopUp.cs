using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPopUp : MonoBehaviour
{

    private GameObject[] minions;
    private GameObject player;
    private Rigidbody2D rb;

    private bool isOpened = false;



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
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = true;
    }

    void ResumeGame()
    {
        foreach (GameObject minion in minions)
        {

            minion.GetComponent<MinionController>().isPause = false;

        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isPause = false;
        GameManager.Instance.SetPageState(GameManager.PageState.Play);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.tag == "Player" && !isOpened)
        {
            PauseGame();
            GameManager.Instance.ChestPopUp();
            CreateOpenedChest.OpenedChestInstance.CreateOpenedChests(transform.position.x,
            transform.position.y, transform.position.z);
            gameObject.SetActive(false);
            Invoke("ResumeGame", 1f);
            isOpened = true;
            
        }
    }
}
