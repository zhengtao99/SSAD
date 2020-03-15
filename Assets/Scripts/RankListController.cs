using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankListController : MonoBehaviour
{
    [SerializeField]
    private GameObject rankTemplate;


    public GameObject currentPlayerRank;

    private List<GameObject> ranks = new List<GameObject>();

    private List<Player> playersList = new List<Player> { 
        new Player(1, "Yuanchao", 100, 1200), 
        new Player(2, "Minh Thu", 50, 600), 
        new Player(3, "Zheng Tao", 8, 22)
    };

    // Start is called before the first frame update
    void OnEnable()
    {
        PopulatePlayers();
        //SetCurrentPlayerRank();
        GenerateRanks();
    }

    public void PopulatePlayers()
    {
        //Retrieve list of players, add into playersList with highest score first

        //playersList.Clear();
        //for loop to add into playersList
        //playersList.Add();
    }

    private void GenerateRanks()
    {
        //To generate the list in the leaderboard page

        if (ranks.Count > 0)
        {
            foreach (GameObject rank in ranks)
            {
                Destroy(rank.gameObject);
            }
            ranks.Clear();
        }
        
        foreach (Player player in playersList)
        {
            //Create Rank Clone
            GameObject rank = Instantiate(rankTemplate) as GameObject;
            rank.SetActive(true);

            //Update Rank Clone with Index, Name, Level and Score
            TextMeshProUGUI indexText = rank.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            indexText.text = player.index.ToString();
            TextMeshProUGUI nameText = rank.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            nameText.text = player.name;
            TextMeshProUGUI levelText = rank.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            levelText.text = player.level.ToString();
            TextMeshProUGUI scoreText = rank.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            scoreText.text = player.score.ToString();

            //Push into list
            rank.transform.SetParent(rankTemplate.transform.parent, false);
            ranks.Add(rank.gameObject);
        }
    }

    public void SetCurrentPlayerRank(int index, string name, int level, int score)
    {
        //SetCurrentPlayerRank
    }
}

internal class Player
{
    public Player(int index, string name, int level, int score)
    {
        this.index = index;
        this.name = name;
        this.level = level;
        this.score = score;
    }

    public int index { get; set; }

    public string name { get; set; }

    public int level { get; set; }

    public int score { get; set; }

}