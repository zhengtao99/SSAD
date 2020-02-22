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
    public Text qnText;
    private Text ansText = null;
    // Start is called before the first frame update
    void Start()
    {
        if(transform.name.Replace("(Clone)", "") == "QuestionPopUpPage"){
            RetrievePossibleAnswers();
            RetrieveQuestion();
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
            ansText.text = "I'm the random " + index;
            buttons.RemoveAt(index);
        }
    }

    void RetrieveQuestion(){
        qnText.text = "Qn: " + "I am the retrieved question";
    }

    public void VerifyAnswer(){
        ChestPopUp.Instance.ResumeGame();
    }
}
