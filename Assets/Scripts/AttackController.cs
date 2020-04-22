using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The controller class contains all the methods to generate the various attacks that will be given to the player a a reward or to penalize them.
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
    /// A variable holding a random number generated  and is used to randomly pick out an attack.
    /// </summary>
    int rn;

    /// <summary>
    /// This method is called before the first frame update, it is to gather all the minions and get the difficulty level selected by the player.
    /// </summary>
    void Start()
    {
        minions = GameObject.FindGameObjectsWithTag("Minion");
        chosenLevel = PlayerPrefs.GetInt("chosenLevel");
    }

    /// <summary>
    /// This method is responsible for giving players abilities to slow, freeze or to kill the minions.
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
    /// This method is responsible for giving minions abilities to speed up themselves or to launch fire ball attack in four directions.
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
    /// This method is used in PlayerAttack() method, it is used to freeze the minions.
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
    ///This method is used in PlayerAttack() method, it is used slow down the minions.
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
    /// This method is used in PlayerAttack(), it is used to enable players to be able to kill minions in a certain amount of time period.
    /// </summary>
    void PlayerGlow()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().IStartGlow();
    }

    /// <summary>
    /// This method is used in MinionAttack() method, it is used to speed up the minions.
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
    /// The is used in the MinionAttack() method, it it used by minions to launch fire ball attacks in four directions.
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
