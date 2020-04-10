using UnityEngine;

//Create menu in asset
[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject  //inherit ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "0.0.0";
    public string GameVersion { get { return _gameVersion; } } //get _gameVersion
    [SerializeField]
    private string _nickName = "Thu";
    public string NickName  //get Nickname
    {
        get
        {
            //Random a number for nickname
            int value = UnityEngine.Random.Range(0, 9999);
            return _nickName + value.ToString();
        }
    }
}

