using Assets.Model;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    public GameObject canvas;
    public Sprite lockedImg;
    public Sprite unlockedImg;
    public Vector3 flagScale;
    private List<GameObject> levelButtons = new List<GameObject>();
    private List<GameObject> flags = new List<GameObject>();
    private Vector3 temp = new Vector3(0, 0, 0);

    //Last completed level
    private int lastCompletedLevel = 2; //0 if haven't completed any

    //public SpriteRenderer unlockedLevelBoard;
    //public SpriteRenderer completedLevelBoard;

    public GameObject UnlockedLevelPopUp;
    public GameObject CompletedLevelPopUp;
    public GameObject LoseLevelPopUp;
    public GameObject WinLevelPopUp;
    public static int chosenLevel = -1;
    public bool unlockNewLevel = false;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public int getLastCompletedLevel()
    {
        return lastCompletedLevel;
    }

    public int getChosenLevel()
    {
        return chosenLevel;
    }

    public void SetAvailableStages()
    {
        foreach (Transform child in canvas.transform)
        {
            if (child.tag == "LevelButton")
                levelButtons.Add(child.gameObject);
            if (child.tag == "Flag")
                flags.Add(child.gameObject);
        }
        var stages = ConnectionManager.AvailableStages;

        lastCompletedLevel = stages.Where(z=>z.IsCleared).OrderByDescending(z=>z.Stage).Select(z=>z.Stage).FirstOrDefault();
        

        //Debug.Log("lastCompletedLevel: " + lastCompletedLevel);
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

        }
    }
    void OnEnable()
    {
        if (unlockNewLevel)
        {
            completeLevel();
        }
        else  //first time or not growing flag if already set
        {
            Debug.Log("SetAvailableStages");
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
                ScoreText.text = ConnectionManager.AvailableStages.Where(z => z.Stage == chosenLevel).First().Amount.ToString();
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

    public void closeLevelPopUp()
    {
        enableAllLevelBtns();
        GameManager.Instance.levelUI();
    }

    public void GameOverPopUp(int value)
    {
        SpriteRenderer loseLevelBoard = LoseLevelPopUp.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "lose_level_board").First();
        loseLevelBoard.sprite = Resources.Load<Sprite>("LevelLose/lose_level_" + chosenLevel);

        Text scoreText = LoseLevelPopUp.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText").First();
        scoreText.text = value.ToString();

        GameManager.Instance.EnterGameOver();
    }

    public void WinPopUp(int value)
    {
        SpriteRenderer winLevelBoard = WinLevelPopUp.GetComponentsInChildren<SpriteRenderer>().Where(z => z.name == "win_level_board").First();
        winLevelBoard.sprite = Resources.Load<Sprite>("LevelWin/win_level_" + chosenLevel);

        Text scoreText = WinLevelPopUp.GetComponentsInChildren<Text>().Where(z => z.name == "ScoreText").First();
        scoreText.text = value.ToString();

        GameManager.Instance.EnterGameWin();
    }

    public void completeLevel()
    {
        //Increase the completed levels
        if (lastCompletedLevel != 10)
            lastCompletedLevel++;

        //Wait 1s to popup flag then unlock next level
        Invoke("unlockLevel", 1f);

        FindObjectOfType<SoundManager>().Play("NextLevel");
    }

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

    public void enableAllLevelBtns()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject levelButton = levelButtons[i];
            Button btn = levelButton.GetComponent<Button>();
            btn.enabled = true;
        }
    }

    public void disableAllLevelBtns()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject levelButton = levelButtons[i];
            Button btn = levelButton.GetComponent<Button>();
            btn.enabled = false;
        }
    }

    // Update is called once per frame
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
