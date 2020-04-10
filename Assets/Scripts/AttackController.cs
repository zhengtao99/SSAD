using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class contains all the methods to generate the various attacks that will be given to the player a a reward or to penalize them.
/// </summary>
public class AttackController : MonoBehaviour
{
    /// <summary>
    /// A variable list holding all four minions in the maze.
    /// </summary>
    private GameObject[] minions;

    /// <summary>
    /// A variable holding the player's selected difficulty level.
    /// </summary>
    int chosenLevel;

    /// <summary>
    /// A variable to hold a random number generated to randomly pick out an attack.
    /// </summary>
    int rn;

    /// <summary>
    /// The method is called before the first frame update to gather all the minions and get the difficulty level selected by the player,
    /// </summary>
    void Start()
    {
        minions = GameObject.FindGameObjectsWithTag("Minion");
        chosenLevel = PlayerPrefs.GetInt("chosenLevel");
    }

    /// <summary>
    /// The method is responsible for giving players abilities to slow, freeze or to kill the minions
    /// </summary>
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

    /// <summary>
    /// The method is responsible for giving minions abilities to speed up themselves or to fire ball attack the players.
    /// </summary>
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

    /// <summary>
    /// The method is used by the player to freeze the minions.
    /// </summary>
    void FreezeMinions()
    {
        Debug.Log("Freeze");

        minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().IFreezeMinions();
        }
    }

    /// <summary>
    /// The method is used by the player to slow down the minions.
    /// </summary>
    void SlowMinions()
    {
        Debug.Log("Slow");

        minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().ISlowDownMinion();
        }
    }

    /// <summary>
    /// The method is surround the player with glow and allow players to kill minions in a certain amount of time period.
    /// </summary>
    void PlayerGlow()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().IStartGlow();
    }

    /// <summary>
    /// The method is used by the minions to speed themselves up.
    /// </summary>
    void SpeedUpMinions()
    {
        minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().ISpeedUpMinion();
        }
    }

    /// <summary>
    /// The method is used by the minions to launch fire ball attacks.
    /// </summary>
    void FireBallAttack()
    {
       minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionController>().IFire();
        }
    }
}
