using Assets.Model;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The controller class holds all the methods pertaining to the animations and functions in the section/topic selection page.
/// </summary>
public class SectionController : MonoBehaviour
{
    /// <summary>
    /// A variable that contains the section/topic name.
    /// </summary>
    public Text sectionTxt;

    /// <summary>
    /// A variable that contains an array of the section/topic game objects.
    /// </summary>
    public GameObject[] images;

    /// <summary>
    /// A variable that contains the current section/topic game object.
    /// </summary>
    private GameObject currentImg;

    /// <summary>
    /// A variable that contains the previous section/topic game object.
    /// </summary>
    private GameObject lastImg;

    /// <summary>
    /// A variable that contains page number representing the index of the topics selected.
    /// </summary>
    private int currentPage = 0;

    /// <summary>
    /// A variable that contains an array of section/topic names.
    /// </summary>
    static string[] sections;

    /// <summary>
    /// A variable that contains speed which controls the animation's movement.
    /// </summary>
    private float speed = 20.0f;

    /// <summary>
    /// A variable that contains a boolean to detect if the player selected the next topic.
    /// </summary>
    private bool moveCurrent = false;

    /// <summary>
    /// A variable that contains a boolean to detect if the player selected the previous topic.
    /// </summary>
    private bool moveLast = false;

    /// <summary>
    /// A variable that contains a boolean to detect if the player selected to the right arrow.
    /// </summary>
    private bool right = false;

    /// <summary>
    /// A variable that contains the previous image destination location.
    /// </summary>
    private Vector3 lastImgDest;

    /// <summary>
    /// A variable that contains the current image destination location.
    /// </summary>
    private Vector3 currentImgDest = new Vector3(7.62f, -1.18f, 0);

    /// <summary>
    /// A variable that contains scale to scale the current to previous image size.
    /// </summary>
    private Vector3 scaleFactor = new Vector3(0.06f, 0.06f, 0.06f);

    /// <summary>
    /// A variable that contains scale to scale image to the left.
    /// </summary>
    private Vector3 leftEnd = new Vector3(2.5f, 0, 0);

    /// <summary>
    /// A variable that contains scale to scale image to the right.
    /// </summary>
    private Vector3 rightEnd = new Vector3(10.5f, 0, 0);

    /// <summary>
    /// A variable that contains the current topic selected.
    /// </summary>
    public static Topic currentTopic;

    /// <summary>
    /// The method is called once per frame, it is responsible to detect if player choses to change world and perform the section/topic game object manipulation/animation.
    /// </summary>
    void Update()
    {

        if (lastImg && (Mathf.Abs(lastImg.transform.localPosition.x - lastImgDest.x) == 0))
        {
            Destroy(lastImg);
            moveLast = false;
        }

        if (Mathf.Abs(currentImg.transform.localPosition.x - currentImgDest.x) == 0)
        {
            moveCurrent = false;
        }

        if (moveCurrent)
        {
            currentImg.transform.localPosition = Vector3.MoveTowards(currentImg.transform.localPosition, currentImgDest, Time.deltaTime * speed);
        }

        if (moveLast && lastImg != null)
        {
            lastImg.transform.localScale -= scaleFactor;
            lastImg.transform.localPosition = Vector3.MoveTowards(lastImg.transform.localPosition, lastImgDest, Time.deltaTime * speed);
        }

    }

    /// <summary>
    /// The method is used to set default topic and congifuring the topic's neighbouring topics, it will be used to perform the animation of switching topics when needed.
    /// </summary>
    public void SetCurrentPage()
    {
        sectionTxt.text = sections[this.currentPage];
        currentTopic = ConnectionManager.Topics.Where(z => z.Name == sections[currentPage]).First();
        lastImg = currentImg;
        currentImg = Instantiate(images[this.currentPage % 5]) as GameObject;

        currentImg.SetActive(true);
        Transform t = currentImg.transform;
        t.SetParent(transform);

        Vector3 pos = rightEnd;
        if (right)
        {
            pos = leftEnd;
        }

        t.localPosition = pos;
        moveCurrent = true;
        moveLast = true;
    }

    /// <summary>
    /// The method is used to perform the switching topic animation when the left button is clicked.
    /// </summary>
    public void OnClickBack()
    {
        FindObjectOfType<SoundManager>().Play("MinorButton");
        if (moveLast || moveCurrent)
        {
            return;
        }

        if (this.currentPage == 0)
        {
            this.currentPage = sections.Length - 1;
        }
        else
        {
            this.currentPage = this.currentPage - 1;
        }

        lastImgDest = leftEnd;
        right = false;
        SetCurrentPage();
    }

    /// <summary>
    /// The method is used to perform the switching topic animation when the right button is clicked.
    /// </summary>
    public void OnClickForward()
    {
        FindObjectOfType<SoundManager>().Play("MinorButton");
        if (moveLast || moveCurrent)
        {
            return;
        }
        this.currentPage = (this.currentPage + 1) % sections.Length;
        right = true;
        lastImgDest = rightEnd;
        SetCurrentPage();
    }

    /// <summary>
    /// The method is used navigate the player to the leaderboard page.
    /// </summary>
    public void ViewLeaderboard()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        ConnectionManager.Highscores = new List<Highscore>();
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.GetCurrentUserScore(currentTopic.Id, ConnectionManager.user.Id,"Topics"));
    }

    /// <summary>
    /// The method is used display the initial section page with all the default game objects.
    /// </summary>
    public void StartSectionPage()
    {
        this.currentPage = 0;
        sections = ConnectionManager.Topics.OrderByDescending(z => z.Name).Select(z => z.Name).ToArray();
        currentTopic = ConnectionManager.Topics.Where(z => z.Name == sections[currentPage]).First();
        sectionTxt.text = sections[this.currentPage];
        Destroy(lastImg);
        Destroy(currentImg);
        currentImg = Instantiate(images[0]) as GameObject;
        currentImg.SetActive(true);
        Transform t = currentImg.transform;
        t.SetParent(transform);
        t.localPosition = currentImgDest;
    }
}
