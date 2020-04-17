using Photon.Pun;
using UnityEngine;

public class LeaveRoomMenu : MonoBehaviour
{
    public void OnClick_LeaveRoom()
    {
        //Intentionally leave room: true
        PhotonNetwork.LeaveRoom(true);

        //Go back ViewRooms
        PhotonTestManager.Instance.ViewRoomsUI();
    }
}

