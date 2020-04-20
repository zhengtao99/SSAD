using Assets.Scripts;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomController : MonoBehaviourPunCallbacks
{
    public static RoomController Instance;
    int count = 0;
    private void Awake()
    {
        Instance = this;
    }

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

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully");
        count = 0;
        GameManager.Instance.HideLoading();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        count++;
        Debug.Log("Room creation failed: " + message.ToString());
        if (count <= 2)  //allow to try to re-create room 2 times only
            CreateRoom();
    }

    public void LeaveRoom()
    {
        //Intentionally leave room: true
        PhotonNetwork.LeaveRoom(true);
    }

    public void SendInvitation(string senderName)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            //Client makes a call to master (RpcTarget.MasterClient) to execute RPC_ChangeReadyState on master
            base.photonView.RPC("RPC_ShowInvitation", RpcTarget.MasterClient, senderName);
        }

    }

    [PunRPC]
    private void RPC_ShowInvitation(string senderName)
    {
        GameManager.Instance.ShowInvitation(senderName);
    }

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
    private void RPC_ShowMultiplayerMatchOtherSide()
    {
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.GetRandomQuestions("Inviter"));
    }

    public void LoadMultiplayerScene()
    {
        //Set room with IsOpen and IsVisible
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
       
        PhotonNetwork.LoadLevel(2); //Load scene index 2: MultiplayerScene

    } 

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
    private void RPC_ForceLeaveRoom()
    {
        Debug.Log("Force leave room");
        GameManager.Instance.HideWaitingBoard();
        LeaveRoom();
    }

    public void OnClickCancelInvitation()
    {
        GameManager.Instance.HideWaitingBoard();

        //Hide invitation on the other side
        base.photonView.RPC("RPC_HideInvitation", RpcTarget.Others);

        //Then leave the room
        LeaveRoom();
    }

    [PunRPC]
    private void RPC_HideInvitation()
    {
        GameManager.Instance.HideInvitation();
    }

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
    private void RPC_ScoreUpdateOtherSide(int playerNumber, string nickname, int score)
    {
        var mazePage = MultiplayerSceneManager.Instance.playPage;
        Text scoreText = mazePage.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText_Player" + playerNumber.ToString()).First();
        scoreText.text = nickname + "\n" + score.ToString();
    }

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
    private void RPC_LifeUpdateOtherSide(int playerNumber, int countLife)
    {
        GameObject[] heartArr = GameObject.FindGameObjectsWithTag("Heart_player" + playerNumber.ToString());
        Destroy(heartArr[countLife]);
    }

    public void MiniChestUpdateOtherSide(float x, float y, float z)
    {
        base.photonView.RPC("RPC_MiniChestUpdateOtherSide", RpcTarget.Others, x, y, z);
    }

    [PunRPC]
    private void RPC_MiniChestUpdateOtherSide(float x, float y, float z)
    {
        MiniChestController.OpenedChestInstance.CreateOpenedChests(x,y,z);
    }

    public void GameOverPopUp()
    {
        MultiplayerSceneManager.Instance.GameOverPopUp();
        base.photonView.RPC("RPC_GameOverPopUp", RpcTarget.Others);

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(CloseGameOverPopUp());
    }

    [PunRPC]
    private void RPC_GameOverPopUp()
    {
        MultiplayerSceneManager.Instance.GameOverPopUp();

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(CloseGameOverPopUp());
    }

    IEnumerator CloseGameOverPopUp()
    {
        yield return new WaitForSeconds(4.0f);
        PhotonNetwork.LoadLevel(1); //Go back ModePage
    }
}
