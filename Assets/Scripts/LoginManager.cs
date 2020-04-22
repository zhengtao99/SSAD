using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This manager class holds all the methods neccessary show and hide loading for successful Learnable Web API requests to retrieve/update data into the database.
/// </summary
public class LoginManager : MonoBehaviour
{
    /// <summary>
    /// A variable that holds the LoginManager instance to allow other scripts to access the method in this class.
    /// </summary
    public static LoginManager Instance;

    /// <summary>
    /// A variable that holds the loading game object.
    /// </summary
    public GameObject loading;

    /// <summary>
    /// A variable that holds the login page game object.
    /// </summary
    public GameObject loginPage;

    /// <summary>
    /// This method is instantiate LoginManager class instance to allow other scripts to access the methods define in this class.
    /// </summary
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This method is used to show the loading gameobject.
    /// </summary
    public void ShowLoading()
    {
        loading.SetActive(true);
    }

    /// <summary>
    /// This method is used to hide the loading gameobject.
    /// </summary
    public void HideLoading()
    {
        loading.SetActive(false);
    }
}
