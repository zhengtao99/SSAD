using System.Collections;
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
    public GameObject a;

    private List<GameObject> ranks = new List<GameObject>();
    public void WorldSearch()
    {
        ConnectionManager.Highscores = new List<Highscore>();
        var tb = GameObject.Find("LeaderboardPage").GetComponentsInChildren<Button>().Where(z => z.name == "Subject").First()
            .GetComponentsInChildren<TextMeshProUGUI>().Where(z => z.name == "Label").First();
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.GetCurrentUserScore(WorldController.currentWorld.Id, ConnectionManager.user.Id,"Worlds"));
    }
    public void TopicSearch()
    {
        ConnectionManager.Highscores = new List<Highscore>();
        var tb = GameObject.Find("LeaderboardPage").GetComponentsInChildren<Button>().Where(z => z.name == "Subject").First()
            .GetComponentsInChildren<TextMeshProUGUI>().Where(z => z.name == "Label").First();
        ConnectionManager cm = new ConnectionManager();
        StartCoroutine(cm.GetCurrentUserScore(SectionController.currentTopic.Id, ConnectionManager.user.Id,"Topics"));
    }
    public void SetLabels()
    {
        var worldLbl = GameObject.Find("LeaderboardPage").GetComponentsInChildren<Button>().Where(z => z.name == "Subject").First()
           .GetComponentsInChildren<TextMeshProUGUI>().Where(z => z.name == "Label").First();
        var topicLbl = GameObject.Find("LeaderboardPage").GetComponentsInChildren<Button>().Where(z => z.name == "Topic").First()
           .GetComponentsInChildren<TextMeshProUGUI>().Where(z => z.name == "Label").First();
        worldLbl.text = WorldController.currentWorld.Name;
        topicLbl.text = SectionController.currentTopic.Name;
    }
    public void Search()
    {
        ConnectionManager.Highscores = new List<Highscore>();
        var tb = GameObject.Find("SearchBox").GetComponentsInChildren<TextMeshProUGUI>().Where(z=>z.name=="Text").First();
        ConnectionManager cm = new ConnectionManager();
        string text = tb.text;
        int id = 0;
        if(ConnectionManager.Category == "Topics")
        {
            id = SectionController.currentTopic.Id;
        }
        if (ConnectionManager.Category == "Worlds")
        {
            id = WorldController.currentWorld.Id;
        }
        StartCoroutine(cm.GetHighscore(id, text.Remove(text.Count()-1, 1),""));
    }
    public void AddRank(int id, string Search, string Filter, Highscore highscore, string category)
    {
        
        StopAllCoroutines();
        ConnectionManager cm = new ConnectionManager();
        GameObject rank = Instantiate(rankTemplate) as GameObject;
        rank.SetActive(true);



        //Update Rank Clone with Index, Name, Level and Score
        TextMeshProUGUI indexText = rank.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        indexText.text = highscore.Rank.ToString();
        TextMeshProUGUI nameText = rank.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        nameText.text = highscore.User.FirstName + " " + highscore.User.LastName;
        TextMeshProUGUI levelText = rank.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        levelText.text = "LV. " + highscore.Stage.ToString();
        TextMeshProUGUI scoreText = rank.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        scoreText.text = highscore.TotalScore.ToString() ;

        //Push into list
        rank.transform.SetParent(rankTemplate.transform.parent, false);
        ranks.Add(rank.gameObject);
        StartCoroutine(cm.GetHighscore(id, Search, Filter));        
        
        a = GameObject.Find("bird_idle_f01");
        SpriteRenderer m_SpriteRenderer;

        m_SpriteRenderer = GameObject.Find("bird_idle_f01").GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.red;
        //Color32[] Colors;
        //Colors = new Color32[4];
        //Colors[0] = new Color32(255, 0, 0, 255);
        //Colors[1] = new Color32(0, 255, 0, 255);
        //Colors[2] = new Color32(255, 0, 0, 255);
        //Colors[3] = new Color32(0, 0, 0, 255);
        //a.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
    }
    public void ClearRanks()
    {
        foreach(var rank in ranks)
        {
            Destroy(rank);
        }
    }
    public void SetCurrentPlayerRank(Highscore highscore)
    {
        TextMeshProUGUI indexText = currentPlayerRank.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = currentPlayerRank.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI levelText = currentPlayerRank.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = currentPlayerRank.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        if (highscore != null)
        {
            indexText.text = highscore.Rank.ToString();
            nameText.text = highscore.User.FirstName + " " + highscore.User.LastName;
            levelText.text = "LV. " + highscore.Stage.ToString();
            scoreText.text = highscore.TotalScore.ToString();
        }
        else
        {
            indexText.text = "-";
            nameText.text = ConnectionManager.user.FirstName + " " + ConnectionManager.user.LastName;
            levelText.text = "LV. 0";
            scoreText.text = "0";
        }
        ConnectionManager cm = new ConnectionManager();
        if (ConnectionManager.Category == "Topics")
        {
            StartCoroutine(cm.GetHighscore(SectionController.currentTopic.Id, "", ""));
        }
        if (ConnectionManager.Category == "Worlds")
        {
            StartCoroutine(cm.GetHighscore(WorldController.currentWorld.Id, "", ""));
        }
    }
}
