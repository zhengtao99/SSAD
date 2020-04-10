using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;  //Text of RoomListing

    //RoomInfo class from Photon.Realtime
    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo) //set text for RoomListing
    {
        RoomInfo = roomInfo;
        _text.text = RoomInfo.Name + " (Max: " + roomInfo.MaxPlayers + ")";
    }

    public void OnClick_Button()
    {
        //Join the room by its name
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
