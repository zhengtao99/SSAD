using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public GameObject canvas;
    public Sprite lockedImg;
    public Sprite unlockedImg;
    public Vector3 flagScale;
    private List<GameObject> levelButtons = new List<GameObject>();
    private List<GameObject> flags = new List<GameObject>();
    private Vector3 temp = new Vector3(0, 0, 0);

    //Last completed level
    int lastCompletedLevel = 2; //0 if haven't completed any

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in canvas.transform)
        {
            if (child.tag == "LevelButton")
                levelButtons.Add(child.gameObject);
            if (child.tag == "Flag")
                flags.Add(child.gameObject);
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

            if (i <= lastCompletedLevel - 1)  //completed: < or ==
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

    public void completeLevel()
    {
        //Increase the completed levels
        lastCompletedLevel++;

        //Wait 1s to popup flag then unlock next level
        Invoke("unlockLevel", 1f);
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
