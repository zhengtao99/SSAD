using Assets.Model;
using Assets.Scripts;
using Assets.Scripts.Multiplayer_Scripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class is the main controller for all the activities throughout the entire Learnable Mobile Game Application. This controller is responsible for coordinating the other sub controllers as well as providing the main methods to navigate between different pages.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// A variable that contains an instance of GameManager that allows other game object's controllers to access the methods defined in this class.
    /// </summary>
    public static GameManager Instance;

    /// <summary>
    /// A variable that contains loading game object.
    /// </summary>
    public GameObject loading;

    /// <summary>
    /// A variable that contains inivitation page game object used in multiplayer gamemode.
    /// </summary>
    public GameObject invitation;

    /// <summary>
    /// A variable that contains profile page game object.
    /// </summary>
    public GameObject profilePage;

    /// <summary>
    /// A variable that contains ready page game object.
    /// </summary>
    public GameObject readyPage;

    /// <summary>
    /// A variable that contains start page game object.
    /// </summary>
    public GameObject startPage;

    /// <summary>
    /// A variable that contains play page game object.
    /// </summary>
    public GameObject playPage;

    /// <summary>
    /// A variable that contains game over page game object.
    /// </summary>
    public GameObject gameOverPage;

    /// <summary>
    /// A variable that contains chest pop up page game object.
    /// </summary>
    public GameObject chestPopUpPage;

    /// <summary>
    /// A variable that contains world page game object.
    /// </summary>
    public GameObject worldPage;

    /// <summary>
    /// A variable that contains section/topic page game object.
    /// </summary>
    public GameObject sectionPage;

    /// <summary>
    /// A variable that contains question pop up page game object.
    /// </summary>
    public GameObject questionPopUpPage;

    /// <summary>
    /// A variable that contains level page game object.
    /// </summary>
    public GameObject levelPage;

    /// <summary>
    /// A variable that contains unlocked level pop up page game object.
    /// </summary>
    public GameObject unlockedLevelPopUp;

    /// <summary>
    /// A variable that contains completed level pop up page game object.
    /// </summary>
    public GameObject completedLevelPopUp;

    /// <summary>
    /// A variable that contains win level pop up page game object.
    /// </summary>
    public GameObject winLevelPopUp;

    /// <summary>
    /// A variable that contains leaderboard page game object.
    /// </summary>
    public GameObject leaderboardPage;

    /// <summary>
    /// A variable that contains multiplayer page game object.
    /// </summary>
    public GameObject multiPlayerPage;

    /// <summary>
    /// A variable that contains mode page game object.
    /// </summary>
    public GameObject modePage;

    /// <summary>
    /// A variable that contains online player listing page used in multiplayer gameplay.
    /// </summary>
    public GameObject onlinePlayerListingsPage;

    /// <summary>
    /// A variable that contains warning page.
    /// </summary>
    public GameObject warningInModePage;

    /// <summary>
    /// A variable that contains the text in the invitation game object that is used in multiplayer gameplay.
    /// </summary>
    public Text InvitationText;

    /// <summary>
    /// A variable that contains waiting board that is used in multiplayer gameplay.
    /// </summary>
    public GameObject waitingBoard;

    /// <summary>
    /// A variable that contains multiplayer match up page used in multiplayer gameplay.
    /// </summary>
    public GameObject mutiplayerMatchPage;

    /// <summary>
    /// A variable that logout button game object.
    /// </summary>
    public GameObject btnLogout;

    /// <summary>
    /// A variable that will be used to store the identity of the player that sents the invitation.
    /// </summary>
    public string invitationSender;

    /// <summary>
    /// A variable that contains the score text UI object.
    /// </summary>
    public Text scoreText;

    /// <summary>
    /// A variable that contains the initial page state after the player has login to the game  client.
    /// </summary>
    GameObject initialPlayState;


    /// <summary>
    /// A variable that contains the player's select level of difficulty.
    /// </summary>
    int chosenLevel =0;

    /// <summary>
    /// This method is called before the first frame update, it is used to initialize the chosenLevel variable as well as setting the logout button as inactive.
    /// </summary>
    void Start()
    {
        chosenLevel = PlayerPrefs.GetInt("chosenLevel");
        btnLogout.SetActive(false);
    }

    /// <summary>
    /// A group of constants representing the different page states that the game can be in.
    /// </summary>
    public enum PageState {
        None,    //None of others
        Profile,
        Ready,
        Play,
        GameOver,
        ChestPopUp,
        WorldUI,
        SectionUI,
        QuestionPopUp,
        LevelUI,
        UnlockedLevelPopUp,
        CompletedLevelPopUp,
        GameWin,
        LeaderboardPage,
        MultiplayerPage,
        ModePage,
        OnlinePlayerListingsPage,
        MultiplayerMatchPage,
    }

    /// <summary>
    /// A variable that contains the boolean to check if the game is still ongoing. If it is not, it will be true. 
    /// </summary>
    bool gameOver = true; //Initially not start game

    /// <summary>
    /// This method is used to retrieve the value of gameOver variable
    /// </summary>
    /// <returns>
    /// Boolean value in game over variable.
    /// </returns>
    public bool GameOver {
        get { return gameOver; }
    }

    /// <summary>
    /// This method is called before Start() to initialize the GameManager instance, display login page on game launch and set play page to false.
    /// </summary>
    void Awake() {
        Instance = this;
        SetPageState(PageState.ModePage);

        PhotonNetwork.AutomaticallySyncScene = true;

        //Ensure initialPlayState (playPage clone) is disabled
        playPage.SetActive(false);
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
        //DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// This private method is used to display the different pages based on the page request passed by the other public methods in the class, this will allow the players to navigate between different pages.
    /// </summary>
    /// <param name="state">The player's selected page state.</param>
    public void SetPageState(PageState state) {
        switch (state) {
            case PageState.None:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
           
            case PageState.Ready:
                profilePage.SetActive(false);

                readyPage.SetActive(true);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.GameOver:

                //FindObjectOfType<SoundManager>().Pause("EasyStage");
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(true);
                playPage.SetActive(true);
                chestPopUpPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.Play:
                chosenLevel = LevelController.Instance.getChosenLevel();
                if (chosenLevel > 4 && chosenLevel < 8)
                {
                    FindObjectOfType<SoundManager>().Play("MediumStage");
                }

                else if (chosenLevel > 7)
                {
                    FindObjectOfType<SoundManager>().Play("HardStage");
                }
                else
                {
                    FindObjectOfType<SoundManager>().Play("EasyStage");
                }
                //FindObjectOfType<SoundManager>().Play("EasyStage");
                FindObjectOfType<SoundManager>().Stop("Lobby");
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(true);
                chestPopUpPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.ChestPopUp:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(true);
                chestPopUpPage.SetActive(true);
                questionPopUpPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.WorldUI:
                FindObjectOfType<SoundManager>().Play("Lobby");
                profilePage.SetActive(true);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                worldPage.SetActive(true);
                sectionPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.SectionUI:
                FindObjectOfType<SoundManager>().Play("Lobby");
                profilePage.SetActive(true);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(true);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.LeaderboardPage:
                FindObjectOfType<SoundManager>().Play("Lobby");
                profilePage.SetActive(true);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(true);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.QuestionPopUp:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(true);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(true);
                questionPopUpPage.SetActive(true);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.LevelUI:
                if (chosenLevel != 0)
                    if (chosenLevel > 4 && chosenLevel < 8)
                    {
                        FindObjectOfType<SoundManager>().Stop("MediumStage");
                    }

                    else if (chosenLevel > 7)
                    {
                        FindObjectOfType<SoundManager>().Stop("HardStage");
                    }
                    else
                    {
                        FindObjectOfType<SoundManager>().Stop("EasyStage");
                    }
                FindObjectOfType<SoundManager>().Play("Lobby");
                profilePage.SetActive(true);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(true);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.UnlockedLevelPopUp:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(true);
                unlockedLevelPopUp.SetActive(true);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.CompletedLevelPopUp:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(true);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(true);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.GameWin:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(true);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(true);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.MultiplayerPage:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(true);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.ModePage:
                FindObjectOfType<SoundManager>().Play("Lobby");
                warningInModePage.SetActive(false);
                profilePage.SetActive(true);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(true);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.OnlinePlayerListingsPage:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(true);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(false);
                break;
            case PageState.MultiplayerMatchPage:
                profilePage.SetActive(false);

                readyPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                worldPage.SetActive(false);
                sectionPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                questionPopUpPage.SetActive(false);
                levelPage.SetActive(false);
                unlockedLevelPopUp.SetActive(false);
                completedLevelPopUp.SetActive(false);
                winLevelPopUp.SetActive(false);
                leaderboardPage.SetActive(false);
                multiPlayerPage.SetActive(false);
                modePage.SetActive(false);
                onlinePlayerListingsPage.SetActive(false);
                invitation.SetActive(false);
                waitingBoard.SetActive(false);
                mutiplayerMatchPage.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// This method is used to perform player logout.
    /// </summary>
    public void Logout()
    {
        SceneManager.LoadScene("Login");
    }
    //activated when replay button is hit
    public void WorldUI() {
        StartCoroutine(ConnectionManager.GetWorld());
        SetPageState(PageState.WorldUI);
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
    public void StartGame() {
        if (chosenLevel !=0)
            if (chosenLevel > 4 && chosenLevel < 8)
            {
                FindObjectOfType<SoundManager>().Stop("MediumStage");
            }
            else if (chosenLevel > 7)
            {
                FindObjectOfType<SoundManager>().Stop("HardStage");
            }
            else
            {
                FindObjectOfType<SoundManager>().Stop("EasyStage");
            }
        FindObjectOfType<SoundManager>().Play("MajorButton");

        /*
        //Destroy old play page
        Destroy(playPage);

        playPage = initialPlayState;

        //Save initial state again (clone another object)
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
        //SetPageState(PageState.Play);
        */

        StartCoroutine(ConnectionManager.GetQuestions(SectionController.currentTopic.Id, LevelController.Instance.getChosenLevel()));
    }

    /// <summary>
    /// This method is called create the chest pop up animation when the player touches a chest in the maze environment while playing the game.
    /// </summary>
    public void ChestPopUp()
    {
        SetPageState(PageState.ChestPopUp);
    }

    /// <summary>
    /// This method is called to navigate the player to the ready page, where he/she can select topics to play the game in for specific worlds.
    /// </summary>
    public void sectionUI()
    {
        SetPageState(PageState.SectionUI);
    }

    /// <summary>
    /// This method is called to navigate the player to the world page after successful login.
    /// </summary>
    public void worldUI()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        SetPageState(PageState.WorldUI);
    }

    /// <summary>
    /// This method is called to display the question pop up once the player touches a chest in the maze environment.
    /// </summary>
    public void QuestionPopUp() {
        SetPageState(PageState.QuestionPopUp);
    }

    /// <summary>
    /// This method is called to navigate the player to level selection page after selecting a topic.
    /// </summary>
    public void levelUI()
    {
        SetPageState(PageState.LevelUI);
    }

    /// <summary>
    /// This method is called display a loading page while the system proceeds to retrieve the available levels for a topic selected.
    /// </summary>
    public void loadLevelUI()
    {
        User user = ConnectionManager.user;
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.GetAvailableStages(SectionController.currentTopic.Id));
    }

    /// <summary>
    /// This method is called to load the cleared stages based on topic selected.
    /// </summary>
    /// <param name="TopicId">Player's selected topic id.</param>
    public void loadClearedStages(int TopicId)
    {
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.GetClearedStages(TopicId));
    }

    /// <summary>
    /// This method is called display unlocked level pop up for player's selected level.
    /// </summary>
    public void popUpUnlockedLevelBoard()
    {
        SetPageState(PageState.UnlockedLevelPopUp);
    }

    /// <summary>
    /// This method is called display completed level pop up for player's selected level.
    /// </summary>
    public void popUpUCompletedLevelBoard()
    {
        SetPageState(PageState.CompletedLevelPopUp);
    }

    /// <summary>
    /// This method is called to display loading page.
    /// </summary>
    public void ShowLoading()
    {
        loading.SetActive(true);
    }

    /// <summary>
    /// This method is called to disable loading page.
    /// </summary>
    public void HideLoading()
    {
        loading.SetActive(false);
    }

    /// <summary>
    /// This method is called to display the invitation on the other player's screen once the  current player selects to send invitiation.
    /// </summary>
    public void ShowInvitation(string senderName)
    {
        if (playPage.activeSelf)  //If playing game
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PauseGame();

        invitationSender = senderName;
        invitation.SetActive(true);
        GameObject.Find("Invitation").GetComponent<InvitationController>().Show();
        InvitationText.text = senderName + ": Hi there! Want to start a battle with me?";
    }

    /// <summary>
    /// This method is called to remove the invitation on the oplayer's screen on accept or decline invitation
    /// </summary>
    public void HideInvitation()
    {
        if (playPage.activeSelf)  //If playing game
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ResumeGame();

        invitation.SetActive(false);
    }

    /// <summary>
    /// This method is called to display waiting board when the current player sends an invitation, the player will be waiting for a response.
    /// </summary>
    public void ShowWaitingBoard()
    {
        waitingBoard.SetActive(true);
    }

    /// <summary>
    /// This method is called to remove the waiting waiting board once the receiving player accepts or declines the invitation.
    /// </summary>
    public void HideWaitingBoard()
    {
        WaitingBoardController.Instance.stopSettingWaitingText();
        waitingBoard.SetActive(false);
    }

    /// <summary>
    /// This method is called to display the game over pop up page.
    /// </summary>
    public void EnterGameOver()
    {
        SetPageState(PageState.GameOver);
    }


    /// <summary>
    /// This method is called to display the game win pop up page.
    /// </summary>
    public void EnterGameWin()
    {
        SetPageState(PageState.GameWin);
    }

    /// <summary>
    /// This method is called to display the leaderboard page.
    /// </summary>
    public void ViewLeaderboard()
    {
        SetPageState(PageState.LeaderboardPage);
    }

    /// <summary>
    /// This method is called to display the topic selection page.
    /// </summary>
    public void Ready()
    {
        SetPageState(PageState.Ready);
    }

    /// <summary>
    /// This method is called to display the multiplayer page.
    /// </summary>
    public void multiPlayer()
    {
        SetPageState(PageState.MultiplayerPage);
    }

    /// <summary>
    /// This method is called to display the mode selection page between single or multiplayer.
    /// </summary>
    public void ModePage()
    {
        SetPageState(PageState.ModePage);
    }

    /// <summary>
    /// This method is called to display online player listing page for the player that will be sending the invitation.
    /// </summary>
    public void OnlinePlayerListingsUI()
    {
        //Intentionally leave room to see available rooms
        PhotonNetwork.LeaveRoom(true);

        SetPageState(PageState.OnlinePlayerListingsPage);
    }

    /// <summary>
    /// This method is called to display multiplayer match up page for both players.
    /// </summary>
    public void MultiplayerMatchUI()
    {
        HideInvitation();
        SetPageState(PageState.MultiplayerMatchPage);
    }
}
