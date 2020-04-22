using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// This controller class holds all the methods neccessary to authenticate a player at the login page.
/// </summary
public class LoginController : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// A variable that duplicates the player's user id that will be passed to other scripts.
    /// </summary
    private static string nickname = "";

    /// <summary>
    /// A variable that holds the login page game object.
    /// </summary
    public static GameObject loginPage;

    /// <summary>
    /// This method is used to play the game launch sound effects.
    /// </summary
    void Start()
    {
        FindObjectOfType<SoundManager>().Play("GameLaunch");
    }

    /// <summary>
    /// This method is called when the player selected login, it will proceed to verify the players username and password against the database.
    /// </summary
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

    /// <summary>
    /// This method is responsible for the changing to world page on successful login.
    /// </summary
    public static void Result(string result)
    {
        
        var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
        message.enabled = true;
        message.text = result;

    }

    /// <summary>
    /// This method is responsible for clearing all check boxes on unsuccessful login.
    /// </summary
    public void ClearText()
    {
        var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
        message.enabled = false;
    }

}
