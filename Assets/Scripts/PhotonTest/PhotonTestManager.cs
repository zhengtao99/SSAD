using Assets.Model;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonTestManager : MonoBehaviour
{
    public static PhotonTestManager Instance;
    public GameObject ViewRooms;
    public GameObject CurrentRoom;


    public enum PageState
    {
        None,    //None of others
        ViewRooms,
        CurrentRoom
    }


    void Awake()
    {
        Instance = this;
    }

    public void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                ViewRooms.SetActive(false);
                CurrentRoom.SetActive(false);
                break;
            case PageState.ViewRooms:
                ViewRooms.SetActive(true);
                CurrentRoom.SetActive(false);
                break;
            case PageState.CurrentRoom:
                ViewRooms.SetActive(false);
                CurrentRoom.SetActive(true);
                break;
        }
    }

    //activated when replay button is hit
    public void ViewRoomsUI()
    {
        SetPageState(PageState.ViewRooms);
    }

    public void CurrentRoomUI()
    {
        SetPageState(PageState.CurrentRoom);
    }
}
