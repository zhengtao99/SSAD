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
        //static string Domain = "https://localhost:44365";
        static string Domain = "https://learnablems20200220070049.azurewebsites.net";

        public static List<Question> Questions;

        public static IEnumerator Login(string username, string password)
        {
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/login/" + username + "/" + password);
            yield return www.SendWebRequest();
            GameManager.Instance.HideLoading();
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
            GameManager.Instance.HideLoading();
            Questions = questionCollection.Questions;

        }
        public static IEnumerator GetTopic(int WorldId)
        {
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/topics/" + WorldId);
            yield return www.SendWebRequest();
            string json = "{\"Topics\":" + www.downloadHandler.text + "}";
            var topicCollection = JsonUtility.FromJson<TopicCollection>(json);
            var topics = topicCollection.Topics;
        }
    }
    
}
