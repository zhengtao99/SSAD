using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class is holding the method that is used to keep track of the player logged in particulars, in order to identify the player throughout the game.
/// </summary
public class ProfileController : MonoBehaviour
{
    /// <summary>
    /// The method is called before the first frame update, to display the player's name in every page.
    /// </summary
    void Start()
    {
        var profilePage = GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("profile")).First();
        var usernameText = profilePage.GetComponentsInChildren<Text>().Where(z => z.name.ToLower().Contains("username")).First();
        usernameText.text = ConnectionManager.user.FirstName + " " + ConnectionManager.user.LastName;
    }
}
