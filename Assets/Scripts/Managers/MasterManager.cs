using UnityEngine;

[CreateAssetMenu(menuName = "Singleton/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField]
    private GameSettings _gameSettings; //GameSettings object
    
    // Get GameSettings to access Nickname and GameVersion
    public static GameSettings GameSettings { get { return Instance._gameSettings; } }
}

