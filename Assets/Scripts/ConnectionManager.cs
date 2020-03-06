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
    class ConnectionManager : MonoBehaviour
    {
        static string Domain = "https://localhost:44365";
        //static string Domain = "https://learnablems20200220070049.azurewebsites.net";

        public static User user;
        public static List<Question> Questions;
        public static List<Topic> Topics;
        public static List<World> Worlds;
        public static List<AvailableStage> AvailableStages;
    
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
            catch(Exception ex)
            {
                try
                {
                    user = JsonUtility.FromJson<User>(www.downloadHandler.text);
                    GameManager.Instance.WorldUI();
                }
                catch(Exception ex2)
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
            GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("section")).First().GetComponent<SectionController>().SetCurrentPage();
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
            GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("world")).First().GetComponent<WorldController>().SetCurrentPage();
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
            GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("level")).First().GetComponent<LevelController>().SetAvailableStages();
        }
    }
    
}
