using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.Linq;
using UnityEngine.UI;

public class MatchPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public Text playerTxt;
    void Start()
    {
        //Debug.Log(ConnectionManager.user.FirstName);
        playerTxt.text = ConnectionManager.user.FirstName + " " + ConnectionManager.user.LastName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
