using Assets.Scripts;
using Photon.Pun;       //import 
using Photon.Realtime;  //import 
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks  //inherit MonoBehaviourPunCallbacks
{
    private bool isFirstTime = true;

    // Start is called before the first frame update
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
                RoomController.Instance.LeaveRoom();
                //RoomController.Instance.CreateRoom(ConnectionManager.user.FirstName);
            }
        }
    }

    public override void OnConnectedToMaster() //When connect to photon
    {
        Debug.Log("Connected to server");

        //Print nickname of LocalPlayer
        Debug.Log(PhotonNetwork.LocalPlayer.NickName); //print nickname

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        

    }

    public override void OnDisconnected(DisconnectCause cause)  //When disconnect to photon
    {
        Debug.Log("Disconnected from server for reason: " + cause.ToString());
        GameManager.Instance.warningInModePage.SetActive(true);
        GameManager.Instance.HideLoading();
    }

    
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        if (isFirstTime)
        {
            Debug.Log("First time: OnJoinedLobby");
            base.OnJoinedLobby();
            RoomController.Instance.CreateRoom();
            isFirstTime = false;
        }
    }
    
}
