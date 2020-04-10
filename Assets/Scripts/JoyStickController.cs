using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

/// <summary>
/// The class holding all the methods to allow the player to control the avatar movements through a on screen joystick display.
/// </summary>
public class JoyStickController : MonoBehaviour
{
    /// <summary>
    /// A variable holding a player controller that will be used to manipulate the player's movement in the maze.
    /// </summary>
    private PlayerController playerController;

    /// <summary>
    /// A variable that contains the player game object.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// A variable that contains the player's speed initialized to 5.
    /// </summary>
    public float speed = 5.0f;

    /// <summary>
    /// A variable that contains a boolean that will be used detect is player's finger is on screen touching the joystick button.
    /// </summary>
    private bool touchStart = false;

    /// <summary>
    /// A variable that specifies the player's initial position.
    /// </summary>
    private Vector2 pointA;

    /// <summary>
    /// A variable that specifies the player's next position based on the joystick's movement.
    /// </summary>
    private Vector2 pointB;

    /// <summary>
    /// A variable that contains the button game object that the player will be using to provide input.
    /// </summary>
    public GameObject circle;

    /// <summary>
    /// A variable that contains the border which the button must reside in.
    /// </summary>
    public GameObject outerCircle;

    /// <summary>
    /// A variable that contains x coordinate.
    /// </summary>
    private float xCoord;

    /// <summary>
    /// A variable that contains y coordinate.
    /// </summary>
    private float yCoord;

    /// <summary>
    /// A variable that contains the direction that the player is moving towards.
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// The method is called before the first frame update, it will find the player, player controller as well as initializing the button game objects.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        circle.transform.localPosition = new Vector2(outerCircle.transform.localPosition.x, outerCircle.transform.localPosition.y);
        //StartCoroutine(ConnectionManager.GetQuestions(10, 1));
    }

    /// <summary>
    /// The method is called once per frame, it will detect the position of the taps/clicks that the user is performing on the screen and move the button accordingly.
    /// </summary>
    void Update()
    {
        xCoord = Input.mousePosition.x;
        yCoord = Input.mousePosition.y;
        if (Input.GetMouseButtonDown(0) && inThreshold())
        {
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

        }
        if (Input.GetMouseButton(0) && inThreshold())
        {
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            touchStart = false;
            xCoord = 0;
            yCoord = 0;
            circle.transform.localPosition = new Vector2(outerCircle.transform.localPosition.x, outerCircle.transform.localPosition.y);
            playerController.stopMove();
        }
    }

    /// <summary>
    /// The method is called together with Update(), with the information gathered from Update(), it will manipulate the player to move left or right using player controller.
    /// </summary>
    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            if(Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
            {
                direction = new Vector2(offset.x, 0);
                if(offset.x > 0)
                {
                    playerController.moveRight();
                }
                else
                {
                    playerController.moveLeft();
                }
            }
            else
            {
                direction = new Vector2(0, offset.y);
                if (offset.y > 0)
                {
                    playerController.moveUp();
                }
                else
                {
                    playerController.moveDown();
                }
            }

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y);
        }

    }

    /// <summary>
    /// The method will check if the button is within the border at the specified position, if it is moving outside of the border, the button will be stopped.
    /// </summary>
    private bool inThreshold()
    {
        Vector3 pz = Camera.main.WorldToScreenPoint(outerCircle.transform.position);
        if (xCoord > pz.x - 70 && xCoord < pz.x + 70 && yCoord > pz.y - 70 && yCoord < pz.y + 70)
        {
            return true;
        }
        return false;
    }
}
