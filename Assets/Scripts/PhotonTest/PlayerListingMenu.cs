using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerListingMenu : MonoBehaviourPunCallbacks //inherit MonoBehaviourPunCallbacks 
{
    [SerializeField]
    private Transform _content;  //content of scroll view
    [SerializeField]
    private PlayerListing _playerListing; //from PlayerListing.cs of PlayerListing
    [SerializeField]
    private Text _readyUpText;

    //Store list of PlayerListing objects
    private List<PlayerListing> _listings = new List<PlayerListing>();

    private bool _ready = false;

    public override void OnEnable()
    {
        base.OnEnable();
        SetReadyUp(false);
        GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _listings.Count; i++)
            Destroy(_listings[i].gameObject);

        _listings.Clear();
    }

    private void SetReadyUp(bool state)
    {
        _ready = state;
        if (_ready)
            _readyUpText.text = "Ready";
        else
            _readyUpText.text = "Not ready";
    }

    public override void OnLeftRoom()
    {
        //Use DestroyChildren from Transforms.cs
        _content.DestroyChildren();
    }

    private void GetCurrentRoomPlayers()
    {
        //Check before get current room players
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        //Get dictionary of players in the room you are in
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
        //Check whether player exist in the list
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            //Instantiate a PlayerListing prefab into content of scroll view
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player); //set text for PlayerListing
                _listings.Add(listing);  //add new player to the list of PlayerListings
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Find index of the player that left the room
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public void OnClick_StartGame()
    {
        //Only master can start game
        if (PhotonNetwork.IsMasterClient)
        {
            //Check whether all players are ready
            for (int i = 0; i < _listings.Count; i++)
            {
                if (_listings[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (!_listings[i].Ready)
                        return;
                }
            }

            //Set room with IsOpen and IsVisible
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1); //Load scene index 1
        }
    }

    public void OnClick_ReadyUp()
    {
        //Only clients can click ready button
        if (!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!_ready);

            //Client makes a call to master (RpcTarget.MasterClient) to execute RPC_ChangeReadyState on master
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);
        }
    }

    //Call for the target RpcTarget.MasterClient -> only update ready on master side
    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
            _listings[index].Ready = ready;
    }

    //When the master leaves the room, players also leave the room
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonTestManager.Instance.CurrentRoom.GetComponent<LeaveRoomMenu>().OnClick_LeaveRoom();
    }
}

