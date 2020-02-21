using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject loginPage;
    public GameObject startPage;
    public GameObject playPage;
    public GameObject gameOverPage;
    public GameObject chestPopUpPage;
    public Text scoreText;
    GameObject initialPlayState;

    public enum PageState {
        None,    //None of others
        Login,
        Start,
        Play,
        GameOver,
        ChestPopUp
    } 

    bool gameOver = true; //Initially not start game

    public bool GameOver {
        get { return gameOver; }
    }

    void Awake() {
        Instance = this;
        SetPageState(PageState.Login);
    }

    void OnEnable() {
        // += to subscribe event in other C# script
        PlayerController.OnPlayerDied += OnPlayerDied;
    }

    void OnDisable() {
        // -= to unsubscribe event in other C# script
        PlayerController.OnPlayerDied -= OnPlayerDied;
    }

    void OnPlayerDied(int value) {
        gameOver = true;
        scoreText.text = "Score: " + value.ToString();
        CreateOpenedChest.OpenedChestInstance.CloseOpenedChest();
        SetPageState(PageState.GameOver);
    }

    public void SetPageState(PageState state) {
        switch (state) {
            case PageState.None:
                loginPage.SetActive(false);
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                break;
            case PageState.Login:
                loginPage.SetActive(true);
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                break;
            case PageState.Start:
                loginPage.SetActive(false);
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                break;
            case PageState.GameOver:
                loginPage.SetActive(false);
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                playPage.SetActive(false);
                chestPopUpPage.SetActive(false);
                break;
            case PageState.Play:
                loginPage.SetActive(false);
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(true);
                chestPopUpPage.SetActive(false);
                break;
            case PageState.ChestPopUp:
                loginPage.SetActive(false);
                Debug.Log("StateChange Chest Pop up");
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                playPage.SetActive(true);
                chestPopUpPage.SetActive(true);
                break;
        }
    }

    //activated when replay button is hit
    public void MainMenu() {
        SetPageState(PageState.Start);
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
    }

    //activated when play button is hit
    public void StartGame() {
        SetPageState(PageState.Play);
        playPage = initialPlayState;

        //Save initial state again (clone another object)
        initialPlayState = Instantiate(playPage, playPage.transform.parent);
    }

    public void ChestPopUp()
    {
        Debug.Log("Game Manager Chest Pop up");
        SetPageState(PageState.ChestPopUp);
    }
}
