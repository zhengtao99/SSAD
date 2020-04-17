using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutiplayerMatchController : MonoBehaviour
{
    public Text player1_name;
    public Text player2_name;
    public GameObject masterClient;
    public GameObject client;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            player1_name.text = GameManager.Instance.invitationSender;
        }
        else
        {
            player1_name.text = PhotonNetwork.NickName;
        }
        player2_name.text = PhotonNetwork.CurrentRoom.Name;
    }
}
