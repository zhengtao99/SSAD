using Assets.Model;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QAManager : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    public GameObject topLeftButton;
    public GameObject topRightButton;
    public GameObject bottomLeftButton;
    public GameObject bottomRightButton;
    public GameObject continueButton;
    public Button pressedButton;
    public Text qnText;
    private Text ansText = null;
    private int answerID;
    public static ColorBlock originalColors;
    private static DateTime startTime;
    public static int questionID;
    public int correct;

    public bool isMultiplayerMode;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (transform.name.Replace("(Clone)", "") == "QuestionPopUpPage"){
            //if (!isMultiplayerMode)
                PopulateQuestion();
            //continueButton = GameObject.Find("Continue");
            continueButton.SetActive(false);
            startTime = DateTime.Now;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    void DisableButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("AnswerButton");
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;

        }
    }
    void EnableButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("AnswerButton");
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
            button.GetComponentInChildren<Text>().color = Color.black;
            button.GetComponent<Button>().colors = originalColors;
        }
    }

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
            if (!ChestPopUp.Instance.isMultiplayerMode)
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
            //Debug.Log("set correct is false, check actual correct: " + correct);
            TurnRed();
        }
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.SaveAnalytics(ConnectionManager.user.Id, questionID, answerID, DateTime.Now - startTime));
        continueButton.SetActive(true);
        
    }

    public void ResumeGame()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        EnableButtons();
        continueButton.SetActive(false);
        ChestPopUp.Instance.ResumeGame();
    }
    
    public void TurnRed()
    {
        originalColors = pressedButton.colors;
        this.GetComponentInChildren<Text>().color = Color.white; 
        ColorBlock colors = pressedButton.colors;
        colors.selectedColor = Color.red;
        colors.disabledColor = Color.red;
        pressedButton.colors = colors;
        playerController.LifeUpdate();
    }

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
