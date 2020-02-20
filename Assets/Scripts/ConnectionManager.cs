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
        public static IEnumerator Login()
        {
            UnityWebRequest www = UnityWebRequest.Get("https://localhost:44365/api/loginapi/aaa/123");
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                //Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }
    
}
