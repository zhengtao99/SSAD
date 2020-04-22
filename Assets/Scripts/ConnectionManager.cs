using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Assets.Model;
using Photon.Realtime;
using Photon.Pun;

namespace Assets.Scripts
{
    /// <summary>
    /// This manager class ontains all methods Learnable Mobile will use to retrieve or to update the database through Learnable Manage System's self-created API.
    /// </summary>
    class ConnectionManager
    {
        //static string Domain = "https://localhost:44365"; //ZT host

        /// <summary>
        /// A variable to store the domain's name that the Leanrnable Mobile will be using to update or retrieve students' records.
        /// </summary>
        static string Domain = "https://learnablems20200220070049.azurewebsites.net";

        /// <summary>
        /// A variable to store the student's identity upon successfully login.
        /// </summary>
        public static User user;

        /// <summary>
        /// A variable to store the world the student has selected.
        /// </summary>
        public static List<World> Worlds;
       
        /// <summary>
        /// A variable list to store the list of topics the subject contains the student has selected.
        /// </summary>
        public static List<Topic> Topics;

        /// <summary>
        /// A variable to store the retrieved questions according to a specific world and topic.
        /// </summary>
        public static List<Question> Questions;

        /// <summary>
        /// A variable to store the cleared stages according to the player's user id.
        /// </summary>
        public static List<ClearedStage> ClearedStages;

        /// <summary>
        /// A variable list to store the list of available stages that has questions created by the teachers.
        /// </summary>
        public static List<AvailableStage> AvailableStages;

        /// <summary>
        /// A variable list to store the list of high scores for the subject selected.
        /// </summary>
        public static List<Highscore> Highscores;

        /// <summary>
        /// A variable to store the choice of retrieving students records by world or topic.
        /// </summary>
        public static string Category;

        /// <summary>
        /// To authenticate the player after they enter their username and password.
        /// </summary>
        /// <param name="username">Username input.</param>
        /// <param name="password">Password input.</param>
        public static IEnumerator Login(string username, string password)
        {
            LoginManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/login/" + username + "/" + password);
             yield return www.SendWebRequest();
            LoginManager.Instance.HideLoading();
            try
            {
                user = JsonUtility.FromJson<User>(www.downloadHandler.text);
                SceneManager.LoadScene(1);         
            }
            catch(Exception)
            {
                LoginController.Result(www.downloadHandler.text.Replace("\"",""));
            }
            
        }

        /*
        public static void CreateRoom()
        {
            if (!PhotonNetwork.IsConnected) //if not connected
                return;

            //CreateRoom
            //JoinOrCreateRoom
            RoomOptions options = new RoomOptions();
            options.BroadcastPropsChangeToAll = true; //broadcast CustomProperties
            options.PublishUserId = true;
            options.MaxPlayers = 2;  //max players: 2

            //If exist room -> join, otherwise create
            PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.LocalPlayer.NickName, options, TypedLobby.Default);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Created room successfully");
        }
        */

        /*
        private static void ConnectToPhoton(string nickname)
        {
            Debug.Log("Connecting to server...");

            PhotonNetwork.NickName = nickname;
            PhotonNetwork.GameVersion = "0.0.0";

            PhotonNetwork.ConnectUsingSettings();  //connect by photon app id
        }

        public override void OnConnectedToMaster() //When connect to photon
        {
            Debug.Log("Connected to server");

            //Print nickname of LocalPlayer
            Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName); //print nickname

            if (!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)  //When disconnect to photon
        {
            Debug.Log("Disconnected from server for reason: " + cause.ToString());
        }

        
        public override void OnJoinedLobby()
        {
            Debug.Log("hello");
            GameManager.Instance.ModePage();
        }
        */


        /// <summary>
        /// This method is used to retrieve respective questions from datbase based on player selected topic and difficulty level.
        /// </summary>
        /// <param name="topicId">Player's selected topic.</param>
        /// <param name="stage">Player's selected diffculty stage between 1 to 10.</param>
        public static IEnumerator GetQuestions(int topicId, int stage)
        {
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/questions/" + topicId + "/" + stage);
            yield return www.SendWebRequest();
            string json = "{\"Questions\":" + www.downloadHandler.text + "}";
            var questionCollection = JsonUtility.FromJson<QuestionCollection>(json);          
            Questions = questionCollection.Questions;
            GameManager.Instance.HideLoading();
            GameManager.Instance.createNewGame();
            GameManager.Instance.SetPageState(GameManager.PageState.Play);
        }

        /// <summary>
        /// This method is used to retrieve random questions based on player's user id in the multiplayer gameplay mode.
        /// </summary>
        /// <param name="Player">Player's user id</param>
        public IEnumerator GetRandomQuestions(string Player)
        {
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/questions/random");
            yield return www.SendWebRequest();
            string json = "{\"Questions\":" + www.downloadHandler.text + "}";
            var questionCollection = JsonUtility.FromJson<QuestionCollection>(json);
            Questions = questionCollection.Questions;
            Debug.Log("Got question: " + Questions.Count());
            if (Player == "Invitee")
            {
                GameObject.Find("Canvas").GetComponent<RoomController>().LoadMultiplayerScene();
            }
            if(Player == "Inviter")
            {
                GameManager.Instance.HideWaitingBoard();
                GameManager.Instance.MultiplayerMatchUI();
            }
        }

        /// <summary>
        /// This method is used to get the topics available for the world/subject the player has selected.
        /// </summary>
        /// <param name="WorldId">Player's selected world.</param>
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

        /// <summary>
        /// This method is used to get the world/subject available in the game.
        /// </summary>
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

