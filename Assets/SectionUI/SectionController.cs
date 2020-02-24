using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionController : MonoBehaviour
{
    public Text sectionTxt;
    public Sprite[] images;
    public SpriteRenderer img;
    public int currentPage;
    public string[] sections = { "Vector", "Linear Algebra", "Discrete Math" };
    // Start is called before the first frame update
    void Start()
    {
        SetCurrentPage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetCurrentPage()
    {
        sectionTxt.text = sections[this.currentPage];
        img.sprite = images[this.currentPage];
    }

    public void OnClickBack()
    {
        if (this.currentPage == 0)
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
        this.currentPage = (this.currentPage + 1) % 3;
        SetCurrentPage();
    }
}
