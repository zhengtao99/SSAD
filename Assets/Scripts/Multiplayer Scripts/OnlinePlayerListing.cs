using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This entity class contains the attributes and behaviour of a online player listing page.
/// </summary>
public class OnlinePlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    /// <summary>
    /// A Text object in the listing page.
    /// </summary>
    private Text _name;  //Text of RoomListing

    /// <summary>
    /// A boolean variable to check if the player has entered the room.
    /// </summary>
    private bool joinedRoom = false;

    //RoomInfo class from Photon.Realtime
    /// <summary>
    /// The method is used at realtime to join photonnetwork or to retrieve the number of players in the list at real time.
    /// </summary>
    public RoomInfo RoomInfo { get; private set; }

    /// <summary>
    /// The method is used to set room text.
    /// </summary>
    public void SetRoomInfo(RoomInfo roomInfo) //set text for RoomListing
    {
        RoomInfo = roomInfo;
        _name.text = RoomInfo.Name;
    }

    /// <summary>
    /// This method is used send invitation on click battle button in the online player listing page.
    /// </summary>
    public void OnClickSendInvitaion()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
        joinedRoom = true;
    }

    /// <summary>
    /// This method is used to detect if the players are not in the online player listing page, they will be able to receive the invitations.
    /// </summary>
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

    /// <summary>
    /// This method will return an error message is the game client fails to join the photon network.
    /// </summary>
    /// <param name="message">The return message</param>
    /// <param name="returnCode">The return error code</param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (joinedRoom != null)
        {
            joinedRoom = false;
        }
    }
}

