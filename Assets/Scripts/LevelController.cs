using Assets.Model;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This controller class holds all the methods for responsible for the interactions between player and the level page.
/// </summary>
public class LevelController : MonoBehaviour
{
    /// <summary>
    /// An instance controller variable created to allow other game object's controller to make use of the methods defined here.
    /// </summary>
    public static LevelController Instance;

    /// <summary>
    /// A variable that contains a personalised canvas game object to display animation on the level page. 
    /// </summary>
    public GameObject canvas;

    /// <summary>
    /// A variable that contains the sprite to show that the level is locked. 
    /// </summary>
    public Sprite lockedImg;

    /// <summary>
    /// A variable that contains the sprite to show that the level has been unlocked.
    /// </summary>
    public Sprite unlockedImg;

    /// <summary>
    /// A variable that contains the sprite to show that the level is unavailable.
    /// </summary>
    public Sprite unavailableImg;

    /// <summary>
    /// A variable that contains the scale to manipulate the flag scale.
    /// </summary>
    public Vector3 flagScale;

    /// <summary>
    /// A variable that contains a list of level buttons.
    /// </summary>
    private List<GameObject> levelButtons = new List<GameObject>();

    /// <summary>
    /// A variable that contains a list of flags for cleared levels.
    /// </summary>
    private List<GameObject> flags = new List<GameObject>();

    /// <summary>
    /// A variable that contains initial position of all 0.
    /// </summary>
    private Vector3 temp = new Vector3(0, 0, 0);

    /// <summary>
    /// A variable that contains the last completed level.
    /// </summary>
    private int lastCompletedLevel = 2; //0 if haven't completed any

    /// <summary>
    /// A variable that contains unlocked level pop up game object.
    /// </summary>
    public GameObject UnlockedLevelPopUp;

    /// <summary>
    /// A variable that contains completed level pop up game object.
    /// </summary>
    public GameObject CompletedLevelPopUp;

    /// <summary>
    /// A variable that contains lose level pop up game object.
    /// </summary>
    public GameObject LoseLevelPopUp;

    /// <summary>
    /// A variable that contains win level pop up game object.
    /// </summary>
    public GameObject WinLevelPopUp;

    /// <summary>
    /// A variable that contains chosen level initialized to -1 when no level is chosen.
    /// </summary>
    public static int chosenLevel = -1;

    /// <summary>
    /// A boolean variable initialized to false, it is used to identify if the player has unlocked a new level.
    /// </summary>
    public bool unlockNewLevel = false;

    /// <summary>
    /// This method that is called to initialize at the start of the game to iniialize the level controller instance.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This method is used to get last completed level.
    /// </summary>
    public int getLastCompletedLevel()
    {
        return lastCompletedLevel;
    }

    /// <summary>
    /// This method is used to get player's chosen level.
    /// </summary>
    public int getChosenLevel()
    {
        return chosenLevel;
    }

    /// <summary>
    /// This method is used to set the level page with available levels.
    /// </summary>
    public void SetAvailableStages()
    {
        lastCompletedLevel = 0;
        foreach (Transform child in canvas.transform)
        {
            if (child.tag == "LevelButton")
                levelButtons.Add(child.gameObject);
            if (child.tag == "Flag")
                flags.Add(child.gameObject);
        }
        var clearedStages = ConnectionManager.ClearedStages;
        var availableStages = ConnectionManager.AvailableStages.OrderBy(z=>z.Stage).ToList();
        if (clearedStages.Where(z => z.IsCleared).Count() > 0)
        {
          
                 lastCompletedLevel = clearedStages.Select(z => z.Stage).Max();
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject levelButton = levelButtons[i];
            GameObject flag = flags[i];
            Animator ani = levelButton.GetComponent<Animator>();
            Image img = levelButton.GetComponent<Image>();
            Button btn = levelButton.GetComponent<Button>();
            SpriteRenderer spriteRenderer = levelButton.GetComponent<SpriteRenderer>();

            ani.enabled = false;
            if (img != null && btn != null)
            {
                btn.enabled = true;
                img.enabled = true;
                spriteRenderer.enabled = false;
            }

            if (i <= lastCompletedLevel-1)  //completed: < or ==
            {
                img.sprite = unlockedImg;               
                flag.transform.localScale = flagScale;
            }
            else  //not completed: >
            {
                if (i == lastCompletedLevel)
                    img.sprite = unlockedImg;
                else
                    img.sprite = lockedImg;
                flag.transform.localScale = new Vector3(0, 0, 0);
            }

            if (availableStages[i].IsAvailable == false)
            {
                btn.enabled = false;
                img.sprite = unavailableImg;
            }


        }
    }

    /// <summary>
    /// This method is used to call SetAvailableStages() everytime the level page is enabled.
    /// </summary>
    void OnEnable()
    {
        if (unlockNewLevel)
        {
            completeLevel();
        }
        else  //first time or not growing flag if already set
        {
            SetAvailableStages();
        }

        unlockNewLevel = false;
        /*
        if (win)
        {
            completeLevel();
            win = false;
        }
        */
    }

    /// <summary>
    /// This method is used to open up the different player game boards based on whether the level is completed or newly unlocked.
    /// </summary>
    public void openLevel()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            string levelButtonName = EventSystem.current.currentSelectedGameObject.name;  //level_button_1 (4)
            char levelStr = levelButtonName[16];
            if (levelStr >= '0' && levelStr <= '9')
                chosenLevel = (int)char.GetNumericValue(levelStr) + 1;
            else
                return;

