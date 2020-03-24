using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Assets.Model;

namespace Assets.Scripts
{
    class ConnectionManager
    {
        static string Domain = "https://localhost:44365"; //ZT host
        //static string Domain = "https://learnablems20200220070049.azurewebsites.net";

        public static User user;
        public static List<Question> Questions;
        public static List<Topic> Topics;
        public static List<World> Worlds;
        public static List<AvailableStage> AvailableStages;
        public static List<Highscore> Highscores;

        public static string Category;
        public static IEnumerator Login(string username, string password)
        {
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/login/" + username + "/" + password);
             yield return www.SendWebRequest();
            GameManager.Instance.HideLoading();
            try
            {
                bool result = bool.Parse(www.downloadHandler.text);
                LoginController.Result(result);
            }
            catch(Exception)
            {
                try
                {
                    user = JsonUtility.FromJson<User>(www.downloadHandler.text);
                    GameManager.Instance.WorldUI();
                }
                catch(Exception)
                {
                   
                }
            }
            
        }
        public static IEnumerator GetQuestions(int topicId, int stage)
        {
   
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/questions/" + topicId + "/" + stage);
            yield return www.SendWebRequest();
            string json = "{\"Questions\":" + www.downloadHandler.text + "}";
            var questionCollection = JsonUtility.FromJson<QuestionCollection>(json);          
            Questions = questionCollection.Questions;
            GameManager.Instance.HideLoading();
            GameManager.Instance.Ready();       
        }
        public static IEnumerator GetTopic(int WorldId)
        {
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/topics/" + WorldId);
            yield return www.SendWebRequest();
            string json = "{\"Topics\":" + www.downloadHandler.text + "}";
            var topicCollection = JsonUtility.FromJson<TopicCollection>(json);
            Topics = topicCollection.Topics;
            GameManager.Instance.HideLoading();
            GameManager.Instance.sectionUI();
            GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("section")).First().GetComponent<SectionController>().StartSectionPage();
        }
        public static IEnumerator GetWorld()
        {
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/worlds/");
            yield return www.SendWebRequest();
            string json = "{\"Worlds\":" + www.downloadHandler.text + "}";
            var worldCollection = JsonUtility.FromJson<WorldCollection>(json);
            Worlds = worldCollection.Worlds;
            GameManager.Instance.HideLoading();
            GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("world")).First().GetComponent<WorldController>().StartWorldPage();
        }
        public static IEnumerator GetAvailableStages(int TopicId, int UserId)
        { 
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/stages/" + TopicId + "/" + UserId);
            yield return www.SendWebRequest();         
            string json = "{\"AvailableStages\":" + www.downloadHandler.text + "}";           
            var stageCollection = JsonUtility.FromJson<AvailableStageCollection>(json);
            AvailableStages = stageCollection.AvailableStages;
            GameManager.Instance.HideLoading();
            GameManager.Instance.levelUI();
        }
        public static IEnumerator UpdateHighscore(int UserId, int TopicId, int Stage, int Score, bool IsCleared)
        {
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/highscore/" + UserId + "/" + TopicId + "/" + Stage + "/" + Score + "/" + IsCleared);
            yield return www.SendWebRequest();
        }
        public static IEnumerator SaveAnalytics(int UserId, int QnsId, int AnsId, TimeSpan Speed)
        {
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/analytics/" + UserId + "/" + QnsId + "/" + AnsId + "/" + Speed.TotalMilliseconds);
            yield return www.SendWebRequest();
        }
        public IEnumerator GetCurrentUserScore(int Id, int UserId, string category)
        {
            Category = category;
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/highscores/" + Category + "/" + Id + "/" + UserId);
            yield return www.SendWebRequest();
            string json = www.downloadHandler.text;
            var highscore = JsonUtility.FromJson<Highscore>(json);
            GameManager.Instance.ViewLeaderboard();
            GameObject.Find("LeaderboardPage").GetComponent<RankListController>().SetLabels();
            GameObject.Find("LeaderboardPage").GetComponent<RankListController>().SetCurrentPlayerRank(highscore);
        }
        public IEnumerator GetHighscore(int id, string Search, string Filter)
        {          
            bool firstLoad = false;
            if (Search == "")
            {
                Search = "-";
            }
            if (Filter == "")
            {
                firstLoad = true;
                Filter = "-";
            }
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/highscores/" + Category + "/" + id + "/" + Search + "/" + Filter);
            Debug.Log(www.url);
            yield return www.SendWebRequest();

            string json = www.downloadHandler.text;
            if (json != "")
            {
                var highscore = JsonUtility.FromJson<Highscore>(json);
                Highscores.Add(highscore);
                var ids = Highscores.Select(z => z.User.Id.ToString()).ToArray();
                var idStr = string.Join("-", ids);
                if (firstLoad)
                {
                    GameObject.Find("LeaderboardPage").GetComponent<RankListController>().ClearRanks();
                }
                GameObject.Find("LeaderboardPage").GetComponent<RankListController>().AddRank(id, Search, idStr, highscore, Category);
            }
        }
    }
    
}
