﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject loading;
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
    public Text scoreText;
    GameObject initialPlayState;

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
        GameWin
    } 

    bool gameOver = true; //Initially not start game

    public bool GameOver {
        get { return gameOver; }
    }

    void Awake() {
        Instance = this;
        SetPageState(PageState.Login);

        //Ensure initialPlayState (playPage clone) is disabled
        playPage.SetActive(false);
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
        
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
                break;
            case PageState.GameOver:
                FindObjectOfType<SoundManager>().Pause("EasyStage");
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
                break;
            case PageState.Play:
                FindObjectOfType<SoundManager>().Play("EasyStage");
                FindObjectOfType<SoundManager>().Pause("Lobby");
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
                break;
            case PageState.LevelUI:
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
                break;
        }
    }

    //activated when replay button is hit
    public void WorldUI() {
        StartCoroutine(ConnectionManager.GetWorld());
        SetPageState(PageState.WorldUI);
    }

    //activated when play button is hit
    public void StartGame() {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        //Destroy old play page
        Destroy(playPage);

        playPage = initialPlayState;

        //Save initial state again (clone another object)
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
        SetPageState(PageState.Play);
    }
    public void ChestPopUp()
    {
        SetPageState(PageState.ChestPopUp);
    }

    public void sectionUI()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        SetPageState(PageState.SectionUI);
    }

    public void worldUI()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        SetPageState(PageState.WorldUI);
    }

    public void QuestionPopUp(){
        SetPageState(PageState.QuestionPopUp);
    }

    public void levelUI()
    {
        SetPageState(PageState.LevelUI);
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
    public void EnterGameOver()
    {
        SetPageState(PageState.GameOver);
    }

    public void EnterGameWin()
    {
        SetPageState(PageState.GameWin);
    }
}
