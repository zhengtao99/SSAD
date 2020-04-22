using Assets.Scripts;
using Assets.Scripts.Multiplayer_Scripts;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This controller class holds the methods with photonnetwork connections, as well as the different ways of handling inivitations between 2 players.
/// </summary>
public class RoomController : MonoBehaviourPunCallbacks
{

    /// <summary>
    /// A variable holding the RoomController Instance.
    /// </summary>
    public static RoomController Instance;

    /// <summary>
    /// A counter variable.
    /// </summary>
    int count = 0;

    /// <summary>
    /// This method is used to instantiate RoomController instance to allow other scripts to access the methods define in this class.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This method is used to create a connection into the photon network.
    /// </summary>
    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)  //if not connected 
        {
            Debug.Log("PhotonNetwork is not connected");
            return;
        }
        GameManager.Instance.ShowLoading();
        //PhotonNetwork.NickName = username;
        Debug.Log("PhotonNetwork.NickName: " + PhotonNetwork.NickName);
        Debug.Log("Creating a room...");

        //CreateRoom
        //JoinOrCreateRoom
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true; //broadcast CustomProperties
        options.PublishUserId = true;
        options.MaxPlayers = 2;  //max players: 2

        //If exist room -> join, otherwise create
        //PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.NickName, options, TypedLobby.Default);
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, options, TypedLobby.Default);
    }

    /// <summary>
    /// This method is used to display successful message when the player's game client creates a room.
    /// </summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully");
        count = 0;
        GameManager.Instance.HideLoading();
    }

    /// <summary>
    /// This method is used to display error message when the player's game client fails to create a room.
    /// </summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        count++;
        Debug.Log("Room creation failed: " + message.ToString());
        if (count <= 2)  //allow to try to re-create room 2 times only
            CreateRoom();
    }

    /// <summary>
    /// This method is used to leave the photon network.
    /// </summary>
    public void LeaveRoom()
    {
        //Intentionally leave room: true
        PhotonNetwork.LeaveRoom(true);
    }

    /// <summary>
    /// This method is used to send the invitation request using RPC_ShowInvitation() method.
    /// </summary>
    /// <param name="senderName">The master/sender player's name</param>
    public void SendInvitation(string senderName)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            //Client makes a call to master (RpcTarget.MasterClient) to execute RPC_ChangeReadyState on master
            base.photonView.RPC("RPC_ShowInvitation", RpcTarget.MasterClient, senderName);
        }

    }

    [PunRPC]
    /// <summary>
    /// This method is used show invitation request.
    /// </summary>
    /// <param name="senderName">The master/sender player's name</param>
    private void RPC_ShowInvitation(string senderName)
    {
        GameManager.Instance.ShowInvitation(senderName);
    }

    /// <summary>
    /// This method is used to accept the invitation request using RPC_ShowMultiplayerMatchOtherSide() method.
    /// </summary>
    public void OnClickAcceptInvitation()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.MultiplayerMatchUI();
            base.photonView.RPC("RPC_ShowMultiplayerMatchOtherSide", RpcTarget.Others);
            ConnectionManager cm = new ConnectionManager();
            StartCoroutine(cm.GetRandomQuestions("Invitee"));
        }
    }

    [PunRPC]
    /// <summary>
    /// This method is used to display Match up screen.
    /// </summary>
    private void RPC_ShowMultiplayerMatchOtherSide()
    {
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.GetRandomQuestions("Inviter"));
    }

    /// <summary>
    /// This method is used to go into multiplayer maze environment
    /// </summary>
    public void LoadMultiplayerScene()
    {
        //Set room with IsOpen and IsVisible
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
       
        PhotonNetwork.LoadLevel(2); //Load scene index 2: MultiplayerScene

    }

    /// <summary>
    /// This method is used to reject the invitation request using RPC_ForceLeaveRoom() method.
    /// </summary>
    public void OnClickDeclineInvitation()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.HideInvitation();
   
            //Force the other player to leave the room
            base.photonView.RPC("RPC_ForceLeaveRoom", RpcTarget.Others);
        }
    }

    [PunRPC]
    /// <summary>
    /// This method is used when an invitation has been rejected, waiting board will be closed and a reject message will be shown.
    /// </summary>
    private void RPC_ForceLeaveRoom()
    {
        Debug.Log("Force leave room");
        GameManager.Instance.HideWaitingBoard();
        LeaveRoom();
        GameManager.Instance.invitation.SetActive(true);
        GameObject.Find("Invitation").GetComponent<InvitationController>().Hide();
        GameManager.Instance.InvitationText.text = PhotonNetwork.NickName + " has declined your invitation.";
        Invoke("RPC_HideInvitation", 2f);
    }

    /// <summary>
    /// This method is used to cancel invitation on the opponent side using RPC_HideInvitation() method.
    /// </summary>
    public void OnClickCancelInvitation()
    {
        GameManager.Instance.HideWaitingBoard();

        //Hide invitation on the other side
        base.photonView.RPC("RPC_HideInvitation", RpcTarget.Others);
        
        //Then leave the room
        LeaveRoom();
    }

    [PunRPC]
    /// <summary>
    /// This method is used to hide game invitation.
    /// </summary>
    private void RPC_HideInvitation()
    { 
        GameManager.Instance.HideInvitation();
    }

    /// <summary>
    /// This method is used to update current player score at the opponent side using RPC_ScoreUpdateOtherSide() method.
    /// </summary>
    public void ScoreUpdateOtherSide(int score)
    {
        int playerNumber;
        if (!PhotonNetwork.IsMasterClient)  //player 1
        {
            playerNumber = 1;
        } else
        {
            playerNumber = 2;
        }
        base.photonView.RPC("RPC_ScoreUpdateOtherSide", RpcTarget.Others, playerNumber, PhotonNetwork.NickName, score);
    }

    [PunRPC]
    /// <summary>
    /// This method is used to update player scores.
    /// </summary>
    private void RPC_ScoreUpdateOtherSide(int playerNumber, string nickname, int score)
    {
        var mazePage = MultiplayerSceneManager.Instance.playPage;
        Text scoreText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText_Player" + playerNumber.ToString()).First();
        scoreText.text = nickname + "\n" + score.ToString();
    }

    /// <summary>
    /// This method is used to update current player live at the opponent side using RPC_LifeUpdateOtherSide() method.
    /// </summary>
    public void LifeUpdateOtherSide(int countLife)
    {
        int playerNumber;
        if (!PhotonNetwork.IsMasterClient)  //player 1
        {
            playerNumber = 1;
        }
        else
        {
            playerNumber = 2;
        }
        base.photonView.RPC("RPC_LifeUpdateOtherSide", RpcTarget.Others, playerNumber, countLife);
    }

    [PunRPC]
    /// <summary>
    /// This method is used to update player lives.
    /// </summary>
    private void RPC_LifeUpdateOtherSide(int playerNumber, int countLife)
    {
        GameObject[] heartArr = GameObject.FindGameObjectsWithTag("Heart_player" + playerNumber.ToString());
        Destroy(heartArr[countLife]);
    }

    /// <summary>
    /// This method is used to open the mini chest at the slave side using RPC_MiniChestUpdateotherSide() method.
    /// </summary>
    public void MiniChestUpdateOtherSide(float x, float y, float z)
    {
        base.photonView.RPC("RPC_MiniChestUpdateOtherSide", RpcTarget.Others, x, y, z);
    }

    [PunRPC]
    /// <summary>
    /// This method is used to open the mini chest.
    /// </summary>
    private void RPC_MiniChestUpdateOtherSide(float x, float y, float z)
    {
        MiniChestController.OpenedChestInstance.CreateOpenedChests(x,y,z);
    }

    /// <summary>
    /// This method is used to create game over pop up at both master and slave sides using RPC_GameOverPopUp() method.
    /// </summary>
    public void GameOverPopUp()
    {
        MultiplayerSceneManager.Instance.GameOverPopUp();
        base.photonView.RPC("RPC_GameOverPopUp", RpcTarget.Others);

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(CloseGameOverPopUp());
    }

    [PunRPC]
    /// <summary>
    /// This method is used to create game over pop up.
    /// </summary>
    private void RPC_GameOverPopUp()
    {
        MultiplayerSceneManager.Instance.GameOverPopUp();

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(CloseGameOverPopUp());
    }

    /// <summary>
    /// This method is used to close game over pop up after 4 seconds.
    /// </summary>
    IEnumerator CloseGameOverPopUp()
    {
        yield return new WaitForSeconds(4.0f);
        PhotonNetwork.LoadLevel(1); //Go back ModePage
    }
}
