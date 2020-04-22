using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This controller class holds the method to model the behaviour of the waiting board pop up.
/// </summary>
public class WaitingBoardController : MonoBehaviour
{
    /// <summary>
    /// A variable that holds WaitingBoardController instance to allow other scripts to access the methods in this class.
    /// </summary>
    public static WaitingBoardController Instance;

    /// <summary>
    /// A variable that holds the text in the waiting board pop up.
    /// </summary>
    public Text waitingText;

    /// <summary>
    /// A variable that holds dots in the waiting board pop up, it is used to stimulating loading.
    /// </summary>
    int waitingDots = 0;

    /// <summary>
    /// This method is used to instantiate the WaitingBoardController instance.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This method is used to stimumlate the loading in the waiting board pop up.
    /// </summary>
    private void OnEnable()
    {
        waitingDots = 0;

        InvokeRepeating("setWaitingText", 0, 0.5f);
    }

    /// <summary>
    /// This method is used to set the text in the waiting board pop up.
    /// </summary>
    void setWaitingText()
    {
        waitingDots = ((waitingDots + 1) % 4);
        waitingText.text = "Invitation is sent!\n" +
                            "Waiting for " + PhotonNetwork.CurrentRoom.Name + new string('.', waitingDots);
    }

    /// <summary>
    /// This method is used to close the waiting board pop up when the player clicks cancel or when the opponent player accepts the invitation.
    /// </summary>
    public void stopSettingWaitingText()
    {
        CancelInvoke();
    }
}
