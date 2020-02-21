using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LoginController : MonoBehaviour
{
    public static string username;
    public static string password;
    public void Submit()
    {
        var inputFields = GameObject.FindGameObjectsWithTag("Page").Where(z=>z.name == "LoginPage").First().GetComponentsInChildren<InputField>();
        username = inputFields.Where(z => z.name == "Username").First().text;
        password = inputFields.Where(z => z.name == "Password").First().text;

        StartCoroutine(ConnectionManager.Login(username, password));
    }
    public static void Result(bool result)
    {
        if(result)
        {
            GameManager.Instance.MainMenu();
        }
    }
}
