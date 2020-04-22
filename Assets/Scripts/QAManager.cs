using Assets.Model;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This manager class is responsible for displaying of questions, managing the interactions between players and the game, as well as giving out rewards or penalties for every question answered.
/// </summary>
public class QAManager : MonoBehaviour
{
    /// <summary>
    /// A variable that contains QAManager instance. 
    /// </summary>
    public static QAManager Instance;

    /// <summary>
    /// A variable that holds the player game object where the player attributes will be manipulated in the game.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// A variable that holds player's controller class that will be used to manipulate the player game object in the game. 
    /// </summary>
    public PlayerController playerController;

    /// <summary>
    /// A variable that holds the first answer button game object located at the top left side of the display.
    /// </summary>
    public GameObject topLeftButton;

    /// <summary>
    /// A variable that holds the second answer button game object located at the top right side of the display.
    /// </summary>
    public GameObject topRightButton;

    /// <summary>
    /// A variable that holds the third answer button game object located at the bottom left side of the display.
    /// </summary>
    public GameObject bottomLeftButton;

    /// <summary>
    /// A variable that holds the forth answer button game object located at the bottom right side of the display.
    /// </summary>
    public GameObject bottomRightButton;

    /// <summary>
    /// A variable that holds the continue button game object.
    /// </summary>
    public GameObject continueButton;

    /// <summary>
    /// A variable that holds the pressed answer button game object.
    /// </summary>
    public Button pressedButton;

    /// <summary>
    /// A variable that holds question text ui object displayed on the screen.
    /// </summary>
    public Text qnText;

    /// <summary>
    /// A variable that holds answer text ui object displayed on the screen.
    /// </summary>
    private Text ansText = null;

    /// <summary>
    /// A variable that holds answer id.
    /// </summary>
    private int answerID;

    /// <summary>
    /// A variable that holds the color of a answer button.
    /// </summary>
    public ColorBlock originalColors =  ColorBlock.defaultColorBlock;

    /// <summary>
    /// A variable that holds the time the player open a chest and starts answering a question. This is recorded in date & time.
    /// </summary>
    private static DateTime startTime;

    /// <summary>
    /// A variable that holds question id.
    /// </summary>
    public static int questionID;

    /// <summary>
    /// A variable that holds a boolean to indicate is the player have answered the question correctly. Correct would be true.
    /// </summary>
    public int correct;

    /// <summary>
    /// A boolean variable that records if the scene is in single or multiplayer mode.
    /// </summary>
    public bool isMultiplayerMode;

    // Start is called before the first frame update
    /// <summary>
    /// This method is called to create an QAManager instance to allow other scripts to access the methods defined in this class.
    /// </summary>
    private void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// This method is called whenever the question page is called, the method is responsble to initiailize the question by calling PopulateQuestion() and initializing the player variable to be used for manipulation later.
    /// </summary>
    void OnEnable()
    {
        if (transform.name.Replace("(Clone)", "") == "QuestionPopUpPage"){
            //if (!isMultiplayerMode)
                PopulateQuestion();
            //continueButton = GameObject.Find("Continue");
            if (originalColors == ColorBlock.defaultColorBlock)
            {
                Debug.Log("Is Default");
                originalColors = pressedButton.colors;
            }
            EnableButtons();
            continueButton.SetActive(false);
            startTime = DateTime.Now;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    /// <summary>
    /// This method takes in a list of answers and populate the text in the answer buttons in the question page.
    /// </summary>
    /// <param name="answers">A list of answers for a given question</param>
    public void PopulateAnswers(List<Answer> answers)
    {
        //Create list to randomly pick up 1 button to populate data

        List<GameObject> buttons = new List<GameObject>{topLeftButton,topRightButton,
        bottomLeftButton,bottomRightButton};
        while (buttons.Count != 0)
        {
            int index = UnityEngine.Random.Range(0, buttons.Count); //Not inclusive of Max
            GameObject button = buttons[index];//Retrieve button from list
            ansText = button.GetComponentInChildren<Text>();
            ansText.text = answers[buttons.Count - 1].Description; //Retrieve 1 possible answer
            button.GetComponent<QAManager>().answerID = answers[buttons.Count - 1].Id;//Retrieve answerID
            //Debug.Log("Error 1");
            answers.RemoveAt(buttons.Count - 1);
            //Debug.Log("Error 2");
            buttons.RemoveAt(index);
            //Debug.Log("Error 3");
        }
    }

    /// <summary>
    /// This method is responsible to populate the question page with a question.
    /// </summary>
    public void PopulateQuestion()
    {
        qnText.text = "Qn: " + "The question will be retrieved here. Loading....";
        var questions = ConnectionManager.Questions;
        int randomNumber = UnityEngine.Random.Range(0, questions.Count);
        var question = questions[randomNumber];
        questionID = question.Id;
        qnText.text = question.Description;
        PopulateAnswers(question.Answers);
        questions.Remove(question);
    }

    /// <summary>
    /// This method is responsible for disabling the buttons to prevent unwanted interactions that may cause error after the player has answered the question.
    /// </summary>
    void DisableButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("AnswerButton");
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;

        }
    }

