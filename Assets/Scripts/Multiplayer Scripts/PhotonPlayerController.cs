using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This controller class holds the method to generate two views for the players to play in the same maze environment.
/// </summary>
public class PhotonPlayerController : MonoBehaviour
{
    /// <summary>
    /// A PhotonView variable to store a copy of the maze environment.
    /// </summary>
    private PhotonView PV;

    /// <summary>
    /// A variable that holds the spawn point for player 1.
    /// </summary>
    public Transform spawnPoint_1;

    /// <summary>
    /// A variable that holds the spawn point for player 2.
    /// </summary>
    public Transform spawnPoint_2;

    /// <summary>
    /// A variable that holds the spawn life points for player 1.
    /// </summary>
    public Transform spawnLivesPoint_1;

    /// <summary>
    /// A variable that holds the spawn life points for player 1.
    /// </summary>
    public Transform spawnLivesPoint_2;

    /// <summary>
    /// A variable that holds the current player in the game client
    /// </summary>
    public GameObject myPlayer;

    /// <summary>
    /// This method is called before the first frame update, it will instantiate the copy of the maze environment, spawns the players and start their lives. The players are differentiate by master and slave game clients.
    /// </summary>
    void Start()
    {
        PV = GetComponent<PhotonView>();

        Transform t_player, t_lives;
        string livesPrefabName;
        if (!PhotonNetwork.IsMasterClient)
        {
            t_player = spawnPoint_1;
            t_lives = spawnLivesPoint_1;
            livesPrefabName = "Lives_m1";
        }
        else
        {
            t_player = spawnPoint_2;
            t_lives = spawnLivesPoint_2;
            livesPrefabName = "Lives_m2";
        }

        if (PV.IsMine)
        {

            if (PhotonNetwork.IsMasterClient)
            {
                myPlayer = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player_em"),
                    t_player.position, t_player.rotation, 0);
            }
            else
            {
                myPlayer = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player_m"),
                    t_player.position, t_player.rotation, 0);
            }
            
            JoyStickMulController.Instance.player = myPlayer;

            JoyStickMulController.Instance.playerController = myPlayer.GetComponent<MyPlayerController>();

            MultiplayerSceneManager.Instance.myPlayer = myPlayer;

            PhotonNetwork.Instantiate(Path.Combine("Prefabs", livesPrefabName),
                t_lives.position, t_lives.rotation, 0);
        }
    }

}
