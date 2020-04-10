using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class holds the methods to control the micro animations on the chests in the maze environment, when the player hits starts. Replaces closed with open chests when player collides with the chest.
/// </summary>
public class MiniChestController : MonoBehaviour
{
    /// <summary>
    /// An instance to allow other Game Objects' to access the methods implemented here.
    /// </summary>
    public static MiniChestController OpenedChestInstance;

    /// <summary>
    /// A variable that holds the game object that will be used to replace closed chests in the maze environment when the player opens a chest.
    /// </summary>
    public GameObject openedChest;

    /// <summary>
    /// This method is called before the first frame update.
    /// </summary>
    void Start()
    {
        OpenedChestInstance = this;
    }

    /// <summary>
    /// The method is responsbile for creation of openchests to replace the closed ones on the maze environment when the user opens a chest.
    /// </summary>
    /// <param name="x">X coordinate of the closed chest in the environment</param>
    /// <param name="y">Y coordinate of the closed chest in the environment</param>
    /// <param name="z">Z coordinate of the closed chest in the environment</param>
    public void CreateOpenedChests(float x, float y, float z)
    {
        GameObject clone = null;

        clone = Instantiate(openedChest) as GameObject;
        Transform t = clone.transform;

        Vector3 pos = new Vector3(x, y, z);
        t.position = pos;
    }

    /// <summary>
    /// The method is responsbile for closing of openchests in order to reinitialize the environment to start a new game.
    /// </summary>
    public void CloseOpenedChest(){
        GameObject [] openedChests = GameObject.FindGameObjectsWithTag("OpenedChest");
        foreach(GameObject chest in openedChests){
            //chest.SetActive(false);
            Destroy(chest);
        }
    }
}
