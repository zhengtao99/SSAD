using Assets.Scripts;
using Photon.Pun;       //import 
using Photon.Realtime;  //import 
using UnityEngine;

/// <summary>
/// This multiplayer tester class holds the methods required to test if the players are connected to the online listing player page to be able to view each other.
/// </summary>
public class TestConnect : MonoBehaviourPunCallbacks  //inherit MonoBehaviourPunCallbacks
{
    /// <summary>
    /// A boolean variable to check if the player has created a connection prior to this instance.
    /// </summary>
    private bool isFirstTime = true;

    /// <summary>
    /// This method is called before the first frame update, it is used to create a connection to other players.
    /// </summary>
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            GameManager.Instance.ShowLoading();
            Debug.Log("Connecting to server...");

            //Sync for both players when load scene LoadLevel
            PhotonNetwork.AutomaticallySyncScene = true;

            //get NickName
            //PhotonNetwork.NickName = MasterManager.GameSettings.NickName; 
            PhotonNetwork.NickName = ConnectionManager.user.FirstName;

            //Get GameVersion
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;

            PhotonNetwork.ConnectUsingSettings();  //connect by photon app id
        }
        else
        {
            //if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name != PhotonNetwork.NickName) //In other player's room
            if (PhotonNetwork.CurrentRoom != null)
            {
                PhotonNetwork.NickName = ConnectionManager.user.FirstName;
                RoomController.Instance.LeaveRoom();
                //RoomController.Instance.CreateRoom(ConnectionManager.user.FirstName);
            }
        }
    }

    /// <summary>
    /// This method is make players appear on the player listing page when the master player enters the multiplayer invitation lobby.
    /// </summary>
    public override void OnConnectedToMaster() //When connect to photon
    {
        Debug.Log("Connected to server");

        //Print nickname of LocalPlayer

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

    }

    /// <summary>
    /// This method is used to return warning messages in case of unsuccessful photon connections.
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)  //When disconnect to photon
    {
        Debug.Log("Disconnected from server for reason: " + cause.ToString());
        GameManager.Instance.warningInModePage.SetActive(true);
        GameManager.Instance.HideLoading();
    }

    /// <summary>
    /// This method is used to create a master invitation lobby.
    /// </summary>
    public override void OnJoinedLobby()
    {
        if (isFirstTime)
        {
            base.OnJoinedLobby();
            RoomController.Instance.CreateRoom();
            isFirstTime = false;
        }
    }
}
