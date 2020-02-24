using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    public Text subject;
    public Sprite[] images;
    public SpriteRenderer img;
    public int currentPage;
    // Start is called before the first frame update
    void Start()
    {
        currentPage = 0;
        SetCurrentPage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetCurrentPage()
    {
        switch (this.currentPage)
        {
            case 0:
                subject.text = "Mathematics";
                img.sprite = images[0];
                break;
            case 1:
                subject.text = "Physics";
                img.sprite = images[1];
                break;
            case 2:
                subject.text = "Programming";
                img.sprite = images[2];
                break;
        }
    }
    public void OnClickBack() { 
        if(this.currentPage == 0)
        {
            this.currentPage = 2;
        }
        else
        {
            this.currentPage = this.currentPage - 1;
        }
        SetCurrentPage();
    }

    public void OnClickForward()
    {
        this.currentPage = (this.currentPage + 1)%3;
        SetCurrentPage();
    }

    public void OnClickContinue()
    {

    }
}
