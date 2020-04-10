using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviourPunCallbacks
{
    public static RoomController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateRoom(string username)
    {
        if (!PhotonNetwork.IsConnected)  //if not connected 
        {
            Debug.Log("PhotonNetwork is not connected");
            return;
        }

        PhotonNetwork.NickName = username;
        Debug.Log("PhotonNetwork.NickName: " + PhotonNetwork.NickName);

        //CreateRoom
        //JoinOrCreateRoom
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true; //broadcast CustomProperties
        options.PublishUserId = true;
        options.MaxPlayers = 2;  //max players: 2

        //If exist room -> join, otherwise create
        PhotonNetwork.JoinOrCreateRoom(username, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message.ToString());
    }

    public void LeaveRoom()
    {
        //Intentionally leave room: true
        PhotonNetwork.LeaveRoom(true);
    }

    public void SendInvitation()
    {
        Debug.Log("In SendInvitation");
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Call RPC_ShowInvitation");
            //Client makes a call to master (RpcTarget.MasterClient) to execute RPC_ChangeReadyState on master
            base.photonView.RPC("RPC_ShowInvitation", RpcTarget.MasterClient);
        }

    }

    [PunRPC]
    private void RPC_ShowInvitation()
    {
        Debug.Log("In RPC_ShowInvitation");
        GameManager.Instance.ShowInvitation();
    }

    public void OnClickAcceptInvitation()
    {
        if (PhotonNetwork.IsMasterClient)
            GameManager.Instance.HideInvitation();
    }

    public void OnClickCancelInvitation()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.HideInvitation();

            //Force the other player to leave the room
            base.photonView.RPC("RPC_ForceLeaveRoom", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void RPC_ForceLeaveRoom()
    {
        Debug.Log("Force leave room");
        LeaveRoom();
    }
}
