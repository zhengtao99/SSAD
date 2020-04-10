using Photon.Pun;       //import 
using Photon.Realtime;  //import 
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks  //inherit MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to server...");

        //get NickName
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName; 
        //Get GameVersion
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;

        PhotonNetwork.ConnectUsingSettings();  //connect by photon app id
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
        
    }
}
