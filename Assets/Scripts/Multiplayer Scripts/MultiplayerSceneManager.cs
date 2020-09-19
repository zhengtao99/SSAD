using Assets.Scripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// This class is the main controller for all the activities throughout the entire multiplayer gameplay mode. This controller is responsible for coordinating the other sub controllers as well as providing the main methods to navigate between different pages.
/// </summary>
public class MultiplayerSceneManager : MonoBehaviour
{
    /// <summary>
    /// A variable that contains an instance of MultiplayerSceneManager that allows other game object's controllers to access the methods defined in this class.
    /// </summary>
    public static MultiplayerSceneManager Instance;

    /// <summary>
    /// A variable that contains loading game object.
    /// </summary>
    public GameObject loading;

    /// <summary>
    /// A variable that contains play page game object.
    /// </summary>
    public GameObject playPage;

    /// <summary>
    /// A variable that contains chest pop up page game object.
    /// </summary>
    public GameObject chestPopUpPage;

    /// <summary>
    /// A variable that contains question pop up page game object.
    /// </summary>
    public GameObject questionPopUpPage;

    /// <summary>
    /// A variable that contains game over pop up page game object.
    /// </summary>
    public GameObject gameOverPopUpPage;

    /// <summary>
    /// A variable that contains the initial page state after the player selected multiplayer mode.
    /// </summary>
    GameObject initialPlayState;

    /// <summary>
    /// A variable that contains the current player game object.
    /// </summary>
    public GameObject myPlayer;

    /// <summary>
    /// A group of constants representing the different page states that the multiplayer game can be in.
    /// </summary>
    public enum PageState
    {
        None,    //None of others
        Play,
        ChestPopUp,
        QuestionPopUp,
        GameOverPopUp
    }

    /// <summary>
    /// This method is called before Start() to initialize the GameManager instance, display multiplayer maze environment and create player.
    /// </summary>
    void Awake()
    {
        Instance = this;
        SetPageState(PageState.Play);
        CreatePlayer();

        //Ensure initialPlayState (playPage clone) is disabled
        //playPage.SetActive(false);
        //initialPlayState = Instantiate(playPage, playPage.transform.parent);
    }

    /// <summary>
    /// This method is used to display the different pages based on the page request passed by the other public methods in the class, this will allow the players to navigate between different pages.
    /// </summary>
    /// <param name="state">The player's selected page state.</param>
    public void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                break;
            case PageState.Play:
                FindObjectOfType<SoundManager>().Play("MediumStage");
                FindObjectOfType<SoundManager>().Stop("Lobby");
                playPage.SetActive(true);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                break;
            case PageState.ChestPopUp:
                playPage.SetActive(true);
                chestPopUpPage.SetActive(true);
                questionPopUpPage.SetActive(false);
                break;
            case PageState.QuestionPopUp:
                playPage.SetActive(true);
                chestPopUpPage.SetActive(true);
                questionPopUpPage.SetActive(true);
                break;
            case PageState.GameOverPopUp:
                playPage.SetActive(true);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                gameOverPopUpPage.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// This method is used to create a new game, initialize the maze environment.
    /// </summary>
    public void createNewGame()
    {
        //Destroy old play page
        Destroy(playPage);

        playPage = initialPlayState;

        //Save initial state again (clone another object)
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
    }

    /// <summary>
    /// This method is called whenever player selects to play a game, the relevant world, topic and level selected details will be passed to the play game to generate the maze environment accordingly.
    /// </summary>
    public void StartGame()
    {
        FindObjectOfType<SoundManager>().Stop("MediumStage");
        FindObjectOfType<SoundManager>().Play("MajorButton");

        SetPageState(PageState.Play);

        //StartCoroutine(ConnectionManager.GetQuestions(SectionController.currentTopic.Id, LevelController.Instance.getChosenLevel()));
    }

    /// <summary>
    /// This method is called create the chest pop up animation when the player touches a chest in the maze environment while playing the game.
    /// </summary>
    public void ChestPopUp()
    {
        SetPageState(PageState.ChestPopUp);
    }

    /// <summary>
    /// This method is called to display the question pop up once the player touches a chest in the maze environment.
    /// </summary>
    public void QuestionPopUp()
    {
        SetPageState(PageState.QuestionPopUp);
    }

    /// <summary>
    /// This method is called to display loading page.
    /// </summary
    public void ShowLoading()
    {
        loading.SetActive(true);
    }

    /// <summary>
    /// This method is called to hide loading page.
    /// </summary
    public void HideLoading()
    {
        loading.SetActive(false);
    }

    /// <summary>
    /// This method is called to display game over pop up page for both players.
    /// </summary>
    public void GameOverPopUp()
    {
        SetPageState(PageState.GameOverPopUp);
    }

    /// <summary>
    /// This method is called to create a player network controller.
    /// </summary>
    private void CreatePlayer()
    {
        //creates player network controller but not player character
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PhotonNetworkPlayer"), 
            transform.position, Quaternion.identity, 0);
    }
}
