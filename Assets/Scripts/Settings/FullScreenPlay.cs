using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenPlay : MonoBehaviour {
    public void OnValueChange()
    {
        PlayerPrefs.SetInt("Settings.Fullscreen", GetComponent<Toggle>().isOn ? 1 : 0);
    }
    // Use this for initialization
    void Start () {
        GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("Settings.Fullscreen") == 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
