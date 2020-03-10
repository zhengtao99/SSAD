using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickController : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject player;
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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        circle.transform.localPosition = new Vector2(370, -197);
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
            circle.transform.localPosition = new Vector2(370, -197);
            playerController.stopMove();
        }
    }

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

    private bool inThreshold()
    {
        Debug.Log(xCoord);
        Debug.Log(yCoord);
        if (xCoord > 651 && xCoord < 811 && yCoord > 0 && yCoord < 147)
        {
            return true;
        }
        return false;
    }
}
