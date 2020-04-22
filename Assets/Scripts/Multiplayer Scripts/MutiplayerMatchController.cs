using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This controller class holds the method responsible for matching the master and slave clients to enable multiplayer match up.
/// </summary>
public class MutiplayerMatchController : MonoBehaviour
{

    /// <summary>
    /// A variable holding player 1's name.
    /// </summary>
    public Text player1_name;

    /// <summary>
    /// A variable holding player 2's name.
    /// </summary>
    public Text player2_name;

    /// <summary>
    /// A variable holding the master client game object.
    /// </summary>
    public GameObject masterClient;


    /// <summary>
    /// A variable holding the slave client game object.
    /// </summary>
    public GameObject client;


    /// <summary>
    /// This method is called before the first frame update, it is used to set the text in the multiplayer match up page.
    /// </summary>
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
