using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QAManager : MonoBehaviour
{
    
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
    // Start is called before the first frame update
    void Start()
    {
        if(transform.name.Replace("(Clone)", "") == "QuestionPopUpPage"){
            RetrievePossibleAnswers();
            RetrieveQuestion();
            continueButton = GameObject.Find("Continue");
            continueButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RetrievePossibleAnswers(){
        //Create list to randomly pick up 1 button to populate data
        List<GameObject> buttons = new List<GameObject>{topLeftButton,topRightButton,
        bottomLeftButton,bottomRightButton};
        while (buttons.Count != 0){
            int index = Random.Range(0,buttons.Count); //Not inclusive of Max
            GameObject button = buttons[index];//Retrieve button from list
            ansText = button.GetComponentInChildren<Text>();
            ansText.text = "answer " + index; //Retrieve 1 possible answer
            answerID = 2;//Retrieve answer ID;
            buttons.RemoveAt(index);
        }
    }

    void RetrieveQuestion(){
        qnText.text = "Qn: " + "The question will be retrieved here. Loading....";
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
