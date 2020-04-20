using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class JoyStickMulController : MonoBehaviour
{
    public static JoyStickMulController Instance;
    public MyPlayerController playerController;
    public GameObject player;
    public float speed = 5.0f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;
    public GameObject circle;
    public GameObject outerCircle;
    private float xCoord;
    private float yCoord;
    private Vector2 direction;
    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        circle.transform.localPosition = new Vector2(outerCircle.transform.localPosition.x, outerCircle.transform.localPosition.y);
        //StartCoroutine(ConnectionManager.GetQuestions(10, 1));
    }

    // Update is called once per frame
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
            if (playerController)
            {
                playerController.stopMove();
            }
        }
    }

    private void FixedUpdate()
    {
        if (player)
        {
            playerController = player.GetComponent<MyPlayerController>();
        }
        else
        {
            return;
        }
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
            {
                direction = new Vector2(offset.x, 0);
                if (offset.x > 0)
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

    private bool inThreshold()
    {
        Vector3 pz = Camera.main.WorldToScreenPoint(outerCircle.transform.position);
        if (xCoord > pz.x - 150 && xCoord < pz.x + 150 && yCoord > pz.y - 150 && yCoord < pz.y + 150)
        {
            return true;
        }
        return false;
    }
}
