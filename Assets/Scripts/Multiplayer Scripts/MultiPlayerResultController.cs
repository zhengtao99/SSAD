using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;       //import 
using Photon.Realtime;  //import 


/// <summary>
/// This controller class holds the method responsible for displaying the multiplayer result page.
/// </summary>
public class MultiPlayerResultController : MonoBehaviour
{

    /// <summary>
    /// A variable holding the winner name.
    /// </summary>
    public Text winnerName;
    /// <summary>
    /// A variable holding the winner score.
    /// </summary>
    public Text winnerScore;
    /// <summary>
    /// A variable holding the loser name.
    /// </summary>
    public Text loserName;
    /// <summary>
    /// A variable holding the loser score.
    /// </summary>
    public Text loserScore;

    /// <summary>
    /// A variable holding player 1's score.
    /// </summary>
    public Text player1Score;

    /// <summary>
    /// A variable holding player 2's score.
    /// </summary>
    public Text player2Score;

    /// <summary>
    /// An array holding player 1's name and score in a string
    /// </summary>
    string[] player1Array = null;

    /// <summary>
    /// An array holding player 2's name and score in a string
    /// </summary>
    string[] player2Array = null;

    /// <summary>
    /// This method is used to set the winner and loser names and scores in the result page.
    /// </summary>
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

    /*
    // Update is called once per frame
    public void CloseMultiplayerResultPopUp()
    {
        PhotonNetwork.LoadLevel(1);
    }
    */
}
