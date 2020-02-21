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
        //static string Domain = "https://localhost:44365";
        static string Domain = "https://learnablems20200220070049.azurewebsites.net";

        public static IEnumerator Login(string username, string password)
        { 
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/login/" + username + "/" + password);
             yield return www.SendWebRequest();
            Debug.Log("*" + www.downloadHandler.text);
            bool result = bool.Parse(www.downloadHandler.text);
            LoginController.Result(result);
        }
    }
    
}
