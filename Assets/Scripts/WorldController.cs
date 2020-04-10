using Assets.Model;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class holds all the methods to manipulate the world page display.
/// </summary>
public class WorldController : MonoBehaviour
{
    /// <summary>
    /// A variable that contains the subject name.
    /// </summary>
    public Text subject;

    /// <summary>
    /// A variable that contains an array of world images for various subjects.
    /// </summary>
    public GameObject[] images;

    /// <summary>
    /// A variable that contains the current world image.
    /// </summary>
    private GameObject currentImg;

    /// <summary>
    /// A variable that contains the previous world image
    /// </summary>
    private GameObject lastImg;

    /// <summary>
    /// A variable that contains the current page.
    /// </summary>
    public int currentPage;

    /// <summary>
    /// A variable that contains the current world object.
    /// </summary>
    public static World currentWorld;

    /// <summary>
    /// A variable that contains the name of current world chosen.
    /// </summary>
    private string currentWorldName;

    /// <summary>
    /// A variable that contains speed of world images switching.
    /// </summary
    private float speed = 20.0f;

    /// <summary>
    /// A variable that contains a boolean to detect if the player selected the next world.
    /// </summary>
    private bool moveCurrent = false;

    /// <summary>
    /// A variable that contains a boolean to detect if the player selected the previous world.
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
    /// </summary
    private Vector3 currentImgDest = new Vector3(2.1f, 0, 0);

    /// <summary>
    /// A variable that contains scale to scale the current to previous image size.
    /// </summary
    private Vector3 scaleFactor = new Vector3(0.025f, 0.025f, 0.025f);

    /// <summary>
    /// A variable that contains scale to scale image to the left.
    /// </summary>
    private Vector3 leftEnd = new Vector3(-5f, 0, 0);

    /// <summary>
    /// A variable that contains scale to scale image to the right.
    /// </summary>
    private Vector3 rightEnd = new Vector3(7.5f, 0, 0);

    /// <summary>
    /// A variable that contains an array of world names.
    /// </summary>
    private string[] worlds;

    /// <summary>
    /// The method is called before the first frame update, it is responsible to initialize the default world image in the display.
    /// </summary>
    void Start()
    {
        currentPage = 0;
        currentImg = Instantiate(images[0]) as GameObject;
        currentImg.SetActive(true);
        images[0].SetActive(false);
        Transform t = currentImg.transform;
        t.SetParent(transform);
        t.localPosition = currentImgDest;
    }

    /// <summary>
    /// The method is called once per frame, it is responsible to detect if player choses to change world and perform the world image manipulation/animation.
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

        if (moveLast)
        {
            lastImg.transform.localScale -= scaleFactor;
            lastImg.transform.localPosition = Vector3.MoveTowards(lastImg.transform.localPosition, lastImgDest, Time.deltaTime * speed);
        }

    }

    /// <summary>
    /// The method is used to set default world and congifuring the world's neighbouring worlds, it will be used to perform the animation of switching worlds when needed.
    /// </summary>
    public void SetCurrentPage()
    {
        worlds = ConnectionManager.Worlds.Select(z=>z.Name).ToArray();
        currentWorldName = worlds[currentPage];
        subject.text = currentWorldName;
        lastImg = currentImg;
        currentImg = Instantiate(images[currentPage % 3]) as GameObject;
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
    /// The method is used to perform the switching world animation when the left button is clicked.
    /// </summary>
    public void OnClickBack() {
        FindObjectOfType<SoundManager>().Play("MinorButton");
        if (moveLast || moveCurrent)
        {
            return;
        }

        if (this.currentPage == 0)
        {
            this.currentPage = 2;
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
    /// The method is used to perform the switching world animation when the right button is clicked.
    /// </summary>
    public void OnClickForward()
    {
        FindObjectOfType<SoundManager>().Play("MinorButton");
        if (moveLast || moveCurrent)
        {
            return;
        }

        this.currentPage = (this.currentPage + 1)%3;
        right = true;
        lastImgDest = rightEnd;
        SetCurrentPage();
    }

    /// <summary>
    /// The method is used move the player to the topic page when the continue button is selected.
    /// </summary>
    public void OnClickContinue()
    {
        FindObjectOfType<SoundManager>().Play("MajorButton");
        currentWorld = ConnectionManager.Worlds.Where(z => z.Name == currentWorldName).First();
        StartCoroutine(ConnectionManager.GetTopic(currentWorld.Id));
    }

    /// <summary>
    /// The method is used display the initial world page with all the default game objects.
    /// </summary>
    public void StartWorldPage()
    {
        Destroy(lastImg);
        Destroy(currentImg);
        worlds = ConnectionManager.Worlds.Select(z => z.Name).ToArray();
        currentWorldName = worlds[0];
        subject.text = currentWorldName;
        lastImg = currentImg;
        currentImg = Instantiate(images[0]) as GameObject;
        currentImg.SetActive(true);

        Transform t = currentImg.transform;
        t.SetParent(transform);
        t.localPosition = currentImgDest;
    }
}
