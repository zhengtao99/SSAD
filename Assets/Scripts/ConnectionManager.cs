using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    class ConnectionManager : MonoBehaviour
    {
        static string Domain = "https://localhost:44365";
        //static string Domain = "https://learnablems20200220070049.azurewebsites.net";

        public static List<Question> Questions;
        public static List<Topic> Topics;
        public static IEnumerator Login(string username, string password)
        { 
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/login/" + username + "/" + password);
             yield return www.SendWebRequest();
            bool result = bool.Parse(www.downloadHandler.text);
            LoginController.Result(result);
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
            GameObject.FindGameObjectsWithTag("Page").Where(z => z.name.ToLower().Contains("section")).First().GetComponent<SectionController>().SetCurrentPage();
        }
    }
    
}
