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
        //static string Domain = "https://localhost:44365";  //zt localhost
        static string Domain = "https://learnablems20200220070049.azurewebsites.net";

        public static IEnumerator Login(string username, string password)
        {
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/login/" + username + "/" + password);
            yield return www.SendWebRequest();
            bool result = bool.Parse(www.downloadHandler.text);
            LoginController.Result(result);
        }
        public static IEnumerator GetQuestions(int topicId, int stage)
        {
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/questions/" + topicId + "/" + stage);
            yield return www.SendWebRequest();
            string json = "{\"Questions\":" + www.downloadHandler.text + "}";

            var questionCollection = JsonUtility.FromJson<QuestionCollection>(json);
            MazeGenerator.questions = questionCollection.Questions;

        }
    }
    
}
