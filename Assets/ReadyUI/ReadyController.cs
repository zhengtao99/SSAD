using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyController : MonoBehaviour
{
    public Text timeTxt;
    private int time = 3000;
    private int interval = 5;
    public GameObject mainCamera;
    //public GameObject[] loadingLeft;
    //public GameObject[] loadingRight;
    private float[] size = { 0.4752f, 0.5528f, 0.66f, 0.7458f,  0.93555f};
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        timeTxt.text = "3";
    }

    // Update is called once per frame
    void Update()
    {
        time -= interval;
        //if(time % 100 == 0)
        //{
        //    for(int i = 0; i < 5; i++)
        //    {
        //        float setSize = size[(index + i) % 5];
        //        loadingLeft[i].transform.localScale = new Vector3(setSize, setSize, setSize);
        //        loadingRight[i].transform.localScale = new Vector3(setSize, setSize, setSize);
        //    }
        //    index = (index + 1) % 5;
        //}
        if(time == -1000)
        {
            GameManager gameManager = mainCamera.GetComponent<GameManager>();
            gameManager.StartGame();
        }
        else if (time == 0)
        {
            timeTxt.text = "GO";
        }
        else if (time % 1000 == 0)
        {
            int displayTime = time / 1000;
            timeTxt.text = displayTime.ToString();
        }
    }
}
