using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// The controller class holds all the methods neccessary to authenticate a player at the login page.
/// </summary
public class LoginController : MonoBehaviour
{

    /// <summary>
    /// A variable that holds the login page game object.
    /// </summary
    public static GameObject loginPage;

    /// <summary>
    /// The method is called when the player selected login, it will proceed to verify the players username and password against the database.
    /// </summary
    public void Submit()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        loginPage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name == "LoginPage").First();
        var inputFields = loginPage.GetComponentsInChildren<InputField>();
        var username = inputFields.Where(z => z.name == "Username").First().text;
        var password = inputFields.Where(z => z.name == "Password").First().text;

        StartCoroutine(ConnectionManager.Login(username, password));
    }

    /// <summary>
    /// The method is responsible for the changing to world page on successful login.
    /// </summary
    public static void Result(bool result)
    {
        if(result)
        {
            GameManager.Instance.WorldUI();
        }
        else
        {
            var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
            message.enabled = true;
        }
    }

    /// <summary>
    /// The method is responsible for clearing all check boxes on unsuccessful login.
    /// </summary
    public void ClearText()
    {
        var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
        message.enabled = false;
    }
}