            if (chosenLevel <= lastCompletedLevel)
            {
                SpriteRenderer completedLevelBoard = CompletedLevelPopUp.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "completed_level_board").First();
                completedLevelBoard.sprite = Resources.Load<Sprite>("LevelCompleted/completed_level_" + chosenLevel);
                disableAllLevelBtns();
                GameManager.Instance.popUpUCompletedLevelBoard();
                var ScoreText = CompletedLevelPopUp.GetComponentsInChildren<Text>().First();
                ScoreText.text = ConnectionManager.ClearedStages.Where(z => z.Stage == chosenLevel).First().Score.ToString();
            } 
            else if (chosenLevel == lastCompletedLevel + 1)
            {
                SpriteRenderer unlockedLevelBoard = UnlockedLevelPopUp.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "unlocked_level_board").First();
                unlockedLevelBoard.sprite = Resources.Load<Sprite>("LevelUnlocked/unlocked_level_" + chosenLevel);
                disableAllLevelBtns();
                GameManager.Instance.popUpUnlockedLevelBoard();
            }
        }

        PlayerPrefs.SetInt("chosenLevel", chosenLevel);
    }

    /// <summary>
    /// This method is used to close all the popups and return to the level page.
    /// </summary>
    public void closeLevelPopUp()
    {
        enableAllLevelBtns();
        GameManager.Instance.levelUI();
    }

    /// <summary>
    /// This method is used to display the game over pop up.
    /// </summary>
    public void GameOverPopUp(int value)
    {
        SpriteRenderer loseLevelBoard = LoseLevelPopUp.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "lose_level_board").First();
        loseLevelBoard.sprite = Resources.Load<Sprite>("LevelLose/lose_level_" + chosenLevel);

        Text scoreText = LoseLevelPopUp.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText").First();
        scoreText.text = value.ToString();

        GameManager.Instance.EnterGameOver();
    }

    /// <summary>
    /// This method is used to display the win game pop up.
    /// </summary>
    public void WinPopUp(int value)
    {
        SpriteRenderer winLevelBoard = WinLevelPopUp.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "win_level_board").First();
        winLevelBoard.sprite = Resources.Load<Sprite>("LevelWin/win_level_" + chosenLevel);

        Text scoreText = WinLevelPopUp.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText").First();
        scoreText.text = value.ToString();

        GameManager.Instance.EnterGameWin();
    }

    /// <summary>
    /// This method is used to move players to a new level on level completion.
    /// </summary>
    public void completeLevel()
    {
        //Increase the completed levels
        if (lastCompletedLevel != 10)
            lastCompletedLevel++;

        //Wait 1s to popup flag then unlock next level
        Invoke("unlockLevel", 1f);

        FindObjectOfType<SoundManager>().Play("NextLevel");
    }

    /// <summary>
    /// This method is used to show the animation of unlocking a new level when players completes a level.
    /// </summary>
    public void unlockLevel()
    {
        if (lastCompletedLevel <= 9)
        {
            GameObject nextUnlockedLevelButton = levelButtons[lastCompletedLevel];
            Animator ani = nextUnlockedLevelButton.GetComponent<Animator>();
            Image img = nextUnlockedLevelButton.GetComponent<Image>();
            Button btn = nextUnlockedLevelButton.GetComponent<Button>();
            SpriteRenderer spriteRenderer = nextUnlockedLevelButton.GetComponent<SpriteRenderer>();

            if (img != null && btn != null)
            {
                spriteRenderer.enabled = true;
                btn.enabled = false;
                img.enabled = false;
            }
            ani.enabled = true;

            Invoke("wakeupBtn", 1f);
        }
    }

    /// <summary>
    /// This method is used to change a level button to allow players to select it when the level is unlocked.
    /// </summary>
    public void wakeupBtn()
    {
        enableAllLevelBtns();

        GameObject nextUnlockedLevelButton = levelButtons[lastCompletedLevel];
        Image img = nextUnlockedLevelButton.GetComponent<Image>();
        Button btn = nextUnlockedLevelButton.GetComponent<Button>();
        SpriteRenderer spriteRenderer = nextUnlockedLevelButton.GetComponent<SpriteRenderer>();

        if (img != null && btn != null)
        {
            img.sprite = unlockedImg;
            spriteRenderer.enabled = false;
            btn.enabled = true;
            img.enabled = true;
        }
    }

    /// <summary>
    /// This method is used to set all unlocked level buttons as active.
    /// </summary>
    public void enableAllLevelBtns()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject levelButton = levelButtons[i];
            Button btn = levelButton.GetComponent<Button>();
            btn.enabled = true;
        }
    }

    /// <summary>
    /// This method is used to set all unlocked level buttons as inactive.
    /// </summary>
    public void disableAllLevelBtns()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject levelButton = levelButtons[i];
            Button btn = levelButton.GetComponent<Button>();
            btn.enabled = false;
        }
    }


    /// <summary>
    /// This method is used to check if there are new levels completed and creates a flag on newly completed levels.
    /// </summary>
    void Update()
    {
        if (lastCompletedLevel > 0)
        {
            GameObject newFlag = flags[lastCompletedLevel - 1];
            if (newFlag.transform.localScale.x < flagScale.x && newFlag.transform.localScale.y < flagScale.y)
            {
                temp.x += 0.1f;
                temp.y += 0.1f;
                newFlag.transform.localScale = temp;
            }
            else
                temp = new Vector3(0, 0, 0);
        }
    }
}
