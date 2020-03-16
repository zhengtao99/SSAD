﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts;
using Assets.Model;
using System.Linq;

public class RankListController : MonoBehaviour
{
    [SerializeField]
    private GameObject rankTemplate;


    public GameObject currentPlayerRank;

    private List<GameObject> ranks = new List<GameObject>();
    public void Search()
    {
        ConnectionManager.Highscores = new List<Highscore>();
        var tb = GameObject.Find("SearchBox").GetComponentsInChildren<TextMeshProUGUI>().Where(z=>z.name=="Text").First();
        ConnectionManager cm = new ConnectionManager();
        string text = tb.text;
        StartCoroutine(cm.GetTopicHighscore(SectionController.currentTopic.Id, text.Remove(text.Count()-1, 1),""));
    }
    public void AddRank(int TopicId, string Search, string Filter, Highscore highscore)
    {
        StopAllCoroutines();
        ConnectionManager cm = new ConnectionManager();
        GameObject rank = Instantiate(rankTemplate) as GameObject;
        rank.SetActive(true);

        //Update Rank Clone with Index, Name, Level and Score
        TextMeshProUGUI indexText = rank.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        indexText.text = ConnectionManager.Highscores.Count.ToString();
        TextMeshProUGUI nameText = rank.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        nameText.text = highscore.User.FirstName + " " + highscore.User.LastName;
        TextMeshProUGUI levelText = rank.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        levelText.text = "LV. " + highscore.Stage.ToString();
        TextMeshProUGUI scoreText = rank.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        scoreText.text = highscore.TotalScore.ToString() ;

        //Push into list
        rank.transform.SetParent(rankTemplate.transform.parent, false);
        ranks.Add(rank.gameObject);
        if(ValidateMatchingUser(highscore) == true)
        {
            SetCurrentPlayerRank();
        }

        StartCoroutine(cm.GetTopicHighscore(TopicId, Search, Filter));
        
    }
    public void ClearRanks()
    {
        foreach(var rank in ranks)
        {
            Destroy(rank);
        }
    }
    public void SetCurrentDefaultRank()
    {
        TextMeshProUGUI indexText = currentPlayerRank.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = currentPlayerRank.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI levelText = currentPlayerRank.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = currentPlayerRank.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        indexText.text = "-";
        nameText.text = ConnectionManager.user.FirstName + " " + ConnectionManager.user.LastName;
        levelText.text = "LV. 0";
        scoreText.text = "0";
    }
    public void SetCurrentPlayerRank()
    {
        var highscore = ConnectionManager.Highscores.Where(z => z.User.Id == ConnectionManager.user.Id).First();
        TextMeshProUGUI indexText = currentPlayerRank.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = currentPlayerRank.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI levelText = currentPlayerRank.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = currentPlayerRank.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        if (highscore != null)
        {
            indexText.text = ConnectionManager.Highscores.Count.ToString();
            nameText.text = highscore.User.FirstName + " " + highscore.User.LastName;
            levelText.text = "LV. " + highscore.Stage.ToString();
            scoreText.text = highscore.TotalScore.ToString();
        }
    }
    public bool ValidateMatchingUser(Highscore highscore)
    {
        if(highscore.User.Id == ConnectionManager.user.Id)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
