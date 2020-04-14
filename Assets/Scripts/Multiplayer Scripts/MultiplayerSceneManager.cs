using Assets.Scripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MultiplayerSceneManager : MonoBehaviour
{
    public static MultiplayerSceneManager Instance;
    public GameObject loading;
    public GameObject multiplayerMatch;
    public GameObject playPage;
    public GameObject chestPopUpPage;
    public GameObject questionPopUpPage;

    GameObject initialPlayState;

    public enum PageState
    {
        None,    //None of others
        MultiplayerMatch,
        Play,
        ChestPopUp,
        QuestionPopUp
    }

    void Awake()
    {
        Instance = this;
        SetPageState(PageState.Play);
        CreatePlayer();

        //Ensure initialPlayState (playPage clone) is disabled
        //playPage.SetActive(false);
        //initialPlayState = Instantiate(playPage, playPage.transform.parent);
    }

    public void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                multiplayerMatch.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                break;
            case PageState.MultiplayerMatch:
                multiplayerMatch.SetActive(true);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                break;
            case PageState.Play:
                FindObjectOfType<SoundManager>().Play("MediumStage");
                FindObjectOfType<SoundManager>().Stop("Lobby");
                multiplayerMatch.SetActive(false);
                playPage.SetActive(true);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                break;
            case PageState.ChestPopUp:
                multiplayerMatch.SetActive(false);
                playPage.SetActive(true);
                chestPopUpPage.SetActive(true);
                questionPopUpPage.SetActive(false);
                break;
            case PageState.QuestionPopUp:
                multiplayerMatch.SetActive(false);
                playPage.SetActive(true);
                chestPopUpPage.SetActive(true);
                questionPopUpPage.SetActive(true);
                break;
        }
    }

    public void createNewGame()
    {
        //Destroy old play page
        Destroy(playPage);

        playPage = initialPlayState;

        //Save initial state again (clone another object)
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
    }

    //activated when play button is hit
    public void StartGame()
    {
        FindObjectOfType<SoundManager>().Stop("MediumStage");
        FindObjectOfType<SoundManager>().Play("MajorButton");

        SetPageState(PageState.Play);

        //StartCoroutine(ConnectionManager.GetQuestions(SectionController.currentTopic.Id, LevelController.Instance.getChosenLevel()));
    }
    public void ChestPopUp()
    {
        SetPageState(PageState.ChestPopUp);
    }

    public void QuestionPopUp()
    {
        SetPageState(PageState.QuestionPopUp);
    }

    public void ShowLoading()
    {
        loading.SetActive(true);
    }

    public void HideLoading()
    {
        loading.SetActive(false);
    }

    public void MultiplayerMatchingUI()
    {
        SetPageState(PageState.MultiplayerMatch);
    }

    private void CreatePlayer()
    {
        //creates player network controller but not player character
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PhotonNetworkPlayer"), 
            transform.position, Quaternion.identity, 0);
    }
}
