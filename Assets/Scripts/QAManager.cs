using Assets.Model;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QAManager : MonoBehaviour
{
    
    public  GameObject topLeftButton;
    public  GameObject topRightButton;
    public  GameObject bottomLeftButton;
    public  GameObject bottomRightButton;
    public GameObject continueButton;
    public  Button pressedButton;
    public  Text qnText;
    private  Text ansText = null;
    private  int answerID;
    public  ColorBlock originalColors;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(transform.name.Replace("(Clone)", "") == "QuestionPopUpPage"){
            PopulateQuestion();
            continueButton = GameObject.Find("Continue");
            continueButton.SetActive(false);
           
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void  PopulateAnswers(List<Answer> answers){
        //Create list to randomly pick up 1 button to populate data

        List<GameObject> buttons = new List<GameObject>{topLeftButton,topRightButton,
        bottomLeftButton,bottomRightButton};
        while (buttons.Count != 0){
            int index = Random.Range(0,buttons.Count-1); //Not inclusive of Max
            GameObject button = buttons[index];//Retrieve button from list
            ansText = button.GetComponentInChildren<Text>();
            ansText.text = answers[buttons.Count - 1].Description; //Retrieve 1 possible answer
            answerID = 2;//Retrieve answer ID;
            answers.RemoveAt(buttons.Count - 1);
            buttons.RemoveAt(index);
        }
       
    }

    public  void  PopulateQuestion(){
        qnText.text = "Qn: " + "The question will be retrieved here. Loading....";
        var questions = MazeGenerator.questions;
        int randomNumber = Random.Range(0, questions.Count);
        var question = questions[randomNumber];
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

    public void VerifyAnswer(){
        FindObjectOfType<SoundManager>().Play("MajorButton");
        bool isCorrect = (answerID == 1? true :false);
        DisableButtons();
        if (isCorrect)
        {
            TurnGreen();
        }
        else
        {
            TurnRed();
        }
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
    }

    public void TurnGreen()
    {
        this.GetComponentInChildren<Text>().color = Color.white;
        originalColors = pressedButton.colors;
        ColorBlock colors = pressedButton.colors;
        colors.selectedColor = Color.green;
        colors.disabledColor = Color.green;
        pressedButton.colors = colors;
    }
}
