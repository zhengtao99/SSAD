using Assets.Model;
using Assets.Scripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject loading;
    public GameObject invitation;
    public GameObject loginPage;
    public GameObject profilePage;
    public GameObject readyPage;
    public GameObject startPage;
    public GameObject playPage;
    public GameObject gameOverPage;
    public GameObject chestPopUpPage;
    public GameObject worldPage;
    public GameObject sectionPage;
    public GameObject questionPopUpPage;
    public GameObject levelPage;
    public GameObject unlockedLevelPopUp;
    public GameObject completedLevelPopUp;
    public GameObject winLevelPopUp;
    public GameObject leaderboardPage;
    public GameObject multiPlayerPage;
    public GameObject modePage;
    public GameObject onlinePlayerListingsPage;
    public GameObject warningInModePage;
    public Text InvitationText;
    public GameObject waitingBoard;

    public Text scoreText;
    GameObject initialPlayState;

    int chosenLevel=0;

    void Start()
    {
        chosenLevel = PlayerPrefs.GetInt("chosenLevel");
    }

    public enum PageState {
        None,    //None of others
        Login,
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
        OnlinePlayerListingsPage
    }

    bool gameOver = true; //Initially not start game

    public bool GameOver {
        get { return gameOver; }
    }

    void Awake() {
        Instance = this;
        SetPageState(PageState.ModePage);

        PhotonNetwork.AutomaticallySyncScene = true;

        //Ensure initialPlayState (playPage clone) is disabled
        playPage.SetActive(false);
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
        //DontDestroyOnLoad(this.gameObject);
    }

    public void SetPageState(PageState state) {
        switch (state) {
            case PageState.None:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                break;
            case PageState.Login:
                FindObjectOfType<SoundManager>().Play("GameLaunch");
                profilePage.SetActive(false);
                loginPage.SetActive(true);
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
                break;
            case PageState.Ready:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                break;
            case PageState.GameOver:

                //FindObjectOfType<SoundManager>().Pause("EasyStage");
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                loginPage.SetActive(false);
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
                break;
            case PageState.ChestPopUp:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                break;
            case PageState.WorldUI:
                FindObjectOfType<SoundManager>().Play("Lobby");
                profilePage.SetActive(true);
                loginPage.SetActive(false);
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
                break;
            case PageState.SectionUI:
                FindObjectOfType<SoundManager>().Play("Lobby");
                profilePage.SetActive(true);
                loginPage.SetActive(false);
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
                break;
            case PageState.LeaderboardPage:
                FindObjectOfType<SoundManager>().Play("Lobby");
                profilePage.SetActive(true);
                loginPage.SetActive(false);
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
                break;
            case PageState.QuestionPopUp:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                loginPage.SetActive(false);
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
                break;
            case PageState.UnlockedLevelPopUp:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                break;
            case PageState.CompletedLevelPopUp:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                break;
            case PageState.GameWin:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                break;
            case PageState.MultiplayerPage:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                break;
            case PageState.ModePage:
                FindObjectOfType<SoundManager>().Play("Lobby");
                warningInModePage.SetActive(false);
                profilePage.SetActive(true);
                loginPage.SetActive(false);
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
                break;
            case PageState.OnlinePlayerListingsPage:
                profilePage.SetActive(false);
                loginPage.SetActive(false);
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
                break;
        }
    }

    //activated when replay button is hit
    public void WorldUI() {
        StartCoroutine(ConnectionManager.GetWorld());
        SetPageState(PageState.WorldUI);
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
    public void ChestPopUp()
    {
        SetPageState(PageState.ChestPopUp);
    }

    public void sectionUI()
    {
        SetPageState(PageState.SectionUI);
    }

    public void worldUI()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        SetPageState(PageState.WorldUI);
    }

    public void QuestionPopUp() {
        SetPageState(PageState.QuestionPopUp);
    }

    public void levelUI()
    {
        SetPageState(PageState.LevelUI);
    }

    public void loadLevelUI()
    {
        User user = ConnectionManager.user;
        StartCoroutine(ConnectionManager.GetAvailableStages(SectionController.currentTopic.Id, user.Id));
    }

    public void popUpUnlockedLevelBoard()
    {
        SetPageState(PageState.UnlockedLevelPopUp);
    }

    public void popUpUCompletedLevelBoard()
    {
        SetPageState(PageState.CompletedLevelPopUp);
    }
    public void ShowLoading()
    {
        loading.SetActive(true);
    }
    public void HideLoading()
    {
        loading.SetActive(false);
    }

    public void ShowInvitation(string senderName)
    {
        if (playPage.activeSelf)  //If playing game
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PauseGame();

        invitation.SetActive(true);
        InvitationText.text = senderName + ": Hi there! Want to start a battle with me?";
    }
    public void HideInvitation()
    {
        if (playPage.activeSelf)  //If playing game
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ResumeGame();

        invitation.SetActive(false);
    }

    public void ShowWaitingBoard()
    {
        waitingBoard.SetActive(true);
    }

    public void HideWaitingBoard()
    {
        WaitingBoardController.Instance.stopSettingWaitingText();
        waitingBoard.SetActive(false);
    }

    public void EnterGameOver()
    {
        SetPageState(PageState.GameOver);
    }

    public void EnterGameWin()
    {
        SetPageState(PageState.GameWin);
    }

    public void ViewLeaderboard()
    {
        SetPageState(PageState.LeaderboardPage);
    }

    public void Ready()
    {
        SetPageState(PageState.Ready);
    }

    public void multiPlayer()
    {
        SetPageState(PageState.MultiplayerPage);
    }

    public void ModePage()
    {
        SetPageState(PageState.ModePage);
    }

    public void OnlinePlayerListingsUI()
    {
        //Intentionally leave room to see available rooms
        PhotonNetwork.LeaveRoom(true);

        SetPageState(PageState.OnlinePlayerListingsPage);
    }
}
