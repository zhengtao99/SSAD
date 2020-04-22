using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The controller class is holding the method that is used to keep track of the player logged in particulars, in order to identify the player throughout the game.
/// </summary
public class ProfileController : MonoBehaviour
{
    /// <summary>
    /// This method is called before the first frame update, to display the player's name in every page.
    /// </summary
    void Start()
    {
        var profilePage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("profile")).First();
        var usernameText = profilePage.GetComponentsInChildren<Text>().Where(z => z.name.ToLower().Contains("username")).First();
        usernameText.text = ConnectionManager.user.FirstName + " " + ConnectionManager.user.LastName;
    }

    /// <summary>
    /// This method is called when the player clicks on the profile name, a drop down will be shown.
    /// </summary
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

    /// <summary>
    /// This method is called when the player clicks on the logout button in the dropdown, the player will be logged out.
    /// </summary
    public void Logout()
    {
        GameManager.Instance.Logout();
    }
}
