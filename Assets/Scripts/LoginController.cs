using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class LoginController : MonoBehaviourPunCallbacks
{
    private static string nickname = "";
   
    public static GameObject loginPage;

    void Start()
    {
        FindObjectOfType<SoundManager>().Play("GameLaunch");
    }

    public void Submit()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        loginPage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name == "LoginPage").First();
        var inputFields = loginPage.GetComponentsInChildren<InputField>();
        var username = inputFields.Where(z => z.name == "Username").First().text;
        var password = inputFields.Where(z => z.name == "Password").First().text;
        nickname = username;
        StartCoroutine(ConnectionManager.Login(username, password));
    }
    public static void Result(bool result)
    {
        if(result)
        {
            Debug.Log("result: " + result);
            //ConnectToPhoton(nickname);
            GameManager.Instance.WorldUI();
        }
        else
        {
            var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
            message.enabled = true;
        }
    }
    public void ClearText()
    {
        var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
        message.enabled = false;
    }

    /*
    private static void ConnectToPhoton(string nickname)
    {
        Debug.Log("Connecting to server...");

        PhotonNetwork.NickName = nickname;
        PhotonNetwork.GameVersion = "0.0.0";

        PhotonNetwork.ConnectUsingSettings();  //connect by photon app id
    }

    public override void OnConnectedToMaster() //When connect to photon
    {
        Debug.Log("Connected to server");

        //Print nickname of LocalPlayer
        Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName); //print nickname

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)  //When disconnect to photon
    {
        Debug.Log("Disconnected from server for reason: " + cause.ToString());
    }

    
    public override void OnJoinedLobby()
    {
        Debug.Log("hello");
        GameManager.Instance.WorldUI();
    }
    */
}
