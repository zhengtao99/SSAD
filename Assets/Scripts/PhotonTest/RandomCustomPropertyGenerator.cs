using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RandomCustomPropertyGenerator : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    //Create a photon hashtable
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    private void SetCustomNumber()
    {
        System.Random rnd = new System.Random();
        int result = rnd.Next(0, 99);

        _text.text = result.ToString();

        //Set key value in hashtable
        _myCustomProperties["RandomNumber"] = result;

        //Set CustomProperties for all players
        PhotonNetwork.SetPlayerCustomProperties(_myCustomProperties);
    }

    public void OnClick_Button()
    {
        SetCustomNumber();
    }
}

