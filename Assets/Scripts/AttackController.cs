using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] minions;
    int chosenLevel;
    int rn;

    void Start()
    {
        minions = GameObject.FindGameObjectsWithTag("Minion");
        chosenLevel = PlayerPrefs.GetInt("chosenLevel");
    }

    public void PlayerAttack()
    {
        rn = Random.Range(1, 4);

        if (rn == 1)
        {
            SlowMinions();
        }

        else if (rn == 2)
        {
            FreezeMinions();
        }

        else if (rn == 3)
        {
            PlayerGlow();
        }
    }

    public void MinionAttack()
    {
        rn = Random.Range(1, 3);

        if (rn == 1)
        {
            SpeedUpMinions();
        }

        else if (rn == 2)
        {
            FireBallAttack();
        }
    }

    void FreezeMinions()
    {
        Debug.Log("Freeze");

        minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().IFreezeMinions();
        }
    }

    void SlowMinions()
    {
        Debug.Log("Slow");

        minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().ISlowDownMinion();
        }
    }

    void PlayerGlow()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().IStartGlow();
    }

    void SpeedUpMinions()
    {
        minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().ISpeedUpMinion();
        }
    }

    void FireBallAttack()
    {
       minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().IFire();
        }
    }
}
