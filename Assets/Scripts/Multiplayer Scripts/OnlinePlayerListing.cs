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

    //Click battle_button on OnlinePlayerListing
    public void OnClickSendInvitaion()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
        joinedRoom = true;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.Name != PhotonNetwork.NickName)
        {
            RoomController.Instance.SendInvitation(PhotonNetwork.NickName);
            GameManager.Instance.ShowWaitingBoard();
        }
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

