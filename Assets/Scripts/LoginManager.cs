using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;
    public GameObject loading;
    public GameObject loginPage;

    void Awake()
    {
        Instance = this;
    }

    public void ShowLoading()
    {
        loading.SetActive(true);
    }
    public void HideLoading()
    {
        loading.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
