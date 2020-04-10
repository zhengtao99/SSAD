using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayerListingsController : MonoBehaviourPunCallbacks
{
    public static OnlinePlayerListingsController Instance;

    [SerializeField]
    private Transform _content;  //content of scroll view
    [SerializeField]
    private OnlinePlayerListing _onlinePlayerListing; //from OnlinePlayerListing.cs of OnlinePlayerListing

    //Store list of OnlinePlayerListing objects
    private List<OnlinePlayerListing> _listings = new List<OnlinePlayerListing>();

    void Awake()
    {
        Instance = this;
    }

    //Change view to CurrentRoom
    public override void OnJoinedRoom()
    {
        //PhotonTestManager.Instance.CurrentRoomUI();

        //Clear room listings
        _content.DestroyChildren();
        _listings.Clear();
    }

    //RoomInfo from Photon.Realtime
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //Removed from roomList
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            else
            {
                //Check if it's new room, then add to list
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    //Instantiate added OnlinePlayerListing into content of scroll view
                    OnlinePlayerListing listing = Instantiate(_onlinePlayerListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info); //SetRoomInfo in OnlinePlayerListing.cs
                        _listings.Add(listing); //add listing into list of OnlinePlayerListings
                    }
                }
            }
        }
    }

    /*
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
        //GameManager.Instance.ShowInvitation();
    }
    */
}
