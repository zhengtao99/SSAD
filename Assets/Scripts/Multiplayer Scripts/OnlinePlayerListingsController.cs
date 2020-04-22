using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This controller class holds the method to model the behaviour of the online player listing page.
/// </summary>
public class OnlinePlayerListingsController : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// A variable that holds OnlinePlayerListingsController instance.
    /// </summary>
    public static OnlinePlayerListingsController Instance;

    [SerializeField]
    /// <summary>
    /// A variable that holds the scroll view.
    /// </summary>
    private Transform _content;  //content of scroll view

    [SerializeField]
    /// <summary>
    /// A variable that holds current script.
    /// </summary>
    private OnlinePlayerListing _onlinePlayerListing; //from OnlinePlayerListing.cs of OnlinePlayerListing

    /// <summary>
    /// A variable that holds the list of online players.
    /// </summary>
    private List<OnlinePlayerListing> _listings = new List<OnlinePlayerListing>();

    /// <summary>
    /// This method is used to instantiate OnlinePlayerListingsController instance to allow other scripts to access the methods define in this class.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /*
    //Change view to CurrentRoom
    public override void OnJoinedRoom()
    {
        //PhotonTestManager.Instance.CurrentRoomUI();

        //Clear room listings
        _content.DestroyChildren();
        _listings.Clear();
    }
    */

    /// <summary>
    /// This method is used to update the online player listing at real time.
    /// </summary>
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
