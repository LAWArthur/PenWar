using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsExit : MonoBehaviour {
    public int scene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Exit()
    {
        PlayerPrefs.Save();
        Camera.main.GetComponent<ButtonCtrl>().Listen();
        SceneManager.UnloadSceneAsync(scene);
    }
}
