using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;       //import 
using Photon.Realtime;  //import 

public class MultiPlayerResultController : MonoBehaviour
{
    public Text winnerName;
    public Text winnerScore;
    public Text loserName;
    public Text loserScore;
    public Text player1Score;
    public Text player2Score;
    string[] player1Array = null, player2Array = null;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (player1Score != null && player2Score != null)
        {
            player1Array = player1Score.text.Split(char.Parse("\n"));
            player2Array = player2Score.text.Split(char.Parse("\n"));

            if (Int32.Parse(player1Array[1]) < Int32.Parse(player2Array[1]))
            {
                winnerName.text = player2Array[0];
                winnerScore.text = player2Array[1];
                loserName.text = player1Array[0];
                loserScore.text = player1Array[1];
            }
            else
            {
                winnerName.text = player1Array[0];
                winnerScore.text = player1Array[1];
                loserName.text = player2Array[0];
                loserScore.text = player2Array[1];
            }
        }
    }

    // Update is called once per frame
    public void CloseMultiplayerResultPopUp()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        SceneManager.LoadScene(1);
    }
}
