using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class OnlinePlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _name;  //Text of RoomListing

    private bool joinedRoom = false;

    //RoomInfo class from Photon.Realtime
    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo) //set text for RoomListing
    {
        RoomInfo = roomInfo;
        _name.text = RoomInfo.Name;
    }
    
    
    public void OnClickSendInvitaion()
    {
        //Join the room by its name
        PhotonNetwork.JoinRoom(RoomInfo.Name);
        joinedRoom = true;
        //InvitationController.Instance.SendInvitation();
        //OnlinePlayerListingsController.Instance.SendInvitation();
    }

    
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.Name != PhotonNetwork.NickName)
            RoomController.Instance.SendInvitation(PhotonNetwork.NickName);
    }
    

    /*
    [PunRPC]
    private void RPC_ShowInvitation()
    {
        GameManager.Instance.ShowInvitation();
    */

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (joinedRoom != null)
        {
            joinedRoom = false;
        }
    }
}

