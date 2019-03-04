using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Physics.gravity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        switch (GameObject.Find("Dropdown").GetComponent<Dropdown>().value)
        {
            case 0:
                GameObject.Find("Global").GetComponent<Global>().GeneratePool();
                SceneManager.LoadSceneAsync("game");
                break;
            case 1:
                ((Multiplayer)NetworkManager.singleton).Match();
                break;
            case 2:
                GameObject.Find("Global").GetComponent<Global>().GeneratePool();
                SceneManager.LoadSceneAsync(5);
                break;
            case 3:
                GameObject.Find("Global").GetComponent<Global>().GeneratePool();
                SceneManager.LoadSceneAsync(6);
                break;
            case 4:
                GameObject.Find("Global").GetComponent<Global>().GeneratePool();
                SceneManager.LoadSceneAsync(7);
                break;
            case 5:
                GameObject.Find("Global").GetComponent<Global>().GeneratePool();
                SceneManager.LoadSceneAsync(8);
                break;
        }
    }
}
