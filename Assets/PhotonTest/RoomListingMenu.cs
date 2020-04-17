using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks //inherit MonoBehaviourPunCallbacks 
{
    [SerializeField]
    private Transform _content;  //content of scroll view
    [SerializeField]
    private RoomListing _roomListing; //from RoomListing.cs of RoomListing

    //Store list of RoomListing objects
    private List<RoomListing> _listings = new List<RoomListing>();

    //Change view to CurrentRoom
    public override void OnJoinedRoom()
    {
        PhotonTestManager.Instance.CurrentRoomUI();

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
                    //Instantiate added RoomListing into content of scroll view
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info); //SetRoomInfo in RoomListing.cs
                        _listings.Add(listing); //add listing into list of RoomListings
                    }
                }
            }
        }
    }
}

