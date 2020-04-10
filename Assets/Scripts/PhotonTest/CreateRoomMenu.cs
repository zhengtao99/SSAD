using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomMenu : MonoBehaviourPunCallbacks  //inherit MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName; //Text input for room name

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected) //if not connected
            return;

        //CreateRoom
        //JoinOrCreateRoom
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true; //broadcast CustomProperties
        options.PublishUserId = true;
        options.MaxPlayers = 2;  //max players: 2

        //If exist room -> join, otherwise create
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully");
        PhotonTestManager.Instance.CurrentRoomUI();  //Change to CurrentRoom
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message.ToString());
    }
}

