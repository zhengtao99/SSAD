using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Assets.Model;

public class LoginController : MonoBehaviour
{
    public static User user = new User();
    public static GameObject loginPage;
    public void Submit()
    {
        loginPage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name == "LoginPage").First();
        var inputFields = loginPage.GetComponentsInChildren<InputField>();
        user.Username = inputFields.Where(z => z.name == "Username").First().text;
        user.Password = inputFields.Where(z => z.name == "Password").First().text;
        if (Validate(user.Username, user.Password))
        {
            StartCoroutine(ConnectionManager.Login(user.Username, user.Password));
        }
    }
    public static void Result(bool result)
    {
        if (result)
        {
            GameManager.Instance.MainMenu();
        }
        else
        {
            var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
            message.text = "Invalid username or password.";
            message.enabled = true;
        }
    }
    public void ClearText()
    {
        var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
        message.enabled = false;
    }
    public bool Validate(string username, string password)
    {
        if (username == "")
        {
            var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
            message.text = "Please enter your username.";
            message.enabled = true;
        }
        else if (password == "")
        {
            var message = loginPage.GetComponentsInChildren<Text>().Where(z => z.name == "Message").First();
            message.text = "Please enter your password.";
            message.enabled = true;
        }
        else
        {
            return true;
        }
        return false;

    }
}
