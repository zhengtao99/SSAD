using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingBoardController : MonoBehaviour
{
    public static WaitingBoardController Instance;
    public Text waitingText;
    int waitingDots = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        waitingDots = 0;

        InvokeRepeating("setWaitingText", 0, 0.5f);
    }

    void setWaitingText()
    {
        waitingDots = ((waitingDots + 1) % 4);
        waitingText.text = "Invitation is sent!\n" +
                            "Waiting for " + PhotonNetwork.CurrentRoom.Name + new string('.', waitingDots);
    }

    public void stopSettingWaitingText()
    {
        CancelInvoke();
    }
}