        /// <summary>
        /// This method is used to get all the unlocked stages based on topic selected and the player's identity.
        /// </summary>
        /// <param name="TopicId">Player's selected topic</param>
        /// <param name="UserId">Player's user id.</param>
        public IEnumerator GetAvailableStages(int TopicId)
        { 
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/stages/" + TopicId );

            yield return www.SendWebRequest();         
            string json = "{\"AvailableStages\":" + www.downloadHandler.text + "}";           
            var stageCollection = JsonUtility.FromJson<AvailableStageCollection>(json);
            AvailableStages = stageCollection.AvailableStages;
            GameManager.Instance.loadClearedStages(TopicId);
        }

        /// <summary>
        /// This method is used to get all the cleared stages based on player's user id and topic selected.
        /// </summary>
        /// <param name="TopicId">Player's selected topic</param>
        public IEnumerator GetClearedStages(int TopicId)
        {
            Debug.Log("in");
            GameManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/stages/" + TopicId + "/" + user.Id);

            yield return www.SendWebRequest();
            string json = "{\"ClearedStages\":" + www.downloadHandler.text + "}";
            var stageCollection = JsonUtility.FromJson<ClearedStageCollection>(json);
            ClearedStages = stageCollection.ClearedStages;
            GameManager.Instance.HideLoading();
            GameManager.Instance.levelUI();
        }

        /// <summary>
        /// This method is used to update player's new highscore into database, the score obtained after clearing a stage for a topic.  
        /// </summary>
        /// <param name="UserId">Player's user id.</param>
        /// <param name="TopicId">Player's selected topic.</param>
        /// <param name="Stage">Player's selected stage.</param>
        /// <param name="Score">Player's score for the stage.</param>
        /// <param name="IsCleared">A check to verify if stage has been cleared.</param>
        public static IEnumerator UpdateHighscore(int UserId, int TopicId, int Stage, int Score, bool IsCleared)
        {
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/highscore/" + UserId + "/" + TopicId + "/" + Stage + "/" + Score + "/" + IsCleared);
            yield return www.SendWebRequest();
        }

        /// <summary>
        /// This method is used to update player's performance to database for analysis done at Learnable Management System.  
        /// </summary>
        /// <param name="UserId">Player's user id.</param>
        /// <param name="QnsId">The question answered.</param>
        /// <param name="AnsId">Player's selected answer.</param>
        /// <param name="Speed">Time for player to answer the question.</param>
        public IEnumerator SaveAnalytics(int UserId, int QnsId, int AnsId, TimeSpan Speed)
        {
            if (!GameObject.Find("QuestionPopUpPage").GetComponent<QAManager>().isMultiplayerMode)
                GameManager.Instance.ShowLoading();
            else
                MultiplayerSceneManager.Instance.ShowLoading();
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/analytics/" + UserId + "/" + QnsId + "/" + AnsId + "/" + Convert.ToInt32(Speed.TotalMilliseconds));
            yield return www.SendWebRequest();
            if (!GameObject.Find("QuestionPopUpPage").GetComponent<QAManager>().isMultiplayerMode)
                GameManager.Instance.HideLoading();
            else
                MultiplayerSceneManager.Instance.HideLoading();
            GameObject.Find("QuestionPopUpPage").GetComponent<QAManager>().continueButton.SetActive(true);
            if (AnsId != 1)
            {
                if (GameObject.Find("QuestionPopUpPage").GetComponent<QAManager>().isMultiplayerMode)
                    MultiplayerSceneManager.Instance.myPlayer.GetComponent<MyPlayerController>().LifeUpdate();
                else
                    GameObject.Find("QuestionPopUpPage").GetComponent<QAManager>().playerController.LifeUpdate();
            }
        }

        /// <summary>
        /// This method is used to retrieve player's accumulated score for the world or topic selected.
        /// </summary>
        /// <param name="Id">Player's selected world or topic id.</param>
        /// <param name="UserId">Player's user id.</param>
        /// <param name="category">Player's selected world or topic.</param>
        public IEnumerator GetCurrentUserScore(int Id, int UserId, string category)
        {
            Category = category;
            UnityWebRequest www = UnityWebRequest.Get(Domain + "/api/highscores/" + Category + "/" + Id + "/" + UserId);
            yield return www.SendWebRequest();
            string json = www.downloadHandler.text;
            Highscore highscore = null;
            if (json != "null")
            {
                 highscore = JsonUtility.FromJson<Highscore>(json);
            }
            GameManager.Instance.ViewLeaderboard();
            GameObject.Find("LeaderboardPage").GetComponent<RankListController>().SetLabels();
            GameObject.Find("LeaderboardPage").GetComponent<RankListController>().SetCurrentPlayerRank(highscore);
        }

        /// <summary>
        /// This method is used to retrieve a list of players that matches the player's search input.
        /// </summary>
        /// <param name="id">Player's selected world or topic id.</param>
        /// <param name="Search">Player's input in search box.</param>
        /// <param name="Filter">Player's selected world or topic.</param>
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
            yield return www.SendWebRequest();
            if (firstLoad)
            {
                GameObject.Find("LeaderboardPage").GetComponent<RankListController>().ClearRanks();
            }
            string json = www.downloadHandler.text;
            if (json != "")
            {
                var highscore = JsonUtility.FromJson<Highscore>(json);
                Highscores.Add(highscore);
                var ids = Highscores.Select(z => z.User.Id.ToString()).ToArray();
                var idStr = string.Join("-", ids);             
                GameObject.Find("LeaderboardPage").GetComponent<RankListController>().AddRank(id, Search, idStr, highscore, Category);
            }
        }
    }
    
}