    /// <summary>
    /// This method is responsible to enable the buttons when the question page is active to allow the player to choose an answer for the question.
    /// </summary>
    public void EnableButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("AnswerButton");
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
            button.GetComponentInChildren<Text>().color = Color.black;
            button.GetComponent<Button>().colors = originalColors;
        }
    }

    /// <summary>
    /// This method is responsible to verify and update the players points or lifes in cases where the player answer the question correctly or wrongly.
    /// </summary>
    public void VerifyAnswer()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        bool isCorrect = true;
        DisableButtons();
        bool condition;

        if (answerID == 1)
        {
            correct = 1;
            PlayerPrefs.SetInt("correct", correct);
            //Debug.Log("set correct is true, check actual correct: " +correct);
            TurnGreen();
            if (!ChestPopUpController.Instance.isMultiplayerMode)
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ScoreUpdate(20);

            else
            {
                MultiplayerSceneManager.Instance.myPlayer.GetComponent<MyPlayerController>().ScoreUpdate(20);
            }
        }

        else
        {
            correct = 0;
            PlayerPrefs.SetInt("correct", correct);
            TurnRed();
        }
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.SaveAnalytics(ConnectionManager.user.Id, questionID, answerID, DateTime.Now - startTime)); //Easy
        //continueButton.SetActive(true);

    }

    /// <summary>
    /// This method is responsible for continuing the game once the player pressed on continue button.
    /// </summary>
    public void ResumeGame()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        EnableButtons();
        continueButton.SetActive(false);
        ChestPopUpController.Instance.ResumeGame();
    }

    /// <summary>
    /// This method is responsible changing answer button to red color to show wrong answer has be selected.
    /// </summary>
    public void TurnRed()
    {
        originalColors = pressedButton.colors;
        Debug.Log(pressedButton.colors);
        this.GetComponentInChildren<Text>().color = Color.white; 
        ColorBlock colors = pressedButton.colors;
        colors.selectedColor = Color.red;
        colors.disabledColor = Color.red;
        pressedButton.colors = colors;

        //ConnectionManager cm = new ConnectionManager();
        //StartCoroutine(cm.SaveAnalytics(ConnectionManager.user.Id, questionID, answerID, DateTime.Now - startTime));
        //if (!isMultiplayerMode)
        //    playerController.LifeUpdate();
        //else
        //    MultiplayerSceneManager.Instance.myPlayer.GetComponent<MyPlayerController>().LifeUpdate();
    }

    /// <summary>
    /// This method is responsible changing answer button to green color to show correct answer has be selected.
    /// </summary>
    public void TurnGreen()
    {
        this.GetComponentInChildren<Text>().color = Color.white;
        originalColors = pressedButton.colors;
        ColorBlock colors = pressedButton.colors;
        colors.selectedColor = Color.green;
        colors.disabledColor = Color.green;
        pressedButton.colors = colors;

        if (!isMultiplayerMode)
            playerController.increaseCorrectAns();
        
    }
}
