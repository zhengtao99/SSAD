using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var profilePage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("profile")).First();
        var usernameText = profilePage.GetComponentsInChildren<Text>().Where(z => z.name.ToLower().Contains("username")).First();
        usernameText.text = ConnectionManager.user.FirstName + " " + ConnectionManager.user.LastName;
    }
    public void ProfileCLick()
    {
        if (GameManager.Instance.btnLogout.active == true)
        {
            GameManager.Instance.btnLogout.SetActive(false);
        }
        else
        {
            GameManager.Instance.btnLogout.SetActive(true);
        }
    }
    public void Logout()
    {
        GameManager.Instance.Logout();
    }
}
