using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhotonPlayerController : MonoBehaviour
{
    private PhotonView PV;
    public Transform spawnPoint_1;
    public Transform spawnPoint_2;
    public Transform spawnLivesPoint_1;
    public Transform spawnLivesPoint_2;

    public GameObject myPlayer;

    // Start is called before the first frame update
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
            myPlayer = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player_m"),
                t_player.position, t_player.rotation, 0);
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", livesPrefabName),
                t_lives.position, t_lives.rotation, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
