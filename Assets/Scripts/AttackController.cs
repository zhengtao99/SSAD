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
        if (chosenLevel > 0 && chosenLevel < 4)
        {
            FreezeMinions();
        }

        else if (chosenLevel > 3 && chosenLevel < 8)
        {
            rn = Random.Range(1, 3);

            if (rn == 1)
            {
                SlowMinions();
            }

            else if (rn == 2)
            {
                FreezeMinions();
            }
        }

        else if (chosenLevel > 7)
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
    }

    public void MinionAttack()
    {
        if (chosenLevel > 3 && chosenLevel < 8)
        {
            SpeedUpMinions();
        }

        else if (chosenLevel > 7)
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
    }

    void FreezeMinions()
    {
        Debug.Log("Freeze");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().IFreezeMinions();
        }
    }

    void SlowMinions()
    {
        Debug.Log("Slow");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().ISlowDownMinion();
        }
    }

    void PlayerGlow()
    {
        Debug.Log("Glow");

        GameObject.FindWithTag("Player").GetComponent<PlayerController>().IStartGlow();
    }

    void SpeedUpMinions()
    {
        Debug.Log("Speed");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().ISpeedUpMinion();
        }
    }

    void FireBallAttack()
    {
        Debug.Log("Fireball");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().IFire();
        }
    }
}
