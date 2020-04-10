using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniChestController : MonoBehaviour
{
    public static MiniChestController OpenedChestInstance;
    public GameObject openedChest;

    // Start is called before the first frame update
    void Start()
    {
        OpenedChestInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateOpenedChests(float x, float y, float z)
    {
        GameObject clone = null;

        clone = Instantiate(openedChest) as GameObject;
        Transform t = clone.transform;

        Vector3 pos = new Vector3(x, y, z);
        t.position = pos;
    }

    public void CloseOpenedChest(){
        GameObject [] openedChests = GameObject.FindGameObjectsWithTag("OpenedChest");
        foreach(GameObject chest in openedChests){
            //chest.SetActive(false);
            Destroy(chest);
        }
    }
}
