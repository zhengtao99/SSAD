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
    public static GameObject loginPage;
    public void Submit()
    {
        loginPage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name == "LoginPage").First();
        var inputFields = loginPage.GetComponentsInChildren<InputField>();
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
        else
        {
            var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
            message.enabled = true;
        }
    }
    public  void ClearText()
    {
        var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
        message.enabled = false;
    }
}
