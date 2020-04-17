using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _text;  //Text of PlayerListing

    //Player class from Photon.Realtime
    public Player Player { get; private set; }

    //Ready state for each RoomListing
    public bool Ready = false;

    public void SetPlayerInfo(Player player) //set text for PlayerListing
    {
        Player = player;
        SetPlayerText(player);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (targetPlayer != null && targetPlayer == Player)
        {
            if (changedProps.ContainsKey("RandomNumber"))
                SetPlayerText(targetPlayer);
        }
    }

    private void SetPlayerText(Player player)
    {
        //Get CustomProperties from Hashtable 
        int result = -1;
        if (player.CustomProperties.ContainsKey("RandomNumber"))
            result = (int)player.CustomProperties["RandomNumber"];

        _text.text = player.NickName + " (CustomProperties: " + result + ")";
    }
}
